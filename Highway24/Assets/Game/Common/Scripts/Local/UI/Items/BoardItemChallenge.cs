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
    public class BoardItemChallenge : MonoBehaviour
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private TMP_Text   challengeDescText;
        [SerializeField] private TMP_Text   challengeRewardText;
        [SerializeField] private GameObject challengeSectionTodo;
        [SerializeField] private GameObject challengeSectionCompleted;
        [SerializeField] private GameObject challengeSectionClaim;

        //================================
        //===FIELDS
        //================================

        private PlayerManager playerManager;
        private BoardManager boardManager;
        private MenuHud gameHud;
        private string itemId;
        private int itemReward;

        //================================
        //===PROPERTIES
        //================================

        public string Id { get { return itemId; } }

        //================================
        //===ITEM
        //================================

        public void Initialize(PlayerManager manager, BoardManager board, MenuHud hud, BoardManager.BoardChallenge challenge)
        {
            playerManager = manager;
            boardManager  = board;
            gameHud       = hud;
            itemId        = challenge.challenge_id;
            itemReward    = challenge.challenge_reward;

            challengeDescText.text = challenge.challenge_desc;
            challengeRewardText.text = challenge.challenge_reward.ToString();

            var playerChallenge = manager.GetChallenge(challenge.challenge_id);
            var state = playerChallenge.challenge_state;

            Switch(state);
        }

        public void Switch(PlayerManager.PlayerChallenge.State state)
        {
            switch(state)
            {
                case PlayerManager.PlayerChallenge.State.Todo:
                {
                    challengeSectionTodo.SetActive(true);
                    challengeSectionCompleted.SetActive(false);
                    challengeSectionClaim.SetActive(false);
                    break;
                }
                case PlayerManager.PlayerChallenge.State.Completed:
                {
                    challengeSectionTodo.SetActive(false);
                    challengeSectionCompleted.SetActive(true);
                    challengeSectionClaim.SetActive(false);
                    break;
                }
                case PlayerManager.PlayerChallenge.State.Claim:
                {
                    challengeSectionTodo.SetActive(false);
                    challengeSectionCompleted.SetActive(false);
                    challengeSectionClaim.SetActive(true);
                    break;
                }
            }
        }

        public void Claim()
        {
            gameHud.RewardCoins(itemReward);
            playerManager.CompleteChallenge(itemId);
            Switch(PlayerManager.PlayerChallenge.State.Completed);
        }

        public void PlayButtonTap()
        {
            SoundSystem.Instance.PlaySound("tap");
        }
    }
}
