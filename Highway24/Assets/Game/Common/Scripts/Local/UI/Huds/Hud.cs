using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using TMPro;
using Zenject;
using DG.Tweening;

using Game.Common.Scripts.Global.Managers;
using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Players;
using Game.Common.Scripts.Local.Game.Spawners;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Local.UI.Huds 
{	
	public abstract class Hud : MonoBehaviour
    {
        //================================
        //===INSPECTOR
        //================================

        [Header("Hud UI")]
        [SerializeField] protected Canvas uiMainCanvas;
        [SerializeField] protected GameObject[] uiViews;
        [SerializeField] protected Button[] uiButtons;
        [SerializeField] protected TMP_Text[] uiCoinsTexts;
        [SerializeField] protected TMP_Text[] uiScoreTexts;        

        //================================
        //===FIELDS
        //================================

        protected GameObject uiPrevView;
        protected GameObject uiCurrentView;

        //================================
        //===INJECTS
        //================================

        [Inject] protected PlayerManager playerManager;

        //================================
        //===PROPERTIES
        //================================

        public Canvas Canvas { get { return uiMainCanvas; } }

        //================================
        //===MONOBEHAVIOUR
        //================================ 

        private void OnDestroy()
        {
            //playerManager.SaveProfile();
            DOTween.Clear();
        }

        private void OnApplicationQuit()
        {
            playerManager.SaveProfile();
        }

        //================================
        //===UI
        //================================ 

        public virtual void Initialize()
        {
            //Views
            if (uiViews.Length > 0)
            {
                SwitchView(uiViews[0]);
            }

            //Values
            UpdateCoinTexts();
            UpdateScoreTexts();            
        }

        //================================
        //===VIEWS
        //================================ 

        protected void HideViews()
        {
            foreach (var view in uiViews)
            {
                HideView(view);
            }
        }
        protected void HideView(GameObject view)
        {
            view.SetActive(false);
        }
        protected void ShowView(GameObject view)
        {
            uiPrevView = uiCurrentView;
            uiCurrentView = view;

            view.SetActive(true);
        }

        public void SwitchView(GameObject view)
        {
            HideViews();
            ShowView(view);
        }
        public void SwitchBack()
        {
            HideViews();
            ShowView(uiPrevView);
        }

        public void SwitchScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        public void SwitchButtons(bool state)
        {
            foreach (var button in uiButtons)
            {
                button.enabled = state;
            }
        }

        //================================
        //===AUDIO
        //================================ 

        public void SwitchMusic(bool state)
        {
            MusicSystem.Instance.SwitchMusic(state);
        }

        public void SwitchSound(bool state)
        {
            SoundSystem.Instance.SwitchSound(state);
        }

        public void PlayMusic(string name)
        {
            MusicSystem.Instance.PlayMusic(name);
        }
        public void PlaySound(string name)
        {
            SoundSystem.Instance.PlaySound(name);
        }

        public void PlayButtonTap()
        {
            PlaySound("tap");
        }
        public void PlayCoinTap()
        {
            PlaySound("coin");
        }
        public void PlayGameover()
        {
            PlaySound("game over");
        }

        //================================
        //===VALUES
        //================================ 

        public void UpdateCoinTexts()
        {
            foreach (var text in uiCoinsTexts)
            {
                text.text = playerManager.Data.player_coins.ToString();
            }
        }
        public void UpdateScoreTexts()
        {
            foreach (var text in uiScoreTexts)
            {
                text.text = playerManager.Data.player_best_score.ToString();
            }
        }

        public virtual void RewardCoins(int coins)
        {
            playerManager.AddCoins(coins);
            UpdateCoinTexts();
        }
        public virtual void SetBestScore(int newScore)
        {
            int oldScore = playerManager.Data.player_incidents;
            if(newScore > oldScore)
            {
                playerManager.SetBestScore(newScore);
            }            
            UpdateScoreTexts();
        }
    }
}
