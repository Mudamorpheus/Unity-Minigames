using System.Collections.Generic;

using UnityEngine;

namespace Game.Common.Scripts.Local.Game.Entities 
{	
	public class Entity : MonoBehaviour
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] protected SpriteRenderer entitySprite;       
        [SerializeField] protected float entitySpeed = 1f;
        [SerializeField] protected float entityLifespan = 5f;
        [SerializeField] protected bool entityInvincible = false;

        //================================
        //===FIELDS
        //================================

        static private List<Entity> entitiesList = new List<Entity>();

        protected bool entityActive = true;
        protected float entityLifetime = 0f;

        //================================
        //===PROPERTIES
        //================================

        static public List<Entity> Entities { get { return entitiesList; } }

        public bool Invincible { get { return entityInvincible; } }

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Awake()
        {
            entitiesList.Add(this);
        }

        private void OnDestroy()
        {
            entitiesList.Remove(this);
        }

        private void FixedUpdate()
        {
            if(entityActive)
            {
                //Speed
                if (entitySpeed > 0f)
                {
                    Move();
                }

                //Life
                if(entityLifetime > 0)
                {
                    entityLifetime += Time.deltaTime;
                    if (entityLifetime >= entityLifespan)
                    {
                        Destroy(gameObject);
                    }
                }                
            }                       
        }

        //================================
        //===ENTITY
        //================================

        private void Move()
        {
            float x = gameObject.transform.localPosition.x;
            float y = gameObject.transform.localPosition.y - entitySpeed * Time.deltaTime;
            float z = gameObject.transform.localPosition.z;
            var position = new Vector3(x, y, z);
            gameObject.transform.localPosition = position;
        }

        //===========================
        //===GAME
        //===========================

        public void Resume()
        {
            entityActive = true;
        }

        public void Pause()
        {
            entityActive = false;
        }
    }
}
