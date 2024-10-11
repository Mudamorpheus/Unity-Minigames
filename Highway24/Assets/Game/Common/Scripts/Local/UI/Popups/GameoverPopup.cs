using UnityEngine;
using UnityEngine.UI;

using Zenject;

using TMPro;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Misc;
using Game.Common.Scripts.Local.Game.Cores;

namespace Game.Common.Scripts.Local.UI.Popups
{
    public class GameoverPopup : Popup
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private TMP_Text uiScoreText;
        [SerializeField] private TMP_Text uiBestScoreText;
        [SerializeField] private TMP_Text uiCoinsCollectedText;
        [SerializeField] private TMP_Text uiCoinsTotalText;

        //================================
        //===FIELDS
        //================================

        private RoadwayCore gameCore;
        private RoadwayHud uiHud;
        private PlayerManager playerManager;

        //===========================
        //===FACTORY
        //===========================

        public class Factory : PlaceholderFactory<GameoverPopup>
        {
            private static GameoverPopup factoryPrefab;

            public static void BindPrefab(GameoverPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public GameoverPopup Create(RoadwayCore core, RoadwayHud hud, PlayerManager manager)
            {
                var popup = Instantiate(factoryPrefab, hud.Canvas.transform);
                popup.Initialize(core, hud, manager);
                popup.Open();
                return popup;
            }
        }

        //================================
        //===UI
        //================================

        public void Initialize(RoadwayCore core, RoadwayHud hud, PlayerManager manager)
        {
            gameCore = core;
            uiHud = hud;
            playerManager = manager;

            uiScoreText.text = "Score: " + core.Score.ToString();
            uiBestScoreText.text = "Best score: " + manager.Data.player_best_score.ToString();
            uiCoinsCollectedText.text = "Coins collected: " + core.Coins.ToString();
            uiCoinsTotalText.text = "Coins Total: " + manager.Data.player_coins.ToString();
        }

        public override void Open()
        {
            base.Open();

            uiHud.SwitchButtons(false);
        }
        public override void Close()
        {
            base.Close();

            uiHud.SwitchButtons(true);
        }
    }
}
