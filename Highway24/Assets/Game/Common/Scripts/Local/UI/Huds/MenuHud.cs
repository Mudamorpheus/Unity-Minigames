using UnityEngine;

using Zenject;

using TMPro;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Global.Systems;
using Game.Common.Scripts.Local.UI.Popups;
using Game.Common.Scripts.Local.UI.Misc;
using Game.Common.Scripts.Local.UI.Shops;
using Game.Common.Scripts.Local.UI.Items;

namespace Game.Common.Scripts.Local.UI.Huds 
{	
	public class MenuHud : Hud
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private TMP_Text[]     uiIncidentsTexts;
        [SerializeField] private UniWebView     uiWebView;
        [SerializeField] private ButtonSwitcher uiIncidentsSubButtonSwitcher;
        [SerializeField] private ShopWindow     uiShopWindow;

        //================================
        //===INJECTS
        //================================

        [Inject] private ShopManager           shopManager;        
        [Inject] private SettingsPopup.Factory settingsPopupFactory;
        [Inject] private TutorialPopup.Factory tutorialPopupFactory;

        //================================
        //===UI
        //================================

        public override void Initialize()
        {
            base.Initialize();

            UpdateIncidentsTexts();
            
        }

        public void UpdateIncidentsTexts()
        {
            bool zero = (playerManager.Data.player_incidents != 0);
            uiIncidentsSubButtonSwitcher.Switch(zero);
            foreach (var text in uiIncidentsTexts)
            {
                text.text = playerManager.Data.player_incidents.ToString();
            }            
        }

        public void ShowSettingsPopup()
        {
            settingsPopupFactory.Create(this, playerManager);
        }
        public void ShowTutorialPopup()
        {
            tutorialPopupFactory.Create(this, playerManager);
        }

        public void AddIncidents(int incidents)
        {
            int newValue = playerManager.Data.player_incidents + incidents;
            if (newValue >= 0)
            {
                playerManager.AddIncidents(incidents);
                UpdateIncidentsTexts();
                playerManager.SaveProfile();
            }                        
        }

        //================================
        //===SHOP
        //================================

        public void Buy(string id, int price)
        {
            playerManager.BuySkin(id);
            playerManager.AddCoins(-price);
            foreach(var item in uiShopWindow.Items)
            {
                var type = PlayerManager.PlayerSkin.State.Sold;
                if (item.State == type)
                {
                    item.Switch(type);
                }                
            }
            UpdateCoinTexts();
        }

        public void Select(string id)
        {            
            var prevItem = uiShopWindow.Items.Find(x => x.Id == playerManager.GetSelectedSkin().skin_id);
            var nextItem = uiShopWindow.Items.Find(x => x.Id == id);
            playerManager.SelectSkin(id);
            var prevComp = prevItem.GetComponent<ShopItemSkin>();
            var nextComp = nextItem.GetComponent<ShopItemSkin>();
            prevComp.Switch(PlayerManager.PlayerSkin.State.Unselected);
            nextComp.Switch(PlayerManager.PlayerSkin.State.Selected);
        }
    }
}
