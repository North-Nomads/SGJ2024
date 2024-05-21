using SGJ.Player;

namespace SGJ.Commands
{
    public abstract class InteractionCommand
    {
        public abstract void Execute(PlayerInteraction player);
    }
}