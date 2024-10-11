using UnityEngine;

using Zenject;

using Game.Common.Scripts.Data;
using Game.Common.Scripts.UI.Popups;

namespace Game.Common.Scripts.DI.Cores.Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        #region

        [SerializeField] private SettingsPopup uiSettingsPopupPrefab;
        [SerializeField] private BankruptPopup uiBankruptPopupPrefab;
        [SerializeField] private InfoPopup     uiInfoPopupPrefab;        

        #endregion

        //===========================================

        #region Zenject

        public override void InstallBindings()
        {
            BindData();
            BindFactories();
        }

        public void BindData()
        {
            Container.Bind<PlayerData>().FromNew().AsSingle().NonLazy();
        }

        public void BindFactories()
        {
            Container.BindFactory<SettingsPopup, SettingsPopup.Factory>();
            SettingsPopup.Factory.BindPrefab(uiSettingsPopupPrefab);
            Container.BindFactory<BankruptPopup, BankruptPopup.Factory>();
            BankruptPopup.Factory.BindPrefab(uiBankruptPopupPrefab);
            Container.BindFactory<InfoPopup, InfoPopup.Factory>();
            InfoPopup.Factory.BindPrefab(uiInfoPopupPrefab);
        }

        #endregion
    }
}