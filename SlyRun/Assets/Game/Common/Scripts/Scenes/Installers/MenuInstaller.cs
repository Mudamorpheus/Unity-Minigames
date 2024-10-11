using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;

namespace Game.Common.Scripts.Scenes.Installers
{
    public class MenuInstaller : MonoInstaller
    {
        //======INSPECTOR

        [SerializeField] private MenuHud    sceneHud;
        [SerializeField] private MenuEngine sceneEngine;

        //======ZENJECT

        public override void InstallBindings()
        {
            BindHuds();
            BindEngines();
        }

        private void BindHuds()
        {
            Container.BindInstance<BaseHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<MenuHud>(sceneHud).AsSingle().NonLazy();
        }

        private void BindEngines()
        {
            Container.BindInstance<BaseEngine>(sceneEngine).AsSingle().NonLazy();
            Container.BindInstance<MenuEngine>(sceneEngine).AsSingle().NonLazy();
        }
    }
}