using System;

namespace SGJ.Mobs
{
    public class MobCombat
    {
        private readonly float _maxHealth;
        private float _currentHealth;
        private Mob _thisMob;

        public EventHandler<Mob> OnMobDied = delegate { };

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                if (_currentHealth < 0)
                    OnMobDied(this, _thisMob);
            }
        }

        public bool IsAlive => _currentHealth > 0;
         
        public MobCombat(float maxHealth, Mob thisMob)
        {
            _maxHealth = maxHealth;
            CurrentHealth = maxHealth;
            _thisMob = thisMob;
        }

        public void ResetCurrentValues()
        {
            CurrentHealth = _maxHealth;
        }

        public void HandleIncomeDamage(float incomeDamage)
        {
            CurrentHealth -= incomeDamage;
        }
    }
}
