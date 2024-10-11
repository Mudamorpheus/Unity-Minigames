using UnityEngine;

using Zenject;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;
using Game.Common.Scripts.UI.Buttons;

namespace Game.Common.Scripts.UI.Popups
{
    public class SettingsPopup : BasePopup
    {
        #region Fields

        [SerializeField] private ButtonSwitcher uiMusicSwitcher;
        [SerializeField] private ButtonSwitcher uiSoundSwitcher;

        #endregion

        //===========================================

        #region BasePopup

        public override void Init(BaseHud hud, BaseEngine engine)
        {
            base.Init(hud, engine);

            uiMusicSwitcher.Switch(sceneHud.PlayerData.Music);
            uiSoundSwitcher.Switch(sceneHud.PlayerData.Sound);
        }

        #endregion

        //===========================================

        #region Factory

        public class Factory : PlaceholderFactory<SettingsPopup>
        {
            //Prefab
            private static SettingsPopup factoryPrefab;
            public static void BindPrefab(SettingsPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            //Init
            public SettingsPopup Create(BaseHud hud, BaseEngine engine)
            {
                SettingsPopup popup = Instantiate(factoryPrefab, hud.Canvas.transform);

                popup.Init(hud, engine);
                popup.Show();

                return popup;
            }
        }

        #endregion

        //===========================================

        #region Audio

        public void SwitchMusic()
        {
            uiMusicSwitcher.Switch(!sceneHud.PlayerData.Music);
            sceneHud.SwitchMusic();
        }

        public void SwitchSound()
        {
            uiSoundSwitcher.Switch(!sceneHud.PlayerData.Sound);
            sceneHud.SwitchSound();
        }

        #endregion
    }
}