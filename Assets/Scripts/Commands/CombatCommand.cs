using SGJ.Player;

namespace SGJ.Commands
{
    public abstract class RotationCommand
    {
        public abstract void Execute(PlayerCombat player);
    }
}