using UnityEngine;
using UnityEngine.UI;

namespace Game.Common.Scripts.UI.Items
{
    public class SlotItem : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int slotId;
        [SerializeField] private int slotMultiplier = 1;

        #endregion

        //===========================================

        #region Properties

        public int Id { get { return slotId; } }
        public Image WinBorder { get { return gameObject.transform.GetChild(0).GetComponent<Image>();  } }

        #endregion
    }
}

