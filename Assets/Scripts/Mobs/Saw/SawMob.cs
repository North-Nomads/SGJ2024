using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SGJ.Mobs.Saw
{
    public class SawMob : Mob
    {
        [SerializeField] private float attackRange;
        
        public void Start()
        {
            AllStates = new List<MobState>
            {
                new ChaseState(Agent, this, Player, attackRange),
                new AttackState(Agent, this, Player)
            };
        }
    }
}
