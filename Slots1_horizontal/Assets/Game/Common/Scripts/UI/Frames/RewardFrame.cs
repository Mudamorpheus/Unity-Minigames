using System.Collections.Generic;

using UnityEngine;

using Zenject;

using Game.Common.Scripts.DI.Cores.Huds;
using Game.Common.Scripts.UI.Items;

using static Game.Common.Scripts.Data.PlayerData.Fields;

namespace Game.Common.Scripts.UI.Frames
{
    public class RewardFrame : MonoBehaviour
    {
        #region Fields

        [SerializeField] private List<RewardItem> uiRewardItems;
        [Inject] private MenuHud sceneHud;

        #endregion

        //===========================================

        #region RewardFrame

        public void Start()
        {
            Setup();
        }

        public void Setup()
        {
            int tier = sceneHud.PlayerData.Tier;
            int count = uiRewardItems.Count;

            for (int i = 0; i < count; i++)
            {
                var item = uiRewardItems[i];
                int level = (tier * 5) + i;
                int reward = (level+1) * StaticData.LevelReward;

                LevelState state = LevelState.Default;
                LevelData data = sceneHud.PlayerData.LevelsData.Find(x => x.Level == level);
                if (data != null)
                {
                    state = data.State;
                }

                item.Init(this, level, reward, state);
            }
        }

        public void Complete(int level)
        {
            //Player
            sceneHud.PlayerData.Complete(level);

            var item = uiRewardItems.Find(x => x.Level == level);
            if (item != null) { item.SwitchState(LevelState.Completed); }
        }

        public void Claim(int level, int reward)
        {
            //Player
            sceneHud.PlayerData.AddBalance(reward);
            sceneHud.PlayerData.Claim(level);
            sceneHud.UpdateRewardAttention();
            sceneHud.UpdateBalanceValues();

            //Improve tier
            int count = StaticData.LevelSector;
            bool improve = true;
            for (int i = 0; i < count; i++)
            {
                var item = uiRewardItems[i];
                if(item.State != LevelState.Claimed)
                {
                    improve = false;
                }
            }

            //Upgrade
            if(improve)
            {                
                sceneHud.PlayerData.Tier++;
                Setup();
            }
        }

        #endregion
    }
}

