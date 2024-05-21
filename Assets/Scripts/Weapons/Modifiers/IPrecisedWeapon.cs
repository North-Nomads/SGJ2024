namespace SGJ.Weapons.Modifiers
{
    public interface IPrecisedWeapon
    {
        public float FinalSpreadAngle { get; }
        public float TimeToIncreaseAccuracy { get; }
    }
}
