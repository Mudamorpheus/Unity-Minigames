using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;

using Zenject;

namespace Game.Common.Scripts.Misc.Adaptive
{
    public class ElementsFitter : MonoBehaviour
    {
        #region Vars

        [SerializeField] private RectTransform elementTransform;
        [SerializeField] private RectTransform parentTransform;
        [SerializeField] private float         parentTransformHeight;

        //
        [Inject] private CanvasScaler sceneScaler;

        #endregion

        //===========================================

        #region Monobehaviour

        private void Awake()
        {
            Invoke("Fit", 0.01f);
        }

        private void Fit()
        {
            float ratioY = parentTransform.rect.height / parentTransformHeight;
            float offsetMinY = elementTransform.offsetMin.y * ratioY;
            elementTransform.offsetMin = new Vector2(elementTransform.offsetMin.x, offsetMinY);
        }

        #endregion
    }
}