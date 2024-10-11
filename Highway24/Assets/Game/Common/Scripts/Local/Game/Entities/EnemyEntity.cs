using System.Collections.Generic;
using UnityEngine;

namespace Game.Common.Scripts.Local.Game.Entities 
{	
	public class EnemyEntity : Entity
    {
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] protected float entitySpeedScale = 0.1f;

        //================================
        //===ENTITY
        //================================        

        public void SetDifficulty(int difficulty)
        {
            entitySpeed += entitySpeedScale * difficulty;
        }
    }	
}
