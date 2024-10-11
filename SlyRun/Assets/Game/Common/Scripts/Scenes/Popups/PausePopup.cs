using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Misc.UI;

namespace Game.Common.Scripts.Scenes.Popups
{
    public class PausePopup : BasePopup
    {
        //======INSPECTOR

        [SerializeField] private ButtonSwitcher uiAudioSwitcher;

        //======FACTORY

        public class Factory : PlaceholderFactory<PausePopup>
        {
            private static PausePopup factoryPrefab;

            public static void BindPrefab(PausePopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            public PausePopup Create(BaseHud hud, BaseEngine engine)
            {
                var popup = Instantiate(factoryPrefab, hud.Canvas.transform);
                popup.sceneHud = hud;
                popup.sceneEngine = engine;

                popup.Init();
                popup.Open();

                return popup;
            }
        }

        //======BASE

        public override void Open()
        {
            base.Open();

            if (sceneEngine as GameEngine)
            {
                ((GameEngine)sceneEngine).Pause();
            }
        }
        public override void Close()
        {
            base.Close();

            if (sceneEngine as GameEngine)
            {
                ((GameEngine)sceneEngine).Continue();
            }
        }

        //======PAUSE

        public void Init()
        {
            uiAudioSwitcher.Switch(sceneHud.Account.Data.Audio);
        }

        public void SwitchAudio()
        {
            sceneHud.SwitchAudio();
        }
    }
}