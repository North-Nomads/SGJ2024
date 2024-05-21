namespace SGJ.Weapons.Modifiers
{
    public interface IAcceleratedWeapon
    {
        public float FinalFireDelay { get; }
        public float TimeToRampUp { get; }
    }
}
