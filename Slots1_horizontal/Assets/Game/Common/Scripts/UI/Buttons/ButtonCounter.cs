using UnityEngine;
using UnityEngine.UI;

using TMPro;
using Newtonsoft.Json.Linq;
using UnityEngine.Events;

namespace Game.Common.Scripts.UI.Buttons
{
    [ExecuteInEditMode]
    public class ButtonCounter : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Button      uiButtonPlus;
        [SerializeField] private Button      uiButtonMinus;
        [SerializeField] private TMP_Text    uiCounterText;
        //
        [SerializeField] private int         uiCounterValue;
        [SerializeField] private int         uiCounterIter;
        [SerializeField] private int         uiCounterMin;
        [SerializeField] private int         uiCounterMax;
        //
        [SerializeField] private UnityEvent  OnValueChanged;

        #endregion

        //===========================================

        #region Properties

        public int Value { get { return uiCounterValue; } }

        #endregion

        //===========================================

        #region ButtonCounter

        public void UpdateValue()
        {
            uiCounterText.text = uiCounterValue.ToString();

            //Min
            uiButtonMinus.interactable = (uiCounterValue != uiCounterMin);

            //Max
            uiButtonPlus.interactable  = (uiCounterValue != uiCounterMax);

            //Event
            EventValueChanged();
        }

        public void IncValue()
        {
            uiCounterValue += uiCounterIter;
            if (uiCounterValue >= uiCounterMax)
            {
                uiCounterValue = uiCounterMax;
            }

            UpdateValue();
        }

        public void DecValue()
        {
            uiCounterValue -= uiCounterIter;
            if (uiCounterValue <= uiCounterMin)
            {
                uiCounterValue = uiCounterMin;
            }

            UpdateValue();
        }

        public void SetValue(int value)
        {
            uiCounterValue = value;

            UpdateValue();
        }

        #endregion

        //===========================================

        #region Events

        private void EventValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        #endregion
    }
}