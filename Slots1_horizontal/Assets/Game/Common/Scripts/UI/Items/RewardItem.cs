using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Game.Common.Scripts.UI.Frames;

using static Game.Common.Scripts.Data.PlayerData.Fields;
using Game.Common.Scripts.Systems;

namespace Game.Common.Scripts.UI.Items
{
    public class RewardItem : MonoBehaviour
    {
        #region Fields

        [SerializeField] private TMP_Text    uiLevelText;
        [SerializeField] private TMP_Text    uiRewardText;
        [SerializeField] private Button      uiClaimButton;
        [SerializeField] private CanvasGroup uiCanvasGroup;

        private RewardFrame uiRewardFrame;
        private int itemLevel;
        private int itemReward;
        private LevelState itemState;

        #endregion

        //===========================================

        #region Properties

        public int        Level { get { return itemLevel; } }
        public LevelState State { get { return itemState; } }

        #endregion

        //===========================================

        #region RewardItem

        public void Init(RewardFrame frame, int level, int reward, LevelState state)
        {
            uiRewardFrame = frame;
            itemLevel = level;
            itemReward = reward;

            uiLevelText.text = "Level " + (level+1).ToString();
            uiRewardText.text = reward.ToString();

            SwitchState(state);
        }

        public void SwitchState(LevelState state)
        {
            itemState = state;

            switch (state)
            {
                case LevelState.Default:
                    {
                        uiClaimButton.gameObject.SetActive(false);
                        uiClaimButton.interactable = false;
                        uiCanvasGroup.alpha = 1f;
                        break;
                    }
                case LevelState.Completed:
                    {
                        uiClaimButton.gameObject.SetActive(true);
                        uiClaimButton.interactable = true;
                        uiCanvasGroup.alpha = 1f;
                        break;
                    }
                case LevelState.Claimed:
                    {
                        uiClaimButton.gameObject.SetActive(false);
                        uiClaimButton.interactable = false;
                        uiCanvasGroup.alpha = 0.5f;
                        break;
                    }
            }
        }

        public void Claim()
        {            
            SwitchState(LevelState.Claimed);
            uiRewardFrame.Claim(itemLevel, itemReward);

            SoundSystem.Instance.PlaySound("claim reward");
        }

        #endregion
    }
}