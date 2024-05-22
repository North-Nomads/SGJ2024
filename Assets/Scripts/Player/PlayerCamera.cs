using Cinemachine;
using SGJ.Infrastructure;
using UnityEngine;

namespace SGJ.Player
{
    public class PlayerCamera : MonoBehaviour, IGameService
    {
        private const string PlayerCameraPath = "Prefabs/Player/Virtual Camera";

        private Transform _followObject;
        private Camera _currentCamera;
        private CinemachineVirtualCamera _virutalCamera;
        
        public Camera ActiveCamera => _currentCamera;

        private void Start()
        {
            _currentCamera = Camera.main;
            ServiceLocator.Current.Register(this);
        }

        private void InstantiateCamera()
        {
            Debug.Log("Instntiated camera");
            var camera = Resources.Load<CinemachineVirtualCamera>(PlayerCameraPath);
            _virutalCamera = Instantiate(camera);
        }

        public void SetFollowObject(Transform followObject)
        {
            InstantiateCamera();
            _followObject = followObject;
            _virutalCamera.Follow = _followObject;
        }

        private void ShakeCamera(float time)
        {
            // TODO: implement
        }
    }
}