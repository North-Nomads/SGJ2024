using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs.Saw
{
    public class JumperAttackState : MobState
    {
        private readonly AnimationCurve _jumpHeightCurve;
        private readonly float _jumpChargeTime;
        private readonly float _jumpSpeed;
        private readonly float _startJumpRange;
        private readonly float _jumpDuration;
        private float _jumpChargeTimeLeft;
        private bool _isInJump;

        public JumperAttackState(NavMeshAgent agent, JumpMob jumpMob, Transform player, float startJumpRange, float jumpDuration,
            float jumpChargeTime, AnimationCurve jumpHeightCurve, float jumpSpeed) 
            : base(agent, jumpMob, player)
        {
            _startJumpRange = startJumpRange;
            _jumpDuration = jumpDuration;
            _jumpChargeTime = jumpChargeTime;
            _jumpHeightCurve = jumpHeightCurve;
            _jumpSpeed = jumpSpeed;
        }

        public override void BehaveThisState()
        {
            if (_isInJump)
            {
                Transform.LookAt(Player.position);
                return;   
            }

            _jumpChargeTimeLeft -= Time.deltaTime;
            if (_jumpChargeTimeLeft < 0f)
                PerformJump();
        }

        private void PerformJump()
        {
            Self.StartCoroutine(PerformJumpAfterDelay());

            IEnumerator PerformJumpAfterDelay()
            {
                _isInJump = true;
                yield return new WaitForSeconds(_jumpChargeTime);

                Agent.speed = _jumpSpeed;
                Agent.SetDestination(Player.position);

                var inJumpTimeElapsed = 0f;
                while (inJumpTimeElapsed < _jumpDuration)
                {
                    inJumpTimeElapsed += Time.deltaTime;
                    yield return null;

                    float value = _jumpHeightCurve.Evaluate(inJumpTimeElapsed);
                    Transform.position = new Vector3(Transform.position.x, value, Transform.position.z);
                }
                
                _jumpChargeTimeLeft = _jumpChargeTime;
                if (Vector3.Distance(Transform.position, Player.position) >= _startJumpRange)
                    Self.SwitchState<JumperChaseState>();
                _isInJump = false;
            }
        }

        public override void OnStateStarted()
        {
            Agent.speed = 0f;
        }

        public override void OnStateStopped()
        {
            Agent.speed = Self.DefaultSpeed;
        }
    }
}