using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Zenject;

using Game.Common.Scripts.Scenes.Popups;
using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Systems;
using Game.Common.Scripts.Global.Systems;
using System.Security.Principal;

namespace Game.Common.Scripts.Scenes.Huds
{
    public class BaseHud : MonoBehaviour
    {
        //======INSPECTOR

        [SerializeField] protected Canvas       hudCanvas;
        [SerializeField] protected GameObject[] hudViews;
        [SerializeField] protected Button[]     hudButtons;

        //======INJECT

        [Inject] protected BaseEngine            sceneEngine;
        [Inject] protected AccountSystem         systemAccount;
        [Inject] protected DefeatPopup.Factory   popupDefeatFactory;
        [Inject] protected PausePopup.Factory    popupPauseFactory;
        [Inject] protected TutorialPopup.Factory popupTutorialFactory;

        //======PROPERTIES

        public Canvas        Canvas  { get { return hudCanvas; } }       
        public AccountSystem Account { get { return systemAccount; } }

        //======HUD        

        public virtual void Init()
        {
            if(hudViews.Length > 0)
            {
                SwitchView(hudViews[0]);
            }            
        }

        //===

        public void SwitchButtons(bool state)
        {
            foreach(var button in hudButtons)
            {
                button.enabled = state;
            }
        }
        public void SwitchScene(string scene)
        {
            SceneManager.LoadScene(scene);
        }
        public void SwitchAudio()
        {
            Account.Data.Audio = !Account.Data.Audio;
            bool state = Account.Data.Audio;
            MusicSystem.Instance.SwitchMusic(state);
            SoundSystem.Instance.SwitchSound(state);
            Account.Save();
        }

        //===

        public void ShowView(GameObject view)
        {
            view.SetActive(true);
        }
        public void HideViews()
        {
            foreach (var view in hudViews)
            {
                view.SetActive(false);
            }
        }
        public void SwitchView(GameObject view)
        {
            HideViews();
            ShowView(view);
        }

        //===

        public virtual void ShowPopupDefeat()
        {
            popupDefeatFactory.Create(this, sceneEngine);
        }
        public virtual void ShowPopupPause()
        {
            popupPauseFactory.Create(this, sceneEngine);
        }
        public virtual void ShowPopupTutorial()
        {
            popupTutorialFactory.Create(this);
        }

        //===

        public void SetBestScore(int score)
        {
            Account.Data.Score = score;
            Account.Save();
        }

        //===

        public void ButtonTap()
        {
            SoundSystem.Instance.PlaySound("Tap");
        }
    }
}