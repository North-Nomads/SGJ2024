using SGJ.Mobs;
using SGJ.Mobs.Saw;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ
{
    public class JumperChaseState : MobState
    {
        private float _attackRange;
        public JumperChaseState(NavMeshAgent agent, Mob self, Transform player, float attackRange) : base(agent, self, player)
        {
            _attackRange = attackRange;
        }

        public override void BehaveThisState()
        {
            Agent.SetDestination(Player.position);
            if (Vector3.Distance(Transform.position, Player.position)<= _attackRange)
                Self.SwitchState<JumperAttackState>();
        }

        public override void OnStateStarted()
        { }

        public override void OnStateStopped()
        { }
    }
}