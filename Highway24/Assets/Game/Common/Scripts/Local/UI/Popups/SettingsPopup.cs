using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Misc;

namespace Game.Common.Scripts.Local.UI.Popups 
{	
	public class SettingsPopup : Popup
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

        private MenuHud uiHud;
        private PlayerManager playerManager;        

        //===========================
        //===FACTORY
        //===========================

        public class Factory : PlaceholderFactory<SettingsPopup>
        {
            private static SettingsPopup factoryPrefab;

            public static void BindPrefab(SettingsPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public SettingsPopup Create(MenuHud uiHud, PlayerManager playerManager)
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

        public void Initialize(MenuHud uiHud, PlayerManager playerManager)
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
    }
}
