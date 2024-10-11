using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.UI.Huds;

namespace Game.Common.Scripts.DI.Installers 
{	
	public class MenuInstaller : MonoInstaller
	{
        //================================
        //===INSPECTOR
        //================================
        
        [SerializeField] private MenuHud uiHud;

        //================================
        //===INSTALLER
        //================================

        public override void InstallBindings()
	    {
			BindHud();
	    }

        private void BindHud()
        {
            Container.BindInstance<MenuHud>(uiHud).AsSingle().NonLazy();
        }
    }
}