using UnityEngine;

using DG.Tweening;
using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Scenes.Entities
{
    public class Player : Entity
    {
        //======INSPECTOR

        [SerializeField] private Powerline[] powerlinesList;

        //======FIELD

        private float     healthCurrent = 1f;
        //
        private Powerline powerlineCurrent;        
        private int       powerlineIndex = 1;
        //
        private float     powerlineRecharge = 0f;
        private float     powerlineCooldown = 0.35f;

        //======PROPERTIES

        public float Health { get { return healthCurrent; } }

        //======MONOBEHAVIOUR

        private void Awake()
        {
            Switch(powerlineIndex);       
        }

        private void Update()
        {
            if(powerlineRecharge < powerlineCooldown)
            {
                powerlineRecharge += Time.deltaTime;
            }
        }

        //======PLAYER

        public void Defeat()
        {            
            sceneEngine.Defeat();
        }

        //======SWIPE

        public void SwipeLeft()
        {
            if (powerlineRecharge < powerlineCooldown) return;

            powerlineRecharge = 0f;
            powerlineIndex--;
            if(powerlineIndex < 0)
            {
                powerlineIndex = 0;
            }
            else
            {
                Switch(powerlineIndex);
            }
        }

        public void SwipeRight()
        {
            if (powerlineRecharge < powerlineCooldown) return;

            powerlineRecharge = 0f;
            powerlineIndex++;
            if (powerlineIndex >= powerlinesList.Length)
            {
                powerlineIndex = powerlinesList.Length-1;
            }
            else
            {
                Switch(powerlineIndex);
            }
        }

        //======STATS

        public void Heal(float heal)
        {
            //Value
            healthCurrent += heal;
            if (healthCurrent > 1f) { healthCurrent = 1f; }

            //Bar
            sceneHud.UpdateHealthIndicators();
        }

        public void Hurt(float dmg)
        {
            //Value
            healthCurrent -= dmg;

            //Regen
            sceneEngine.BreakRegen();

            //Bar
            sceneHud.UpdateHealthIndicators();

            //Defeat
            if (healthCurrent <= 0)
            {
                Defeat();
            }
        }

        public void Switch(int index)
        {
            if (powerlineCurrent) { powerlineCurrent.Deactivate(); }
            powerlineCurrent = powerlinesList[index];
            powerlineCurrent.Activate();
            gameObject.transform.DOMove(powerlineCurrent.transform.position, 0.25f);
        }

        //======COLLISION

        private void OnTriggerEnter2D(Collider2D collider)
        {
            Entity entity = collider.gameObject.GetComponent<Entity>();

            if(entity != null)
            {
                if(entity as Wall)
                {
                    //Damage
                    float dmg = ((Wall)entity).Damage;
                    Hurt(dmg);

                    //Sound
                    SoundSystem.Instance.PlaySound("Crash");

                    //Clear
                    Destroy(entity.gameObject);
                }
            }
        }
    }
}

