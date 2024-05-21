using UnityEngine;
using SGJ.Commands;

namespace SGJ.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float dashSpeed;

        private CharacterController _characterController;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void MoveSelf()
        {
            Vector3 movementVector = Vector3.ClampMagnitude(new (InputHandler.HorizontalAxisInput,
                0, InputHandler.VerticalAxisInput), 1);
            
            _characterController.SimpleMove(movementVector * moveSpeed);
        }

        private void PerformDashForward()
        {

        }
    }
}
