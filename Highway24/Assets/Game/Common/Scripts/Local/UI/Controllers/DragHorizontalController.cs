using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Common.Scripts.Local.UI.Controllers 
{	
	public class DragHorizontalController : MonoBehaviour, IDragHandler
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] private GameObject dragPin;
        [SerializeField] private float dragBorder = 100f;
        [SerializeField] private UnityEvent<float> onDragEvent;

        //================================
        //===FIELDS
        //================================

        private float dragCurrent = 0f;
        private float dragNormalized = 0f;
        private float screenWidth = Screen.width;

        //================================
        //===PROPERTIES
        //================================

        public UnityEvent<float> OnDragEvent { get { return onDragEvent; } }

        //================================
        //===MONOBEHAVIOUR
        //================================

        public void OnDestroy()
        {
            onDragEvent.RemoveAllListeners();
        }

        //================================
        //===IDRAGHANDLER
        //================================

        public void OnDrag(PointerEventData eventData)
        {
            //Screen
            float screenX = eventData.pointerCurrentRaycast.screenPosition.x - screenWidth/2;

            //Borders
            if (screenX > dragBorder)
            {
                screenX = dragBorder;
            }
            else if(screenX < -dragBorder)
            {
                screenX = -dragBorder;
            }

            //Move
            dragCurrent = screenX;    
            dragNormalized = screenX/dragBorder;
            dragPin.transform.localPosition = new Vector3(dragCurrent, dragPin.transform.localPosition.y, dragPin.transform.localPosition.z);

            //Event            
            onDragEvent?.Invoke(dragNormalized);
        }
    }
}
