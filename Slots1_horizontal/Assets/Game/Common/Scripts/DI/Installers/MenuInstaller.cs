using UnityEngine;
using UnityEngine.UI;

using Zenject;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;

namespace Game.Common.Scripts.DI.Cores.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        #region Vars

        [SerializeField] private MenuHud      sceneHud;
        [SerializeField] private MenuEngine   sceneEngine;
        [SerializeField] private Canvas       sceneCanvas;
        [SerializeField] private CanvasScaler sceneScaler;

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
            Container.BindInstance<MenuHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<BaseEngine>(sceneEngine).AsSingle().NonLazy();
            Container.BindInstance<MenuEngine>(sceneEngine).AsSingle().NonLazy();
        }

        public void BindComponents()
        {
            Container.BindInstance<Canvas>(sceneCanvas).AsSingle().NonLazy();
            Container.BindInstance<CanvasScaler>(sceneScaler).AsSingle().NonLazy();
        }

        #endregion
    }
}