using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;
using UnityEngine;

using Zenject;

namespace Game.Common.Scripts.DI.Cores.Installers
{
    public class BootInstaller : MonoInstaller
    {
        #region Vars

        [SerializeField] private BootHud    sceneHud;
        [SerializeField] private BootEngine sceneEngine;
        [SerializeField] private Canvas     sceneCanvas;

        #endregion

        //===========================================

        #region Zenject

        public override void InstallBindings()
        {
            BindCores();
            BindComponents();
        }

        public void BindCores()
        {
            Container.BindInstance<BaseHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<BootHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<BaseEngine>(sceneEngine).AsSingle().NonLazy();
            Container.BindInstance<BootEngine>(sceneEngine).AsSingle().NonLazy();
        }

        public void BindComponents()
        {
            Container.BindInstance<Canvas>(sceneCanvas).AsSingle().NonLazy();
        }

        #endregion
    }
}