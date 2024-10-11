using UnityEngine;

using Zenject;

using TMPro;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Popups;
using Game.Common.Scripts.Local.UI.Controllers;
using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Players;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.UI.Huds
{
    public class RoadwayHud : Hud
    {
        //================================
        //===INSPECTOR
        //================================

        [Header("Game UI")]
        [SerializeField] private TMP_Text[] uiCurrentCoinsTexts;
        [SerializeField] private TMP_Text[] uiCurrentScoreTexts;
        [SerializeField] private TMP_Text[] uiLevelTexts;        
        [SerializeField] private DragHorizontalController uiPlayerController;

        //================================
        //===INJECTS
        //================================

        [Inject] private ShopManager shopManager;
        [Inject] private RoadwayCore gameCore;
        [Inject] private PlayerEntity playerEntity;
        [Inject] private PausePopup.Factory pausePopupFactory;
        [Inject] private GameoverPopup.Factory gameoverPopupFactory;
        [Inject] private NextPopup.Factory nextPopupFactory;

        //================================
        //===UI
        //================================

        public override void Initialize()
        {
            base.Initialize();

            //Values
            UpdateLevelTexts();

            //Controllers
            uiPlayerController.OnDragEvent.AddListener(playerEntity.Move);

            //Player
            string id = playerManager.GetSelectedSkin().skin_id;
            Sprite skin = shopManager.Data.shop_skins_data.Find(x => x.skin_id == id).skin_sprite;
            playerEntity.SetSkin(skin);
        }

        public void UpdateCurrentCoins()
        {
            foreach (var text in uiCurrentScoreTexts)
            {
                text.text = gameCore.Coins.ToString();
            }
        }
        public void UpdateCurrentScore()
        {
            foreach (var text in uiCurrentScoreTexts)
            {
                text.text = gameCore.Score.ToString();
            }
        }
        public void UpdateLevelTexts()
        {
            foreach (var text in uiLevelTexts)
            {
                text.text = "Level " + gameCore.Level;
            }
        }

        public void ShowPausePopup()
        {
            ResumeGame();
            pausePopupFactory.Create(this, playerManager);
        }
        public void ShowGameoverPopup()
        {
            ResumeGame();
            gameoverPopupFactory.Create(gameCore, this, playerManager);
        }
        public void ShowNextPopup()
        {
            ResumeGame();
            nextPopupFactory.Create(this);
        }

        public void SwitchPlayerController(bool state)
        {
            uiPlayerController.enabled = state;
        }

        //================================
        //===GAME
        //================================

        public void PauseGame()
        {
            gameCore.Resume();
        }

        public void ResumeGame()
        {
            gameCore.Pause();
        }

        public void RestartGame()
        {
            int level = gameCore.Level+1;
            gameCore.Stop();
            gameCore.Run(level);
        }
    }
}
