using UnityEngine;

using Zenject;

using Game.Common.Scripts.Local.Game.Animators;
using Game.Common.Scripts.Local.Game.Cores;
using Game.Common.Scripts.Local.Game.Spawners;
using Game.Common.Scripts.Local.UI.Huds;

namespace Game.Common.Scripts.Local.Initializers
{
    public class RoadwayInitializer : MonoBehaviour
    {
        //================================
        //===INSPECTOR
        //================================

        [Header("Map")]
        [SerializeField] private GameObject mapRoad;
        [SerializeField] private GameObject mapRoadFinish;
        [SerializeField] private SpriteRenderer[] mapRoadSprites;

        //================================
        //===INJECTS
        //================================

        [Inject] private RoadwayCore gameCore;                
        [Inject] private RoadwayHud gameHud;
        [Inject] private RoadwayAnimator gameAnimator;

        //================================
        //===MONOBEHAVIOUR
        //================================

        private void Start()
        {
            //Animator
            gameAnimator.Initialize(mapRoad, mapRoadFinish, mapRoadSprites);

            //Core
            gameCore.Initialize();
            gameCore.Run(1);

            //Hud
            gameHud.Initialize();
        }
    }
}
