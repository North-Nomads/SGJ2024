using UnityEngine;
using SGJ.Commands;
using SGJ.Infrastructure;

namespace SGJ.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IGameService
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float dashSpeed;

        private CharacterController _characterController;

        public void MoveSelf()
        {
            Vector3 movementVector = Vector3.ClampMagnitude(new (InputHandler.HorizontalAxisInput,
                0, InputHandler.VerticalAxisInput), 1);
            
            _characterController.SimpleMove(movementVector * moveSpeed);
        }

        private void PerformDashForward()
        {

        }

        public void OnServiceInstantiated()
        {
            _characterController = GetComponent<CharacterController>();
            ServiceLocator.Current.Register(this);
        }
    }
}
