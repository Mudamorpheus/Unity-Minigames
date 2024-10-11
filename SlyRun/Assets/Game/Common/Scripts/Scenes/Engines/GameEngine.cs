using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Entities;
using Game.Common.Scripts.Scenes.Spawners;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Scenes.Engines
{
    public class GameEngine : BaseEngine
    {
        //======INSPECTOR

        [SerializeField] private Spawner[] gameSpawners;
        [SerializeField] private float     gameSpeedGrowth = 0.02f;
        [SerializeField] private float     gameSpeedLimit  = 3f;
        [SerializeField] private int       gameScoreGrowth = 1;
        //
        [SerializeField] private float     healthRegen     = 0.066f;
        [SerializeField] private float     healthCooldown  = 5f;
        [SerializeField] private float     spawnCooldown   = 5f;
        [SerializeField] private int       spawnLifespan   = 8;

        //======FIELDS

        protected bool  gameActive     = false;
        protected float gameTick       = 0;
        //
        private float   gameSpeed      = 1f;
        private float   gameHaste      = 1f;
        private int     gameScore      = 0;
        //        
        private float   healthDecay    = 0f;        
        private float   healthTimer    = 0f;
        //        
        private float   spawnTimer     = 0f;

        //======INJECTS

        [Inject] private GameHud sceneHud;
        [Inject] private Player  scenePlayer;

        //======PROPERTIES

        public float Speed { get { return gameSpeed; } }
        public float Haste { get { return gameHaste; } }
        public int   Score { get { return gameScore; } }

        //======MONOBEHAVIOUR

        private void Update()
        {
            if (gameActive)
            {
                //Tick
                gameTick += Time.deltaTime * gameSpeed * gameHaste;
                if (gameTick >= 1f)
                {
                    UpdateTick();                    
                }

                //Timers                
                if(healthTimer < healthCooldown)
                {
                    healthTimer += Time.deltaTime;
                }
                if(spawnTimer < spawnCooldown)
                {
                    spawnTimer += Time.deltaTime;
                }
                else if(gameSpawners.Length > 0)
                {
                    spawnTimer = 0f;
                    int rnd = Random.Range(0, gameSpawners.Length-1);
                    var spawner = gameSpawners[rnd];
                    spawner.Spawn(spawnLifespan);
                }

                //Health                
                if (healthDecay > 0f)
                {
                    scenePlayer.Hurt(healthDecay * Time.deltaTime);
                }
                else if(healthTimer >= healthCooldown)
                {
                    scenePlayer.Heal(healthRegen * Time.deltaTime);
                }
            }
        }

        //======ENGINE

        private void UpdateTick()
        {
            //Game
            gameTick -= 1f;            
            if (gameSpeed < gameSpeedLimit) { gameSpeed += gameSpeedGrowth; }
            gameScore += gameScoreGrowth;

            //Visual
            sceneHud.UpdateScoreIndicators();
            sceneHud.UpdateAnimations();
        }

        public void Launch()
        {
            gameActive = true;
            Continue();
        }

        public void Defeat()
        {
            //Pause
            Pause();

            //Sound
            SoundSystem.Instance.PlaySound("GameOver");

            //Popup
            if (gameScore > sceneHud.Account.Data.Score) { sceneHud.SetBestScore(gameScore); }
            sceneHud.ShowPopupDefeat();
        }

        public void Continue()
        {
            //Game
            gameActive = true;

            //Animations
            sceneHud.ContinueAnimations();            

            //Entitites
            foreach(var entity in Entity.Entities)
            {
                entity.Continue();
            }
        }

        public void Pause()
        {
            //Game
            gameActive = false;

            //Animations            
            sceneHud.PauseAnimations();

            //Entitites
            foreach (var entity in Entity.Entities)
            {
                entity.Pause();
            }
        }

        //======PARAMS

        public void SetGameHaste(float haste)
        {
            gameHaste = haste;
        }
        public void SetGameDecay(float decay)
        {
            healthDecay = decay;
        }

        //======EFFECTS

        public void BreakRegen()
        {
            healthTimer = 0f;
        }
    }
}