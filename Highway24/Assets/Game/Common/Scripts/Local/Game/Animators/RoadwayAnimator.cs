using UnityEngine;

using DG.Tweening;

namespace Game.Common.Scripts.Local.Game.Animators
{	
	public class RoadwayAnimator
	{
        //================================
        //===FIELDS
        //================================

        private GameObject mapRoadMain;
        private GameObject mapRoadFinish;
        private SpriteRenderer[] mapSprites;
        private float mapRoadDuration = 1f;

        //================================
        //===PROPERTIES
        //================================

        public float RoadDuration { get { return mapRoadDuration; } }

        //================================
        //===MAP
        //================================

        public void Initialize(GameObject main, GameObject finish, SpriteRenderer[] sprites)
        {
            mapRoadMain = main;
            mapRoadFinish = finish;
            mapSprites = sprites;
        }

        public void Clear()
        {
            DOTween.Clear();
        }

        public void MoveMap()
        {
            var height = mapSprites[0].bounds.size.y;
            var begin = mapRoadMain.transform.position;
            var end = new Vector3(begin.x, begin.y - height, begin.z);
            mapRoadMain.transform.DOMove(end, mapRoadDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        }
        public void MoveFinish()
        {
            var height = mapSprites[0].bounds.size.y;
            var begin = mapRoadFinish.transform.position;
            var end = new Vector3(begin.x, begin.y - height*2, begin.z);
            mapRoadFinish.transform.DOMove(end, mapRoadDuration).SetEase(Ease.Linear);
        }      
        public void ResetFinish()
        {
            mapRoadFinish.transform.DORestart();
            mapRoadFinish.transform.DOPause();
        }       

        //===========================
        //===GAME
        //===========================

        public void Resume()
        {
            DOTween.PlayAll();

            //TODO - OPTIMIZE LATER
            var animators = GameObject.FindObjectsOfType<Animator>();
            foreach (var animator in animators)
            {
                animator.speed = 1f;
            }
        }
        public void Pause()
        {
            DOTween.PauseAll();

            //TODO - OPTIMIZE LATER
            var animators = GameObject.FindObjectsOfType<Animator>();
            foreach (var animator in animators)
            {
                animator.speed = 0f;
            }
        }
    }
}
