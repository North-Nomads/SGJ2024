using System.Collections.Generic;
using UnityEngine;

namespace SGJ.Mobs.Saw
{
    public class SawMob : Mob
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float attackChargeTime;
        [SerializeField] private float spinAttackDuration;
        [SerializeField] private float spinningSpeed;

        public void Start()
        {
            AllStates = new List<MobState>
            {
                new ChaseState(Agent, this, Player, attackRange),
                new SawAttackState(Agent, this, Player, attackChargeTime, spinAttackDuration, attackRange, spinningSpeed)
            };

            CurrentState = AllStates[0];
        }
    }
}
