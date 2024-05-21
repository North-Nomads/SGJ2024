using UnityEngine;

namespace SGJ.Player
{
    public class PlayerCamera : MonoBehaviour
    {
        private Camera _currentCamera;
        public Camera ActiveCamera => _currentCamera;
    }
}