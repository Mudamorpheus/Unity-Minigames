using System.Collections.Generic;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Items;
using Game.Common.Scripts.Local.UI.Huds;

namespace Game.Common.Scripts.Local.UI.Shops 
{	
	public class ShopWindow : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private GameObject shopGrid;

        //================================
        //===INJECTS
        //================================

        [Inject] private PlayerManager playerManager;
        [Inject] private ShopManager shopManager;
        [Inject] private MenuHud gameHud;

        //================================
        //===FIELDS
        //================================

        private List<ShopItemSkin> shopItems = new List<ShopItemSkin>();

        //================================
        //===PROPERITES
        //================================

        public List<ShopItemSkin> Items { get { return shopItems; } }

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Awake()
        {
            LoadShop();
        }

        //================================
        //===SHOP
        //================================

        public void LoadShop()
        {
            var data = shopManager.Data;
            foreach (var skin in data.shop_skins_data)
            {
                var item = Instantiate(data.shop_skins_prefab, shopGrid.transform);
                var comp = item.GetComponent<ShopItemSkin>();
                comp.Initialize(playerManager, shopManager, gameHud, skin);
                shopItems.Add(comp);
            }
        }
    }
}
