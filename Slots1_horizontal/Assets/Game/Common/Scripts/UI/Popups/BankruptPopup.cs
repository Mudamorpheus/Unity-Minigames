using UnityEngine;

using Zenject;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;

namespace Game.Common.Scripts.UI.Popups
{
    public class BankruptPopup : BasePopup
    {
        #region Factory

        public class Factory : PlaceholderFactory<BankruptPopup>
        {
            //Prefab
            private static BankruptPopup factoryPrefab;
            public static void BindPrefab(BankruptPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            //Init
            public BankruptPopup Create(BaseHud hud, BaseEngine engine)
            {
                BankruptPopup popup = Instantiate(factoryPrefab, hud.Canvas.transform);

                popup.Init(hud, engine);
                popup.Show();

                return popup;
            }
        }

        #endregion

        //===========================================

        public void SwitchBank()
        {
            Hide();
            if(sceneHud as MenuHud) { ((MenuHud)sceneHud).SwitchBank(); }            
        }
    }
}