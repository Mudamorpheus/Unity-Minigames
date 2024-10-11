using System;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.Game.Spawners;
using Game.Common.Scripts.Local.Game.Players;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.Game.Animators;
using Game.Common.Scripts.Local.Game.Entities;
using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Popups;
using static UniWebViewLogger;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.Game.Cores
{	
	public class RoadwayCore : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] int gameDistanceGoal = 1000;
        [SerializeField] int gameDistanceSpeed = 50;

        //================================
        //===FIELDS
        //================================

        private int   gameLevel;
        private int   gameDifficulty;

        private bool  gameActive    = false;
        private bool  gameFinish    = false;
        private float levelTick     = 0f;
        private int   levelDistance = 0;        
        private int   levelScore    = 0;
        private int   sessionScore  = 0;        
        private int   sessionCoins  = 0;

        //================================
        //===INJECTS
        //================================

        [Inject] private PlayerManager   playerManager;
        [Inject] private BoardManager    boardManager;
        [Inject] private RoadwayHud      uiHud;
        [Inject] private RoadwayAnimator gameAnimator;
        [Inject] private PlayerEntity    playerEntity;        

        //================================
        //===PROPERTIES
        //================================

        public bool  Active     { get { return gameActive; } }
        public int   Level      { get { return gameLevel; } }
        public int   Difficulty { get { return gameDifficulty; } }
        public int   Score      { get { return sessionScore; } }
        public int   Coins      { get { return sessionCoins; } }

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Update()
        {
            //Score
            if(gameActive)
            {
                //Tick
                levelTick += gameDistanceSpeed * Time.deltaTime;                
                int levelPoints = (int)levelTick;
                levelTick -= levelPoints;

                //Score
                if(levelScore < gameDifficulty*gameDistanceGoal)
                {
                    levelDistance += levelPoints;
                    levelScore    += gameDifficulty * levelPoints;
                    sessionScore  += gameDifficulty * levelPoints;
                    uiHud.UpdateCurrentScore();
                }                      
                
                //Finish
                if(!gameFinish && levelDistance >= gameDistanceGoal)
                {
                    gameFinish = true;
                    gameAnimator.MoveFinish();
                }
            }
        }

        //================================
        //===CORE
        //================================

        public void Initialize()
        {
            //Init
            gameLevel       = 1;
            gameDifficulty  = 1;

            //Map
            gameAnimator.MoveMap();
        }

        public void Run(int level)
        {
            //Game
            gameLevel = level;
            gameDifficulty = level;
            gameAnimator.Resume();
            gameAnimator.ResetFinish();            

            //Score
            gameActive = true;
            gameFinish = false;
            levelDistance = 0;
            levelScore = 0;

            //Player
            playerEntity.Run();

            //Entities
            foreach (var entity in Entity.Entities)
            {
                if(entity is EnemyEntity || entity is CoinEntity)
                {
                    Destroy(entity.gameObject);
                }                
            }

            //Spawners
            foreach (var spawner in Spawner.Spawners)
            {
                spawner.Run();
            }

            //UI
            uiHud.UpdateLevelTexts();
            uiHud.SwitchPlayerController(true);                        
        }

        public void Stop()
        {
            //Game
            gameActive = false;
            gameFinish = false;

            //Player
            playerEntity.Stop();

            //Spawners
            foreach (var spawner in Spawner.Spawners)
            {
                spawner.Stop();
            }

            //UI
            uiHud.SwitchPlayerController(false);
        }

        public void Resume()
        {
            //Game
            gameActive = true;
            gameAnimator.Resume();
            
            //Spawners
            foreach (var spawner in Spawner.Spawners)
            {
                spawner.Resume();
            }
            //Entities
            foreach(var entity in Entity.Entities)
            {
                entity.Resume();
            }

            //UI
            uiHud.SwitchPlayerController(true);
        }

        public void Pause()
        {
            //Game
            gameActive = false;
            gameAnimator.Pause();

            //Spawners
            foreach (var spawner in Spawner.Spawners)
            {
                spawner.Pause();
            }
            //Entities
            foreach (var entity in Entity.Entities)
            {
                entity.Pause();
            }

            //UI
            uiHud.SwitchPlayerController(false);
        }

        public void Lose()
        {
            SoundSystem.Instance.PlaySound("game over");
            Stop();
            CheckHighscoreChallenge(sessionScore);
            uiHud.SetBestScore(sessionScore);
            uiHud.ShowGameoverPopup();
            playerManager.SaveProfile();
        }

        public void Win()
        {
            Stop();
            CheckHighscoreChallenge(sessionScore);
            CheckLevelChallenge(gameLevel);
            uiHud.SetBestScore(sessionScore);
            uiHud.ShowNextPopup();
            playerManager.SaveProfile();
        }

        public void RewardCoins(int coins)
        {
            uiHud.RewardCoins(coins);
            sessionCoins += coins;
            CheckCoinsChallenge(sessionCoins);
        }

        //================================
        //===CHALLENGES
        //================================

        public void CheckLevelChallenge(int level)
        {
            foreach (var challenge in boardManager.Challenges)
            {
                if (challenge.challenge_requirement == BoardManager.BoardChallenge.Requirement.Levels)
                {
                    if (level >= challenge.challenge_requirement_value)
                    {
                        playerManager.ClaimChallenge(challenge.challenge_id);
                    }
                }
            }
        }
        public void CheckCoinsChallenge(int coins)
        {
            foreach (var challenge in boardManager.Challenges)
            {
                if (challenge.challenge_requirement == BoardManager.BoardChallenge.Requirement.Coins)
                {
                    if (coins >= challenge.challenge_requirement_value)
                    {
                        playerManager.ClaimChallenge(challenge.challenge_id);
                    }
                }
            }
        }
        public void CheckHighscoreChallenge(int score)
        {
            foreach (var challenge in boardManager.Challenges)
            {
                if (challenge.challenge_requirement == BoardManager.BoardChallenge.Requirement.Highscore)
                {
                    if (score >= playerManager.Data.player_best_score)
                    {
                        playerManager.ClaimChallenge(challenge.challenge_id);
                    }
                }
            }
        }
    }
}
