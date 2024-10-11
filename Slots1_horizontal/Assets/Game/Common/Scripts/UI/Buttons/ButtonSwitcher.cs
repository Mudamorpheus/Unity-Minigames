using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Scripts.UI.Buttons
{
    [ExecuteInEditMode]
    public class ButtonSwitcher : MonoBehaviour
	{
        #region Fields

        [SerializeField] private Button uiButton;
		[SerializeField] private Image  uiImage;
        [SerializeField] private Sprite uiSpriteOn;
        [SerializeField] private Sprite uiSpriteOff;
        [SerializeField] private bool   uiSwitchState;

        #endregion

        //===========================================

        #region Properties

        public bool State { get { return uiSwitchState; } }

        #endregion

        //===========================================

        #region MonoBehaviour

        private void Awake()
        {
			Switch(uiSwitchState);
        }

        private void OnValidate()
        {
            Switch(uiSwitchState);
        }

        #endregion

        //===========================================

        #region ButtonSwitcher

        public void Switch(bool state)
		{
            uiSwitchState = state;
            //uiButton.interactable = state;

            if (state)
			{
				uiImage.sprite = uiSpriteOn;
			}
			else
			{
                uiImage.sprite = uiSpriteOff;                
            }
		}

        #endregion
    }
}
