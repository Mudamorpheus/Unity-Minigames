using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;


namespace Game.Common.Scripts.Scenes.Controllers
{
	public class SwipeController : MonoBehaviour
	{
        //======ENUMS

        public enum Direction
		{
			Left = 0,
			Right = 1,
			Up = 2,
			Down = 3,
			Any = 4
		}

		public enum Type
        {
			Default = 0,
			End = 1,
			Cancel = 2
        }

        //======INSPECTOR

        [SerializeField] private Camera    swipeCamera;
        [SerializeField] private Direction swipeDirection;
        [SerializeField] private float     swipeThreeshold;

        //======FIELDS

        private Vector2 swipeBeginPos;
        private Vector2 swipeEndPos;
        private bool    swipeState = false;
        private float   swipeDuration = 0f;
        private float   swipeSpeed = 0f;
        private float   swipeLength = 0f;

        //======MONOBEHAVIOUR

        //Detection
        private void Update()
		{			
            if (Input.touchCount > 0 && Input.touchCount <= 1)
			{				
				foreach (Touch touch in Input.touches)
				{
					//Timer
					if(swipeState)
                    {
						swipeDuration += Time.deltaTime;
                    }

					//Swipe begin
					if (touch.phase == TouchPhase.Began)
					{									
						//Logic
						swipeDuration = 0f;
						swipeState = true;

						//Coordinates
						swipeBeginPos = GetTouchPosition(touch);
                        swipeEndPos = GetTouchPosition(touch);

						//Length
						swipeLength = 0f;
                    }

                    //Swipe detect
                    if (touch.phase == TouchPhase.Moved)
					{
						//Pos
						var position = GetTouchPosition(touch);

                        //Length
                        swipeLength += Vector2.Distance(swipeEndPos, position);

                        //Coordinates
                        swipeEndPos = position;

                        //Detect						
                        DetectionAttempt(Type.Default);
                    }

                    //Swipe end
					/*
                    if (touch.phase == TouchPhase.Ended)
					{
						//Logic
						swipeState = false;

						//Coordinates
						swipeEndPos = GetTouchPosition(touch);

                        //Detect
                        DetectionAttempt(Type.End);

                        //Reset
                        swipeBeginPos = Vector2.zero;
                        swipeEndPos = Vector2.zero;
                        swipeLength = 0f;
                    }

                    //Swipe break
                    if (touch.phase == TouchPhase.Canceled)
					{
						//Logic
						swipeState = false;

						//Coordinates
						swipeEndPos = GetTouchPosition(touch);

                        //Detect
                        DetectionAttempt(Type.Cancel);

						//Reset
                        swipeBeginPos = Vector2.zero;
                        swipeEndPos = Vector2.zero;
                        swipeLength = 0f;
                    }
					*/
                }
			}
		}

        //======SWIPE

        public Vector2 GetTouchPosition(Touch touch)
        {
            var position = new Vector3(touch.position.x, touch.position.y, Mathf.Abs(swipeCamera.transform.position.z));
            return swipeCamera.ScreenToWorldPoint(position);
        }

        public float GetHorizontalLength()
        {
            return Mathf.Abs(swipeEndPos.x - swipeBeginPos.x);
        }
        public float GetVerticalLength()
        {
            return Mathf.Abs(swipeEndPos.y - swipeBeginPos.y);
        }
        public float GetSwipeLength()
        {
            return swipeLength;
        }

        private void DetectionAttempt(Type type)
		{
			if(swipeBeginPos != Vector2.zero && swipeEndPos != Vector2.zero)
            {
				if (GetSwipeLength() > swipeThreeshold && GetVerticalLength() > GetHorizontalLength())
				{
					/*
					if (swipeEndPos.y - swipeBeginPos.y > 0 && (swipeDirection == Direction.Up || swipeDirection == Direction.Any))
					{
                        DetectionCheck(type);
					}
					else if (swipeEndPos.y - swipeBeginPos.y < 0 && swipeDirection == Direction.Down || swipeDirection == Direction.Any)
					{
                        DetectionCheck(type);
					}
					*/
                }
				else if (GetSwipeLength() > swipeThreeshold && GetHorizontalLength() > GetVerticalLength())
				{
					if (swipeEndPos.x - swipeBeginPos.x > 0 && swipeDirection == Direction.Right || swipeDirection == Direction.Any)
					{
                        DetectionCheck(type);
					}
					else if (swipeEndPos.x - swipeBeginPos.x < 0 && swipeDirection == Direction.Left || swipeDirection == Direction.Any)
					{
                        DetectionCheck(type);
					}
                }
			}
		}

        //======EVENTS

        //Swipe detected
        public void DetectionCheck(Type type)
        {
			//Speed
			if(swipeDuration != 0)
            {
				swipeSpeed = swipeThreeshold / swipeDuration;
			}

            //Event
            switch (type)
            {
				case Type.Default:
                {
					OnSwipe();		
					break;
                }
				case Type.End:
				{
                    OnSwipeEnd();							
					break;
				}
				case Type.Cancel:
				{
					OnSwipeCancel();				
					break;
				}
			}
        }

		//Swipe event
		[SerializeField]
		private UnityEvent<Object> swipeEvent;
		private void OnSwipe()
		{
			swipeEvent?.Invoke(this);
		}

		//Swipe release event
		[SerializeField]
		private UnityEvent<Object> swipeEndEvent;
		private void OnSwipeEnd()
		{
            swipeEndEvent?.Invoke(this);
		}

		//Swipe break event
		[SerializeField]
		private UnityEvent<Object> swipeCancelEvent;
		private void OnSwipeCancel()
		{
            swipeCancelEvent?.Invoke(this);
		}
	}
}
