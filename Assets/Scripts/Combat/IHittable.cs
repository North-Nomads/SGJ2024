namespace SGJ.Combat
{
    public interface IHittable
    {
        public float MaxHealth { get; }
        public float CurrentHealth { get; }
        public void HandleHit(float incomeDamage);
        public void DisplayHitImpact();
        public void HandleDeath();
    }
}