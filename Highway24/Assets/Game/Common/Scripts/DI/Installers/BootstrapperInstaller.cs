using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.DI.Installers
{
    public class BootstrapperInstaller : MonoInstaller
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private SoundSystem audioSystem;

        //================================
        //===INSTALLER
        //================================

        public override void InstallBindings()
        {
            BindSystems();
        }

        private void BindSystems()
        {
            
        }
    }
}