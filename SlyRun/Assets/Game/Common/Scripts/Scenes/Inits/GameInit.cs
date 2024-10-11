using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Huds;

namespace Game.Common.Scripts.Scenes.Inits
{
    public class GameInit : BaseInit
    {
        //======INJECTS
        
        [Inject] private GameHud    sceneHud;
        [Inject] private GameEngine sceneEngine;

        //======MONOBEHAVIOUR

        private void Awake()
        {
            sceneEngine.Launch();
        }
    }
}