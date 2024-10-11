using UnityEngine;

using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Entities;
using Game.Common.Scripts.Local.UI.Huds;

using Zenject;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.Game.Players
{	
	public class PlayerEntity : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private SpriteRenderer playerSprite;
        [SerializeField] private float playerBorder = 4f;

        //================================
        //===FIELDS
        //================================

        private bool playerActive = false;

        //================================
        //===INJECTS
        //================================

        [Inject] private RoadwayCore gameCore;
        [Inject] private RoadwayHud uiHud;

        //================================
        //===PLAYER
        //================================

        public void Move(float normalized)
        {
            var position = gameObject.transform.position;
            gameObject.transform.localPosition = new Vector3(playerBorder*normalized, position.y, position.z);
        }

        public void Run()
        {
            playerActive = true;
        }
        public void Stop()
        {
            playerActive = false;
        }

        public void Win()
        {
            gameCore.Win();
        }
        public void Lose()
        {
            gameCore.Lose();
        }

        public void SetSkin(Sprite sprite)
        {
            playerSprite.sprite = sprite;
        }

        //================================
        //===COLLISION
        //================================

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if(playerActive)
            {
                Entity entity = collider.gameObject.GetComponent<Entity>();

                if (entity != null)
                {
                    //Enemy
                    if (entity is FinishEntity)
                    {
                        Win();
                    }
                    else if (entity is EnemyEntity)
                    {
                        Lose();
                    }
                    else if (entity is CoinEntity)
                    {
                        SoundSystem.Instance.PlaySound("coin");
                        CoinEntity coinEntity = (CoinEntity)entity;                        
                        int reward = coinEntity.Reward;
                        gameCore.RewardCoins(reward);
                    }

                    //Destroy
                    if (!entity.Invincible)
                    {
                        Destroy(entity.gameObject);
                    }
                }
            }       
        }
    }
}
