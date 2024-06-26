﻿using System.Collections.Generic;
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
                new SawChaseState(Agent, this, Player.transform, attackRange),
                new SawAttackState(Agent, this, Player.transform, attackChargeTime, spinAttackDuration, attackRange, spinningSpeed)
            };

            CurrentState = AllStates[0];
        }
    }
}
