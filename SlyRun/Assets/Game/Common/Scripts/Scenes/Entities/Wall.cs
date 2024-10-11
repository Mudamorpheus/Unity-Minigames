using Game.Common.Scripts.Scenes.Engines;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Game.Common.Scripts.Scenes.Entities
{
    public class Wall : Entity
    {        
        //======INSPECTOR

        [SerializeField] private float wallSpeed  = 1f;
        [SerializeField] private float wallDamage = 0.335f;

        //======PROPERTIES

        public float Damage { get { return wallDamage; } }

        //======MONOBEHAVIOUR

        private void Update()
        {
            if(entityActive)
            {
                float move = -wallSpeed * Time.deltaTime;
                move *= sceneEngine.Speed * sceneEngine.Haste;;

                float x = gameObject.transform.position.x;
                float y = gameObject.transform.position.y + move;
                float z = gameObject.transform.position.z;

                var position = new Vector3(x, y, z);
                gameObject.transform.position = position;
            }
        }
    }
}

