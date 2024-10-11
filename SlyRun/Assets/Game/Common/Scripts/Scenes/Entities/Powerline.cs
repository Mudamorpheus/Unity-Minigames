using Game.Common.Scripts.Scenes.Engines;
using UnityEngine;
using Zenject;

namespace Game.Common.Scripts.Scenes.Entities
{
    public class Powerline : Entity
    {
        //======INSPECTOR

        [SerializeField] private float powerlineHaste = 1f;                
        [SerializeField] private float powerlineDecay = 0f;

        //======POWERLINE

        public void Activate()
        {
            //Haste
            sceneEngine.SetGameHaste(powerlineHaste);

            //Decay
            sceneEngine.SetGameDecay(powerlineDecay);
        }

        public void Deactivate()
        {
            //Haste
            sceneEngine.SetGameHaste(1f);

            //Decay
            sceneEngine.SetGameDecay(0f);
        }
    }
}

