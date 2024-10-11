using System;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Common.Scripts.Global.Managers 
{	
	public class ShopManager
	{
        //================================
        //===STRUCTS
        //================================

        [Serializable]
        public struct ShopSkin
        {
            //Data
            public string skin_id;
            public Sprite skin_sprite;
            public int    skin_price;
            public bool   skin_own;

            //Constructors
            public ShopSkin(string id, Sprite sprite, int price, bool own)
            {
                skin_id     = id;
                skin_sprite = sprite;
                skin_price  = price;
                skin_own    = own;
            }
        }

        [Serializable]
        public struct ShopData
        {
            //Data
            public List<ShopSkin> shop_skins_data;
            public GameObject     shop_skins_prefab;

            //Constructors
            public ShopData(List<ShopSkin> skins, GameObject prefab)
            {
                shop_skins_data = skins;
                shop_skins_prefab = prefab;
            }
        }

        //================================
        //===FIELDS
        //================================

        private ShopData shopData;

        //================================
        //===PROPERTIES
        //================================

        public ShopData Data { get { return shopData; } }

        //================================
        //===SHOP
        //================================

        public void Initialize(ShopData data)
        {
            shopData = data;
        }
    }

}
