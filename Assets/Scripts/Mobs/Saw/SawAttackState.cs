using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs.Saw
{
    internal class SawAttackState : MobState
    {
        private readonly float _spinAttackDuration;
        private readonly float _attackRange;
        private readonly float _spinningSpeed;
        private readonly float _attackChargeTime;
        private float _attackChargeTimeLeft;
        private bool _isChargingAttack;

        public SawAttackState(NavMeshAgent agent, Mob self, Transform player, float attackChargeTime,
            float spinAttackDuration, float attackRange, float spinningSpeed) : base(agent, self, player)
        {
            _attackChargeTime = attackChargeTime;
            _attackChargeTimeLeft = attackChargeTime;
            _spinAttackDuration = spinAttackDuration;
            _attackRange = attackRange;
            _spinningSpeed = spinningSpeed;
        }

        public override void BehaveThisState()
        {
            if (_isChargingAttack)
                return;

            _attackChargeTimeLeft -= Time.deltaTime;
            if (_attackChargeTimeLeft < 0f)
                PerformAttack();
            
            if (Vector3.Distance(Transform.position, Player.position) >= _attackRange)
                Self.SwitchState<ChaseState>();
        }

        private void PerformAttack()
        {
            Debug.Log("Started Performing");
            _isChargingAttack = true;
            Self.StartCoroutine(SpinAround());

            IEnumerator SpinAround()
            {
                float startRotation = Transform.eulerAngles.y;
                float endRotation = startRotation + 360.0f;
                float spinningElapsed = 0.0f;
                while (spinningElapsed < _spinAttackDuration)
                {
                    spinningElapsed += Time.deltaTime * _spinningSpeed;
                    float yRotation = Mathf.Lerp(startRotation, endRotation, spinningElapsed / _spinAttackDuration) % 360.0f;
                    Transform.eulerAngles = new Vector3(Transform.eulerAngles.x, yRotation, Transform.eulerAngles.z);
                    yield return null;
                }
                _isChargingAttack = false;
                _attackChargeTimeLeft = _attackChargeTime;
            }
        }

        public override void OnStateStarted()
        {
            _attackChargeTimeLeft = _attackChargeTime;
            Agent.SetDestination(Transform.position);
        }

        public override void OnStateStopped() { }
    }
}