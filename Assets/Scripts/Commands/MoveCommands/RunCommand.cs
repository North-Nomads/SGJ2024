using SGJ.Player;

namespace SGJ.Commands.MoveCommands
{
    public class RunCommand : MoveCommand
    {
        public override void Execute(PlayerMovement player)
        {
            player.MoveSelf();
        }
    }
}
