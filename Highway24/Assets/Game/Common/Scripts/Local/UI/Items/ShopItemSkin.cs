using UnityEngine;
using UnityEngine.UI;

using Zenject;

using TMPro;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Misc;
using Game.Common.Scripts.Local.UI.Huds;
using Game.Common.Scripts.Local.UI.Popups;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.UI.Items 
{	
	public class ShopItemSkin : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private Image          skinImage;
        [SerializeField] private TMP_Text       skinPriceText;
        [SerializeField] private GameObject     skinSectionSelect;
        [SerializeField] private GameObject     skinSectionUnselect;
        [SerializeField] private GameObject     skinSectionBuy;                
        [SerializeField] private ButtonSwitcher skinButtonSwitcherBuy;

        //================================
        //===FIELDS
        //================================
        
        private PlayerManager                  playerManager;
        private ShopManager                    shopManager;
        private MenuHud                        gameHud;
        private string                         itemId;
        private int                            itemPrice;
        private PlayerManager.PlayerSkin.State itemState;

        //================================
        //===PROPERTIES
        //================================

        public string                         Id    { get { return itemId; } }
        public PlayerManager.PlayerSkin.State State { get { return itemState; } }

        //================================
        //===ITEM
        //================================

        public void Initialize(PlayerManager manager, ShopManager shop, MenuHud hud, ShopManager.ShopSkin skin)
        {
            playerManager = manager;
            shopManager   = shop;
            gameHud       = hud;
            itemId        = skin.skin_id;
            itemPrice     = skin.skin_price;

            skinImage.sprite   = skin.skin_sprite;
            skinPriceText.text = skin.skin_price.ToString();

            var playerskin = manager.GetSkin(skin.skin_id);
            var state = playerskin.skin_state;

            Switch(state);
        }

        public void Switch(PlayerManager.PlayerSkin.State state)
        {
            itemState = state;

            switch(state)
            {
                case PlayerManager.PlayerSkin.State.Sold:
                {
                    bool enough = (playerManager.Data.player_coins >= itemPrice);
                    skinSectionSelect.SetActive(false);
                    skinSectionUnselect.SetActive(false);
                    skinSectionBuy.SetActive(true);
                    skinButtonSwitcherBuy.Switch(enough);
                    break;
                }
                case PlayerManager.PlayerSkin.State.Selected:
                {
                    skinSectionSelect.SetActive(false);
                    skinSectionUnselect.SetActive(true);
                    skinSectionBuy.SetActive(false);
                    break;
                }
                case PlayerManager.PlayerSkin.State.Unselected:
                {
                    skinSectionSelect.SetActive(true);
                    skinSectionUnselect.SetActive(false);
                    skinSectionBuy.SetActive(false);
                    break;
                }
            }
        }

        public void Buy()
        {
            gameHud.Buy(itemId, itemPrice);
            Switch(PlayerManager.PlayerSkin.State.Unselected);
        }

        public void Select()
        {
            gameHud.Select(itemId);
        }

        public void PlayButtonTap()
        {
            SoundSystem.Instance.PlaySound("tap");
        }
    }
}
