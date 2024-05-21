using UnityEngine;

namespace SGJ.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerEyes : MonoBehaviour
    {
        private Vector3 _aimDirection;
        private PlayerCamera _playerCamera;

        public PlayerCamera PlayerCamera => _playerCamera;

        private void Start()
        {
            _playerCamera = GetComponent<PlayerCamera>();
        }

        public void RotateTowards(Vector3 lookTarget)
        {
            Vector3 lookPosition = new(lookTarget.x, transform.position.y, lookTarget.z);
            transform.LookAt(lookPosition);
            _aimDirection = (lookPosition - transform.position).normalized;
        }
    }
}