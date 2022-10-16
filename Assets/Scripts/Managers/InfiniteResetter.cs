using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Configs;
using UnityEngine;

namespace Managers
{
    public class InfiniteResetter : MonoBehaviour
    {
        public InfiniteResetConfig config;

        public StairsManager stairs;

        public Camera mainCamera;

        public CinemachineVirtualCamera virtualCamera;

        private BallController _ball;

        public void Init(BallController ball)
        {
            _ball = ball;

            _ball.jumpEnded.AddListener(CheckScores);
        }

        private void CheckScores()
        {
            var distanceFromBeginning = _ball.transform.position.sqrMagnitude;

            if (distanceFromBeginning > config.resetDistance * config.resetDistance)
            {
                Reset();
            }
        }

        private void Reset()
        {
            var vCamTransform = virtualCamera.transform;

            var ballTransform = _ball.transform;
            var ballPos = ballTransform.position;

            mainCamera.transform.position -= ballPos;
            virtualCamera.ForceCameraPosition(vCamTransform.position - ballPos, vCamTransform.rotation);
            
            ballTransform.position = Vector3.zero;
            
            stairs.ResetTo(ballPos);
        }
    }
}