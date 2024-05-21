using SGJ.Player;

namespace SGJ.Commands
{
    public abstract class MoveCommand
    {
        public abstract void Execute(PlayerMovement player);
    }
}