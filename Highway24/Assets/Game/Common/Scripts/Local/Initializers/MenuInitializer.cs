using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.UI.Huds;

namespace Game.Common.Scripts.Local.Initializers 
{
	
	public class MenuInitializer : MonoBehaviour
	{
        //================================
        //===INJECTS
        //================================

        [Inject] private MenuHud uiHud;

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Start()
        {
            uiHud.Initialize();
        }
    }	
}
