using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Scripts.Local.UI.Misc 
{
    [ExecuteInEditMode]
    public class ButtonSwitcher : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] Button uiButton;
		[SerializeField] Image uiImage;
        [SerializeField] Sprite uiSpriteOn;
        [SerializeField] Sprite uiSpriteOff;
        [SerializeField] bool uiSwitchState;

        //================================
        //===PROPERTIES
        //================================

        public bool State { get { return uiSwitchState; } }

        //================================
        //===UNITY
        //================================

        private void Awake()
        {
			Switch(uiSwitchState);
        }

        private void OnValidate()
        {
            Switch(uiSwitchState);
        }

        //================================
        //===UI
        //================================

        public void Switch(bool state)
		{
            uiSwitchState = state;
            uiButton.interactable = state;

            if (state)
			{
				uiImage.sprite = uiSpriteOn;
			}
			else
			{
                uiImage.sprite = uiSpriteOff;                
            }
		}
    }	
}
