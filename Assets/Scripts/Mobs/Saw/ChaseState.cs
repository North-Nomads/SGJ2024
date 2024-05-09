using SGJ.Mobs;
using SGJ.Mobs.Saw;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ
{
    public class ChaseState : MobState
    {
        private float _attackRange;
        public ChaseState(NavMeshAgent agent, Mob self, Transform player, float attackRange) : base(agent, self, player)
        {
            _attackRange = attackRange;
        }

        public override void BehaveThisState()
        {
            Agent.SetDestination(Player.position);
            if (Agent.remainingDistance <= _attackRange)
                Self.SwitchState<AttackState>();
        }

        public override void OnStateStarted()
        { }

        public override void OnStateStopped()
        { }
    }
}