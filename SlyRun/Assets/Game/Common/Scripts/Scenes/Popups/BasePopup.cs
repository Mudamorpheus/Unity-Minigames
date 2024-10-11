using UnityEngine;
using UnityEngine.SceneManagement;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Global.Systems;

namespace Game.Common.Scripts.Scenes.Popups
{
    public class BasePopup : MonoBehaviour
    {
        //======FIELDS

        protected BaseHud    sceneHud;
        protected BaseEngine sceneEngine;

        //======POPUP

        public virtual void Open()
        {
            gameObject.SetActive(true);
            sceneHud.SwitchButtons(false);
        }

        public virtual void Close()
        {
            Destroy(gameObject);
            sceneHud.SwitchButtons(true);
        }

        //======HUD

        public void SwitchScene(string scene)
        {
            sceneHud.SwitchScene(scene);
        }

        public void ButtonTap()
        {
            SoundSystem.Instance.PlaySound("Tap");
        }
    }
}
