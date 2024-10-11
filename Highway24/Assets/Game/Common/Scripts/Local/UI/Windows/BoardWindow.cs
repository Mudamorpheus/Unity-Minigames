using System.Collections.Generic;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.UI.Items;
using Game.Common.Scripts.Local.UI.Huds;

namespace Game.Common.Scripts.Local.UI.Shops
{
    public class BoardWindow : MonoBehaviour
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private GameObject boardLayout;

        //================================
        //===INJECTS
        //================================

        [Inject] private PlayerManager playerManager;
        [Inject] private BoardManager boardManager;
        [Inject] private MenuHud gameHud;

        //================================
        //===FIELDS
        //================================

        private List<BoardItemChallenge> boardItems = new List<BoardItemChallenge>();

        //================================
        //===PROPERITES
        //================================

        public List<BoardItemChallenge> Items { get { return boardItems; } }

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
            var data = boardManager.Data;
            foreach (var challenge in data.board_challenges_data)
            {
                var item = Instantiate(data.board_challenges_prefab, boardLayout.transform);
                var comp = item.GetComponent<BoardItemChallenge>();
                comp.Initialize(playerManager, boardManager, gameHud, challenge);
                boardItems.Add(comp);
            }
        }
    }
}
