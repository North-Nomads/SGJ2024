namespace SGJ.Weapons
{
    public interface IWeapon
    {
        public WeaponModel Weapon { get; }
        public int AmmoLeft { set; }
        public IProjectile WeaponProjectile { set; }
        public void TryPerformAttack();
    }
}
