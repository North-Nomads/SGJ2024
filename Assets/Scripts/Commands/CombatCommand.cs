using SGJ.Player;

namespace SGJ.Commands
{
    public abstract class CombatCommand
    {
        public abstract void Execute(PlayerCombat player);
    }
}