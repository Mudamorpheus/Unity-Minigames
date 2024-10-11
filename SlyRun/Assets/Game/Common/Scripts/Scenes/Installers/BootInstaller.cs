using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Systems;
using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;

namespace Game.Common.Scripts.Scenes.Installers
{
    public class BootInstaller : MonoInstaller
    {
        //======INSPECTOR

        [SerializeField] private BootHud    sceneHud;
        [SerializeField] private BootEngine sceneEngine;

        //======ZENJECT

        public override void InstallBindings()
        {
            BindHuds();
            BindEngines();
        }

        private void BindHuds()
        {
            Container.BindInstance<BaseHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<BootHud>(sceneHud).AsSingle().NonLazy();
        }

        private void BindEngines()
        {
            Container.BindInstance<BaseEngine>(sceneEngine).AsSingle().NonLazy();
            Container.BindInstance<BootEngine>(sceneEngine).AsSingle().NonLazy();
        }
    }
}