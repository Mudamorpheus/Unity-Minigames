using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Entities;

namespace Game.Common.Scripts.Scenes.Installers
{
    public class GameInstaller : MonoInstaller
    {
        //======INSPECTOR
        
        [SerializeField] private GameHud    sceneHud;
        [SerializeField] private GameEngine sceneEngine;
        [SerializeField] private Player     scenePlayer;

        //======ZENJECT

        public override void InstallBindings()
        {
            BindHuds();
            BindEngines();
            BindEntities();
        }

        private void BindHuds()
        {
            Container.BindInstance<BaseHud>(sceneHud).AsSingle().NonLazy();
            Container.BindInstance<GameHud>(sceneHud).AsSingle().NonLazy();
        }

        private void BindEngines()
        {
            Container.BindInstance<BaseEngine>(sceneEngine).AsSingle().NonLazy();
            Container.BindInstance<GameEngine>(sceneEngine).AsSingle().NonLazy();
        }

        private void BindEntities()
        {
            Container.BindInstance<Player>(scenePlayer).AsSingle().NonLazy();
        }
    }
}