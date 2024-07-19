namespace SGJ.Combat
{
    public interface IWeaponTimer 
    {
        public void OnShot();

        public void OnGameTick();

        public bool ReadyToFire { get; }
    }
}