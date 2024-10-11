using UnityEngine;
using UnityEngine.UI;

using Game.Common.Scripts.DI.Cores.Engines;
using Game.Common.Scripts.DI.Cores.Huds;
using Game.Common.Scripts.Systems;
using DG.Tweening;

namespace Game.Common.Scripts.UI.Popups
{
    public class BasePopup : MonoBehaviour
    {
        #region Fields

        [SerializeField] protected Button[] uiButtons;

        protected BaseHud    sceneHud;
        protected BaseEngine sceneEngine;

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
            foreach (var button in uiButtons)
            {
                button.onClick.RemoveAllListeners();
            }
        }

        #endregion

        //===========================================

        #region BasePopup

        public virtual void Init(BaseHud hud, BaseEngine engine)
        {
            //DI
            sceneHud    = hud;
            sceneEngine = engine;

            //Taps
            foreach (var button in uiButtons)
            {
                button.onClick.AddListener(Tap);
            }
        }

        public virtual void Show()
        {
            if(sceneHud as MenuHud) { ((MenuHud)sceneHud).SwitchButtons(false);  }
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            if (sceneHud as MenuHud) { ((MenuHud)sceneHud).SwitchButtons(true); }
            Destroy(gameObject);
        }

        public void Tap()
        {
            SoundSystem.Instance.PlaySound("tap");
        }

        #endregion
    }
}