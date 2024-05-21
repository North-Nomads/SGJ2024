using SGJ.Player;
using System;
using UnityEngine;

namespace SGJ.Commands
{
    public class InputHandler : MonoBehaviour
    {
        private float _horizontalAxisInput;
        private float _verticalAxisInput;

        // Player components
        private PlayerMovement _playerMovement;
        private PlayerEyes _playerEyes;
        private PlayerWeaponry _playerWeaponry;
        private PlayerCombat _playerCombat;
        private PlayerInteraction _playerInteraction;

        public float HorizontalAxisInput => _horizontalAxisInput;
        public float VerticalAxisInput => _verticalAxisInput;

        public static InputHandler Instance { get; private set; }

        private void Start()
        {
            Instance = this;

            // Get _player$$$ components using ServiceLocator
        }

        private void Update()
        {
            _horizontalAxisInput = Input.GetAxis("Horizontal");
            _verticalAxisInput = Input.GetAxis("Vertical");

            HandleCommands();
        }

        private void HandleCommands()
        {
            var moveCommand = HandleMoveInput();
            moveCommand?.Execute(_playerMovement);

            var rotationCommand = HandleRotationInput();
            rotationCommand?.Execute(_playerEyes);
            
            var weaponryCommand = HandleWeaponryInput();
            weaponryCommand?.Execute(_playerWeaponry);

            var combatCommand = HandleCombatInput();
            combatCommand?.Execute(_playerCombat);

            var interactionCommand = HandleInteractionInput();
            interactionCommand?.Execute(_playerInteraction);
        }

        private InteractionCommand HandleInteractionInput()
        {
            throw new NotImplementedException();
        }

        private CombatCommand HandleCombatInput()
        {
            throw new NotImplementedException();
        }

        private WeaponryInput HandleWeaponryInput()
        {
            throw new NotImplementedException();
        }

        private RotationCommand HandleRotationInput()
        {
            throw new NotImplementedException();
        }

        private MoveCommand HandleMoveInput()
        {
            throw new NotImplementedException();
        }
    }
}
