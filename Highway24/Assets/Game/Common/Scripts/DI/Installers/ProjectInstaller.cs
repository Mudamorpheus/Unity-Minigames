using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Popups;

namespace Game.Common.Scripts.DI.Installers 
{
	
	public class ProjectInstaller : MonoInstaller
	{
		//================================
		//===INSPECTOR
		//================================

		[Header("Popups")]        
        [SerializeField] private SettingsPopup uiSettingsPopupPrefab;
        [SerializeField] private PausePopup    uiPausePopupPrefab;
        [SerializeField] private GameoverPopup uiGameoverPopupPrefab;
        [SerializeField] private NextPopup     uiNextPopupPrefab;
        [SerializeField] private TutorialPopup uiTutorialPopupPrefab;

        //================================
        //===INSTALLER
        //================================

        public override void InstallBindings()
	    {			
            BindManagers();
			BindFactories();			
	    }

        private void BindManagers()
		{
			Container.BindInstance<PlayerManager>(new PlayerManager()).AsSingle().NonLazy();            
            Container.BindInstance<ShopManager>(new ShopManager()).AsSingle().NonLazy();
            Container.BindInstance<BoardManager>(new BoardManager()).AsSingle().NonLazy();
        }

		private void BindFactories()
		{
            Container.BindFactory<SettingsPopup, SettingsPopup.Factory>();
			SettingsPopup.Factory.BindPrefab(uiSettingsPopupPrefab);

            Container.BindFactory<PausePopup, PausePopup.Factory>();
            PausePopup.Factory.BindPrefab(uiPausePopupPrefab);

            Container.BindFactory<GameoverPopup, GameoverPopup.Factory>();
            GameoverPopup.Factory.BindPrefab(uiGameoverPopupPrefab);

            Container.BindFactory<NextPopup, NextPopup.Factory>();
            NextPopup.Factory.BindPrefab(uiNextPopupPrefab);

            Container.BindFactory<TutorialPopup, TutorialPopup.Factory>();
            TutorialPopup.Factory.BindPrefab(uiTutorialPopupPrefab);
        }
	}
}