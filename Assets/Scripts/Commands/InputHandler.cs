using SGJ.Commands.MoveCommands;
using SGJ.Infrastructure;
using SGJ.Player;
using System;
using UnityEngine;

namespace SGJ.Commands
{
    public class InputHandler : MonoBehaviour
    {
        private static float _horizontalAxisInput;
        private static float _verticalAxisInput;

        // Player components
        private PlayerMovement _playerMovement;
        private PlayerEyes _playerEyes;
        private PlayerWeaponry _playerWeaponry;
        private PlayerCombat _playerCombat;
        private PlayerInteraction _playerInteraction;

        public static float HorizontalAxisInput => _horizontalAxisInput;
        public static float VerticalAxisInput => _verticalAxisInput;

        public static InputHandler Instance { get; private set; }
        public bool IsWASDPressed => Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.S);

        private void Start()
        {
            Instance = this;
            _playerMovement = ServiceLocator.Current.Get<PlayerMovement>();
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
            return null;
        }

        private CombatCommand HandleCombatInput()
        {
            return null;
        }

        private WeaponryCommand HandleWeaponryInput()
        {
            return null;
        }

        private RotationCommand HandleRotationInput()
        {
            return null;
        }

        private MoveCommand HandleMoveInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
                return new DashCommand();
            else if (IsWASDPressed)
                return new RunCommand();
            return null;
        }
    }
}
