using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Misc;

namespace Game.Common.Scripts.Local.UI.Popups
{
    public class NextPopup : Popup
    {
        //================================
        //===FIELDS
        //================================

        private RoadwayHud uiHud;

        //===========================
        //===FACTORY
        //===========================

        public class Factory : PlaceholderFactory<NextPopup>
        {
            private static NextPopup factoryPrefab;

            public static void BindPrefab(NextPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public NextPopup Create(RoadwayHud uiHud)
            {
                var popup = Instantiate(factoryPrefab, uiHud.Canvas.transform);
                popup.Initialize(uiHud);
                popup.Open();
                return popup;
            }
        }

        //================================
        //===UI
        //================================

        public void Initialize(RoadwayHud uiHud)
        {
            this.uiHud = uiHud;
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

        //================================
        //===GAME
        //================================

        public void Restart()
        {
            Close();
            uiHud.RestartGame();
        }
    }
}
