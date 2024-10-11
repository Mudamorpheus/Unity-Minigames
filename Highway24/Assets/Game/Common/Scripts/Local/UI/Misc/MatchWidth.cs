using UnityEngine;

namespace Game.Common.Scripts.Local.UI.Misc
{
    public class MatchWidth : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private float sceneWidth = 10;

        private void Start()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            float unitsPerPixel = sceneWidth / Screen.width;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;
            _camera.orthographicSize = desiredHalfHeight;
        }
    }
}

