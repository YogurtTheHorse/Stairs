using System;
using Cinemachine;
using Configs;
using UI;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class Bootstrapper : MonoBehaviour
    {
        public GameConfig gameConfig;
        
        [Header("Managers")]
        public InputManager inputs;
    
        public StairsManager stairsManager;

        public ScoresCounter scoresCounter;

        public InfiniteResetter infiniteResetter;

        public ObstaclesSpawner obstaclesSpawner;

        public EnterScoreMenu enterScoreMenu;

        [Header("Camera")]
        public CinemachineVirtualCamera virtualCamera;

        [Header("Prefabs")]
        public BallController ballPrefab;

        private Stair _startingStair;
        private BallController _ball;
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            stairsManager.GenerateStairs();

            _startingStair = stairsManager.startingStair;

            Restart();
        }

        private void PrepareRestart()
        {
            _audioSource.PlayOneShot(gameConfig.explosion);
            _startingStair = _ball.currentStair;
            
            enterScoreMenu.Activate();
        }

        public void Restart()
        {
            _ball = Instantiate(
                ballPrefab,
                ballPrefab.ballConfig.ballRadius * Vector3.up + _startingStair.transform.position,
                Quaternion.identity
            );

            _ball.currentStair = _startingStair;
            _ball.onDeath.AddListener(PrepareRestart);

            var ballTransform = _ball.transform;
        
            virtualCamera.Follow = ballTransform;
            virtualCamera.LookAt = ballTransform;

            inputs.Init(_ball);
            scoresCounter.Init(_ball);
            infiniteResetter.Init(_ball);
            obstaclesSpawner.Init(_ball);
        }
    }
}