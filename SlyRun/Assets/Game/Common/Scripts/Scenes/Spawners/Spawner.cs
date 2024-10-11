using UnityEngine;

using Zenject;

using Game.Common.Scripts.Scenes.Huds;
using Game.Common.Scripts.Scenes.Engines;
using Game.Common.Scripts.Scenes.Entities;

namespace Game.Common.Scripts.Scenes.Spawners
{
    public class Spawner : MonoBehaviour
    {
        //======INSPECTOR

        [SerializeField] private GameObject spawnerPrefab;

        //======INJECTS

        [Inject] private GameHud    sceneHud;
        [Inject] private GameEngine sceneEngine;

        //======SPAWNER

        public void Spawn(int lifespan)
        {
            //Object
            var spawn = Instantiate(spawnerPrefab, gameObject.transform);

            //Entity
            var entity = spawn.GetComponent<Entity>();
            if (entity) { entity.Construct(sceneHud, sceneEngine); }
        }
    }
}
