using UnityEngine;

namespace SGJ.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Transform _followObject;
        private Camera _currentCamera;
        public Camera ActiveCamera => _currentCamera;

        private void Start()
        {
            _currentCamera = Camera.main;
        }

        public void SetFollowObject(Transform followObject) => _followObject = followObject;

        private void ShakeCamera(float time)
        {
            // TODO: implement
        }
    }
}