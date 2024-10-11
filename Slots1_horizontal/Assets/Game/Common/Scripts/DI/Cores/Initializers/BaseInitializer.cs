using UnityEngine;

using Zenject;

using Game.Common.Scripts.DI.Cores.Huds;
using Game.Common.Scripts.DI.Cores.Engines;

namespace Game.Common.Scripts.DI.Cores.Initializers
{
    public class BaseInitializer : MonoBehaviour
    {
        #region Vars

        [Inject] protected BaseHud sceneHud;
        [Inject] protected BaseEngine sceneEngine;

        #endregion

        #region MonoBehaviour

        protected virtual void Start()
        {
            sceneHud.Init();
            sceneEngine.Init();
        }

        protected virtual void Update()
        {

        }

        #endregion
    }
}