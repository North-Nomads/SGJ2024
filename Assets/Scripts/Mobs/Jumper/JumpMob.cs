using System.Collections.Generic;
using UnityEngine;

namespace SGJ.Mobs.Saw
{
    public class JumpMob : Mob
    {
        [SerializeField] private float attackRange;
        [SerializeField] private float jumpStrength;
        [SerializeField] private float jumpDuration;
        [SerializeField] private float jumpChargeTime;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private AnimationCurve jumpCurve;
        [SerializeField] private Transform jumpChargeMovingPart;
        [SerializeField] private Transform desiredMovingPartPosition;
        [SerializeField] private Transform legsPosition;
        [SerializeField] private Transform torus;
        [SerializeField] private Vector3 desiredScale;

        public void Start()
        {
            AllStates = new List<MobState>
            {
                new JumperChaseState(Agent, this, Player.transform, attackRange),
                new JumperAttackState(Agent, this, Player.transform, jumpStrength, jumpDuration, jumpChargeTime, jumpCurve,
                jumpSpeed, jumpChargeMovingPart, jumpChargeMovingPart.position.y, desiredMovingPartPosition.position.y,
                legsPosition, legsPosition.localScale, desiredScale, torus)
            };

            CurrentState = AllStates[0];
        }
    }
}
