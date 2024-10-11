using UnityEngine;
using UnityEngine.UI;

using TMPro;

using Game.Common.Scripts.Misc.UI;

namespace Game.Common.Scripts.Scenes.Huds
{
    public class MenuHud : BaseHud
    {
        //======INSPECTOR

        [SerializeField] private TMP_Text[]       uiScoreIndicators;
        [SerializeField] private ButtonSwitcher[] uiAudioIndicators;

        //======HUD        

        public override void Init()
        {
            base.Init();

            UpdateScoreIndicators();
            UpdateAudioIndicators();
        }

        //======MENU

        public void UpdateScoreIndicators()
        {
            foreach (var indicator in uiScoreIndicators)
            {
                indicator.text = "Score: " + Account.Data.Score.ToString();
            }
        }

        public void UpdateAudioIndicators()
        {
            foreach(var indicator in uiAudioIndicators)
            {
                indicator.Switch(Account.Data.Audio);
            }
        }

        public void SetAccountScore(int score)
        {
            Account.Data.Score = score;
            UpdateScoreIndicators();
            Account.Save();
        }

        public void SetAccountAudio(bool state)
        {
            Account.Data.Audio = state;
            UpdateAudioIndicators();
            Account.Save();
        }
    }
}