using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Huds;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Game.Common.Scripts.Scenes.Entities
{
    public class Entity : MonoBehaviour
    {
        //======STATIC

        static protected List<Entity> entitiesList = new List<Entity>();

        //======FIELDS

        protected bool entityActive = false;

        //======INJECTS

        [Inject] protected GameHud    sceneHud;
        [Inject] protected GameEngine sceneEngine;

        //======PROPERTIES

        static public  List<Entity> Entities { get { return entitiesList; } }

        //======MONOBEHAVIOUR

        private void Awake()
        {
            entitiesList.Add(this);
        }

        private void OnDestroy()
        {
            entitiesList.Remove(this);
        }

        //======ENTITY

        public void Construct(GameHud hud, GameEngine engine)
        {
            sceneHud     = hud;
            sceneEngine  = engine;
            entityActive = true;
        }

        public void Continue()
        {
            entityActive = true;
        }

        public void Pause()
        {
            entityActive = false;
        }
    }
}

