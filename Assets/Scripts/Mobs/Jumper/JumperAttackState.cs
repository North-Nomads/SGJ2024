using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace SGJ.Mobs.Saw
{
    public class JumperAttackState : MobState
    {
        private readonly AnimationCurve _jumpHeightCurve;
        private readonly Transform _jumperHead;
        private readonly float _jumpChargeTime;
        private readonly float _jumpSpeed;
        private readonly float _startJumpRange;
        private readonly float _jumpDuration;
        private readonly float _defaultPositionY;
        private readonly float _desiredPositionY;
        private readonly Transform _jumperLegs;
        private readonly Vector3 _defaultLegsScale;
        private readonly Vector3 _desiredLegsScale;
        private float _jumpChargeTimeLeft;
        private bool _isInJump;

        public JumperAttackState(NavMeshAgent agent, JumpMob jumpMob, Transform player, float startJumpRange, float jumpDuration,
            float jumpChargeTime, AnimationCurve jumpHeightCurve, float jumpSpeed, Transform head, float defaultPositionY, 
            float desiredPositionY, Transform jumperLegs, Vector3 defaultScale, Vector3 desiredScale) : base(agent, jumpMob, player)
        {
            _startJumpRange = startJumpRange;
            _jumpDuration = jumpDuration;
            _jumpChargeTime = jumpChargeTime;
            _jumpHeightCurve = jumpHeightCurve;
            _jumpSpeed = jumpSpeed;
            _jumperHead = head;
            _defaultPositionY = defaultPositionY;
            _desiredPositionY = desiredPositionY;
            _jumperLegs = jumperLegs;
            _defaultLegsScale = defaultScale;
            _desiredLegsScale = desiredScale;
        }

        public override void BehaveThisState()
        {
            if (_isInJump)
            {
                Agent.SetDestination(Player.position);
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
                var chargingElapsedTime = 0f;
                var percent = chargingElapsedTime / _jumpChargeTime;
                while (chargingElapsedTime < _jumpChargeTime)
                {
                    _jumperHead.position = new Vector3(_jumperHead.position.x,
                        Mathf.Lerp(_defaultPositionY, _desiredPositionY, percent),
                        _jumperHead.position.z);
                    
                    _jumperLegs.localScale = Vector3.Lerp(_defaultLegsScale, _desiredLegsScale, percent);

                    chargingElapsedTime += Time.deltaTime;
                    percent = chargingElapsedTime / _jumpChargeTime;
                    yield return null;
                }

                _jumperHead.position = new Vector3(_jumperHead.position.x, _defaultPositionY, _jumperHead.position.z);
                _jumperLegs.localScale = _defaultLegsScale;
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

        public override void OnStateStarted() => Agent.speed = 0f;

        public override void OnStateStopped() => Agent.speed = Self.DefaultSpeed;
    }
}