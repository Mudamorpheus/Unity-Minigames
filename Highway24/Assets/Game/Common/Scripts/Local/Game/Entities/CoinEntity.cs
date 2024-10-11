using UnityEngine;

namespace Game.Common.Scripts.Local.Game.Entities 
{
	
	public class CoinEntity : Entity
	{
        //================================
        //===INSPECTOR
        //================================

        [SerializeField] protected float entitySpeedScale = 0.1f;
        [SerializeField] protected int   entityReward = 1;

        //================================
        //===PROPERTIES
        //================================

        public int Reward { get { return entityReward; } }

        //================================
        //===ENTITY
        //================================

        public void SetDifficulty(int difficulty)
        {
            entitySpeed += entitySpeedScale * difficulty;
        }
    }	
}
