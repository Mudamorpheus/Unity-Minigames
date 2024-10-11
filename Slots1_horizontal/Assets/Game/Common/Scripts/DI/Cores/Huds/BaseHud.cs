using Game.Common.Scripts.Data;
using UnityEngine;

using Zenject;
using DG.Tweening;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.UI.Popups;
using Game.Common.Scripts.Systems;

namespace Game.Common.Scripts.DI.Cores.Huds
{
    public class BaseHud : MonoBehaviour
    {
        #region Fields

        [Inject] protected BaseEngine            sceneEngine;
        [Inject] protected Canvas                sceneCanvas;
        [Inject] protected PlayerData            playerData;
        [Inject] protected SettingsPopup.Factory uiSettingsPopupFactory;
        [Inject] protected BankruptPopup.Factory uiBankruptPopupFactory;                
        [Inject] protected InfoPopup.Factory     uiInfoPopupFactory;

        #endregion

        //===========================================

        #region Properties

        public Canvas     Canvas     { get { return sceneCanvas; } }
        public PlayerData PlayerData { get { return playerData; } }

        #endregion

        //===========================================

        #region MonoBehaviour

        private void OnDestroy()
        {
            Clear();
        }

        private void OnApplicationQuit()
        {
            Clear();
        }

        protected virtual void Clear()
        {
            DOTween.Clear();
        }

        #endregion

        //===========================================

        #region BaseHud

        public virtual void Init()
        {
            UpdateValues();
        }

        public virtual void UpdateValues()
        {

        }

        public void SwitchMusic()
        {
            playerData.Music = !playerData.Music;
            MusicSystem.Instance.SwitchMusic(playerData.Music);
        }

        public void SwitchSound()
        {
            playerData.Sound = !playerData.Sound;
            SoundSystem.Instance.SwitchSound(playerData.Sound);
        }

        #endregion

        //===========================================

        #region Actions

        public virtual void BuyBalance(int balance)
        {
            playerData.Balance += balance;            
        }

        #endregion

        //===========================================

        #region BaseHud

        public void ShowSettingsPopup()
        {
            uiSettingsPopupFactory.Create(this, sceneEngine);
        }

        public void ShowBankruptPopup()
        {
            uiBankruptPopupFactory.Create(this, sceneEngine);
        }

        public void ShowInfoPopup()
        {
            uiInfoPopupFactory.Create(this, sceneEngine);
        }

        #endregion
    }
}