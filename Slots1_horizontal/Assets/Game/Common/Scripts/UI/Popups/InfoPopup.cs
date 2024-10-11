using UnityEngine;

using Zenject;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;

namespace Game.Common.Scripts.UI.Popups
{
    public class InfoPopup : BasePopup
    {
        #region Factory

        public class Factory : PlaceholderFactory<InfoPopup>
        {
            //Prefab
            private static InfoPopup factoryPrefab;
            public static void BindPrefab(InfoPopup factoryPrefab)
            {
                Factory.factoryPrefab = factoryPrefab;
            }

            //Init
            public InfoPopup Create(BaseHud hud, BaseEngine engine)
            {
                InfoPopup popup = Instantiate(factoryPrefab, hud.Canvas.transform);

                popup.Init(hud, engine);
                popup.Show();

                return popup;
            }
        }

        #endregion
    }
}