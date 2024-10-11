using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Entities;

namespace Game.Common.Scripts.Local.Game.Spawners 
{	
	public class EnemySpawner : Spawner
	{

        //===========================
        //===INJECTS
        //===========================

        [Inject] private RoadwayCore gameCore;

        //===========================
        //===SPAWNER
        //===========================

        public override GameObject Spawn()
        {
            var spawn = base.Spawn();
            var entity = spawn.GetComponent<EnemyEntity>();
            if(entity != null)
            {
                int difficulty = gameCore.Difficulty;
                entity.SetDifficulty(difficulty);
            }            

            return spawn;
        }
    }	
}
