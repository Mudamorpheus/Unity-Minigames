using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.Game.Players;
using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Animators;

namespace Game.Common.Scripts.DI.Installers
{

    public class RoadwayInstaller : MonoInstaller
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private RoadwayCore gameCore;
        [SerializeField] private RoadwayHud uiHud;        
        [SerializeField] private PlayerEntity playerEntity;

        //================================
        //===INSTALLER
        //================================

        public override void InstallBindings()
        {
            BindGame();
            BindHud();
            BindPlayer();
        }

        private void BindGame()
        {
            Container.BindInstance<RoadwayCore>(gameCore).AsSingle().NonLazy();
            Container.BindInstance<RoadwayAnimator>(new RoadwayAnimator()).AsSingle().NonLazy();
        }

        private void BindHud()
        {
            Container.BindInstance<RoadwayHud>(uiHud).AsSingle().NonLazy();
        }

        private void BindPlayer()
        {
            Container.BindInstance<PlayerEntity>(playerEntity).AsSingle().NonLazy();
        }
    }
}