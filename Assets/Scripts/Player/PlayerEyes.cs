using SGJ.Infrastructure;
using UnityEngine;

namespace SGJ.Player
{
    [RequireComponent(typeof(PlayerCamera))]
    public class PlayerEyes : MonoBehaviour, IGameService
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

        public void OnServiceInstantiated()
        {
            ServiceLocator.Current.Register(this);
        }
    }
}