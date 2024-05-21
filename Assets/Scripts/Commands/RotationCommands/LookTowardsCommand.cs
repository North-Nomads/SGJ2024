using SGJ.Player;
using UnityEngine;

namespace SGJ.Commands.RotationCommands
{
    public partial class LookTowardsCommand : RotationCommand
    {
        private Vector3 _cursorPostion;

        public override void Execute(PlayerEyes player)
        {
            Physics.Raycast(player.PlayerCamera.ActiveCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit);
            _cursorPostion = hit.point;
            player.RotateTowards(_cursorPostion);
        }
    }
}
