using System;

namespace SGJ.Mobs
{
    public class MobCombat
    {
        private readonly float _maxHealth;
        private float _currentHealth;

        public EventHandler OnMobDied = delegate { };

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                if (_currentHealth < 0)
                    OnMobDied(this, null);
            }
        }
        public bool IsAlive => _currentHealth > 0;
         
        public MobCombat(float maxHealth)
        {
            _maxHealth = maxHealth;
            CurrentHealth = maxHealth;
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
