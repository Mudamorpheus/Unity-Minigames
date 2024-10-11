using Game.Common.Scripts.Global.Systems;
using Game.Common.Scripts.Local.UI.Huds;
using UnityEngine;

using UnityEngine.SceneManagement;

namespace Game.Common.Scripts.Local.UI.Popups 
{	
	public class Popup : MonoBehaviour
	{
        //================================
        //===UI
        //================================

        public virtual void Load(string scene)
        {
            SceneManager.LoadScene(scene);
        }

        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            Destroy(gameObject);
        }

        public void PlayButtonTap()
        {
            SoundSystem.Instance.PlaySound("tap");
        }
    }
}
