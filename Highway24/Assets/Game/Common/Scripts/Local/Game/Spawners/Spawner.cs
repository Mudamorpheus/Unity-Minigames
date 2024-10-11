using Game.Common.Scripts.Local.Game.Entities;
using System.Collections.Generic;

using UnityEngine;

namespace Game.Common.Scripts.Local.Game.Spawners 
{
	public class Spawner : MonoBehaviour
	{
        //===========================
        //===INSPECTOR
        //===========================

        [SerializeField] private GameObject spawnerPrefab;
        [SerializeField] private RectTransform spawnerArea;
        [SerializeField] private float spawnerPeriod = 2f;

        //===========================
        //===FIELDS
        //===========================

        private static List<Spawner> spawnersList = new List<Spawner>();
        private bool spawnerActive = false;
        private float spawnerTime = 0f;

        //===========================
        //===PROPERTIES
        //===========================

        static public List<Spawner> Spawners { get { return spawnersList; } }

        //===========================
        //===MONOBEHAVIOUR
        //===========================

        private void Awake()
        {
            spawnersList.Add(this);
        }

        private void OnDestroy()
        {
            spawnersList.Remove(this);
        }

        private void Update()
        {
            if(spawnerActive)
            {
                spawnerTime += Time.deltaTime;
                if (spawnerTime >= spawnerPeriod)
                {
                    spawnerTime = 0f;
                    Spawn();
                }
            }            
        }

        //===========================
        //===SPAWNER
        //===========================

        public virtual GameObject Spawn()
        {
            //Position
            float x = spawnerArea.transform.position.x + Random.Range(spawnerArea.rect.xMin, spawnerArea.rect.xMax);
            float y = spawnerArea.transform.position.y + Random.Range(spawnerArea.rect.yMin, spawnerArea.rect.yMax);
            float z = spawnerArea.transform.position.z;
            var position = new Vector3(x, y, z);

            //Object
            var spawn = Instantiate(spawnerPrefab, gameObject.transform);
            spawn.transform.position = position;

            return spawn;
        }

        //===========================
        //===GAME
        //===========================

        public void Run()
        {
            spawnerActive = true;
            spawnerTime = 0;
        }

        public void Stop()
        {
            spawnerActive = false;
            spawnerTime = 0;
        }

        public virtual void Resume()
        {
            spawnerActive = true;
        }

        public virtual void Pause()
        {
            spawnerActive = false;
        }
    }
}
