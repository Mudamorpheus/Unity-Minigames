using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Misc;

namespace Game.Common.Scripts.Local.UI.Popups
{
    public class PausePopup : Popup
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private ButtonSwitcher uiMusicButtonOn;
        [SerializeField] private ButtonSwitcher uiMusicButtonOff;
        [SerializeField] private ButtonSwitcher uiSoundButtonOn;
        [SerializeField] private ButtonSwitcher uiSoundButtonOff;

        //================================
        //===FIELDS
        //================================

        private RoadwayHud uiHud;
        private PlayerManager playerManager;

        //===========================
        //===FACTORY
        //===========================

        public class Factory : PlaceholderFactory<PausePopup>
        {
            private static PausePopup factoryPrefab;

            public static void BindPrefab(PausePopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public PausePopup Create(RoadwayHud uiHud, PlayerManager playerManager)
            {
                var popup = Instantiate(factoryPrefab, uiHud.Canvas.transform);
                popup.Initialize(uiHud, playerManager);
                popup.Open();
                return popup;
            }
        }

        //================================
        //===UI
        //================================

        public void Initialize(RoadwayHud uiHud, PlayerManager playerManager)
        {
            this.uiHud = uiHud;
            this.playerManager = playerManager;

            SwitchMusic(playerManager.Music);
            SwitchSound(playerManager.Sound);
        }

        public void SwitchMusic(bool state)
        {
            playerManager.SwitchMusic(state);
            uiMusicButtonOn.Switch(!state);
            uiMusicButtonOff.Switch(state);
            uiHud.SwitchMusic(state);
        }

        public void SwitchSound(bool state)
        {
            playerManager.SwitchSound(state);
            uiSoundButtonOn.Switch(!state);
            uiSoundButtonOff.Switch(state);
            uiHud.SwitchSound(state);
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

        public void Resume()
        {
            Close();
            uiHud.PauseGame();
        }
    }
}
