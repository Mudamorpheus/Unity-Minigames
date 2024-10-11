using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Systems;
using Game.Common.Scripts.Scenes.Popups;

namespace Game.Common.Scripts.Scenes.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        //======INSPECTOR

        [SerializeField] private DefeatPopup uiPopupDefeatPrefab;
        [SerializeField] private PausePopup uiPopupPausePrefab;
        [SerializeField] private TutorialPopup uiPopupTutorialPrefab;

        //======ZENJECT

        public override void InstallBindings()
        {
            BindFactories();
            BindSystems();
        }

        public void BindFactories()
        {
            Container.BindFactory<DefeatPopup, DefeatPopup.Factory>();
            DefeatPopup.Factory.BindPrefab(uiPopupDefeatPrefab);
            Container.BindFactory<PausePopup, PausePopup.Factory>();
            PausePopup.Factory.BindPrefab(uiPopupPausePrefab);
            Container.BindFactory<TutorialPopup, TutorialPopup.Factory>();
            TutorialPopup.Factory.BindPrefab(uiPopupTutorialPrefab);
        }

        private void BindSystems()
        {
            Container.Bind<AccountSystem>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }
    }
}