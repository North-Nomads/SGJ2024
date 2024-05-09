using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs.Saw
{
    internal class AttackState : MobState
    {
        private readonly float _spinAttackDuration;
        private readonly float _attackChargeTime;
        private readonly float _rotationSpeed;
        private float _attackChargeTimeLeft;

        public AttackState(NavMeshAgent agent, Mob self, Transform player, float attackChargeTime, float spinAttackDuration, float rotationSpeed)
            : base(agent, self, player)
        {
            _attackChargeTime = attackChargeTime;
            _attackChargeTimeLeft = attackChargeTime;
            _spinAttackDuration = spinAttackDuration;
            _rotationSpeed = rotationSpeed;
        }

        public override void BehaveThisState()
        {
            _attackChargeTimeLeft -= _attackChargeTime;
            if (_attackChargeTimeLeft < 0f)
                PerformAttack();
        }

        private void PerformAttack()
        {
            Self.StartCoroutine(SpinAround());

            IEnumerator SpinAround()
            {
                var spinningTimeElapsed = 0f;
                while (spinningTimeElapsed > 0)
                {
                    Transform.rotation = Quaternion.Euler(0, 0, spinningTimeElapsed * _rotationSpeed);
                    spinningTimeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
        }

        public override void OnStateStarted()
        {
            _attackChargeTimeLeft = _attackChargeTime;
        }

        public override void OnStateStopped() { }
    }
}