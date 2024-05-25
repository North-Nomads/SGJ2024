namespace SGJ.Combat
{
    public interface IAcceleratedWeaponTimer : IWeaponTimer
    {
        public float AccelerationProcentage { get; }

    }
}