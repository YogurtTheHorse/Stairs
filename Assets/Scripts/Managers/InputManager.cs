using Configs;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Managers
{
    public class InputManager : MonoBehaviour
    {
        public Bootstrapper bootstrapper;

        public StairsManager stairs;

        public InputConfig config;

        private bool _swipeJustStarted;

        private BallController _ball;

        private Vector2 _swipeStartPos, _swipeCurrentPosition;

        private float _swipeStartTime;

        private float _swipeDirection = 0;

        private float _lastJumpPressed = -10, _lastSideSwapped = -10;

        public void Init(BallController ball)
        {
            _ball = ball;
            _ball.ballAtZenith.AddListener(BallAtZenith);
        }

        public void DoMove(float direction)
        {
            _swipeDirection = direction;
            _lastSideSwapped = Time.time;
        }

        public void ProcessTap(InputAction.CallbackContext ctx)
        {
            if (ctx.performed)
            {
                _lastJumpPressed = Time.time;
            }
        }

        public void ProcessMove(InputAction.CallbackContext ctx)
        {
            var v = ctx.ReadValue<float>();
            if (v != 0)
            {
                DoMove(v);
            }
        }

        public void TrackContact(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                _swipeJustStarted = true;
                _swipeStartTime = Time.time;
            }
            else if (ctx.canceled)
            {
                var delta = _swipeStartPos - _swipeCurrentPosition;
                var duration = Time.time - _swipeStartTime;

                // maybe optimize to sqrMagnitude?

                if (duration > config.minSwipeDuration && delta.magnitude > config.minSwipeLength)
                {
                    DoMove(Mathf.Sign(delta.x));
                }
            }
        }


        public void TrackPosition(InputAction.CallbackContext ctx)
        {
            _swipeCurrentPosition = ctx.ReadValue<Vector2>();

            if (_swipeJustStarted)
            {
                _swipeStartPos = _swipeCurrentPosition;
                _swipeJustStarted = false;
            }
        }

        private void BallAtZenith()
        {
            stairs.RemoveStep();
        }

        public void Update()
        {
            if (!_ball)
                return;

            if (Time.time - _lastJumpPressed < config.coyoteTime && (!_ball.IsJumping || _ball.isFalling))
            {
                DoAction();
            }

            if (Time.time - _lastSideSwapped < config.coyoteTime && !_ball.isSideJumping)
            {
                _ball.SideJump(_swipeDirection);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(_swipeStartPos, _swipeCurrentPosition);
        }

        private void DoAction()
        {
            _ball.JumpForward();
            stairs.AddStep();
        }
    }
}