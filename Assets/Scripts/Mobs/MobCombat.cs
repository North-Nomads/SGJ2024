using System;
using UnityEngine;

namespace SGJ.Mobs
{
    public class MobCombat
    {
        private readonly float _maxHealth;
        private float _currentHealth;
        private Mob _thisMob;

        public EventHandler<Mob> OnMobDied = delegate { };
        public EventHandler OnMobHit;

        public float CurrentHealth
        {
            get => _currentHealth;
            private set
            {
                _currentHealth = value;
                if (_currentHealth < 0)
                {
                    if (!_isDead)
                        OnMobDied(this, _thisMob);
                    _isDead = true;
                }
            }
        }
        
        private bool _isDead;

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
            OnMobHit.Invoke(this, null);
        }
    }
}
