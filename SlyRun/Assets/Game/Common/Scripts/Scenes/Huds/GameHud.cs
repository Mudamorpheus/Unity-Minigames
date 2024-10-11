using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Zenject;

using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Entities;
using Game.Common.Scripts.Scenes.Controllers;

namespace Game.Common.Scripts.Scenes.Huds
{
    public class GameHud : BaseHud
    {
        //======INSPECTOR

        [SerializeField] private Image[]           uiHealthIndicators;
        [SerializeField] private TMP_Text[]        uiScoreIndicators;        
        [SerializeField] private Animator[]        uiAnimators;
        [SerializeField] private SwipeController[] uiSwipeControllers;

        //======INJECTS

        [Inject] private Player scenePlayer;

        //======HUD        

        public override void Init()
        {
            base.Init();

            UpdateScoreIndicators();
        }

        //======GAME

        public void UpdateScoreIndicators()
        {
            if(sceneEngine as GameEngine)
            {
                foreach (var indicator in uiScoreIndicators)
                {
                    indicator.text = "Score: " + ((GameEngine)sceneEngine).Score.ToString();
                }
            }            
        }
        public void UpdateHealthIndicators()
        {
            if (sceneEngine as GameEngine)
            {
                foreach (var indicator in uiHealthIndicators)
                {
                    indicator.fillAmount = scenePlayer.Health;
                }
            }
        }

        //

        public void UpdateAnimations()
        {
            foreach (var animator in uiAnimators)
            {
                animator.speed = ((GameEngine)sceneEngine).Speed * ((GameEngine)sceneEngine).Haste;
            }
        }
        public void ContinueAnimations()
        {
            foreach(var animator in uiAnimators)
            {
                animator.speed = ((GameEngine)sceneEngine).Speed * ((GameEngine)sceneEngine).Haste;
            }
        }
        public void PauseAnimations()
        {
            foreach (var animator in uiAnimators)
            {
                animator.speed = 0f;
            }
        }

        //

        public void ContinueControllers()
        {
            foreach(var controller in uiSwipeControllers)
            {
                controller.gameObject.SetActive(true);
            }
        }

        public void PauseControllers()
        {
            foreach (var controller in uiSwipeControllers)
            {
                controller.gameObject.SetActive(false);
            }
        }
    }
}