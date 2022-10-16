using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using DG.Tweening;
using MyBox;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        public ObstaclesConfig config;

        public StairsManager stairs;

        private BallController _ball;

        private Dictionary<Obstacle, Queue<Obstacle>> _pools;
        private Transform _transform;

        private HashSet<Obstacle> _activeObstacles;

        private void Awake()
        {
            _transform = transform;
            _activeObstacles = new HashSet<Obstacle>();
            _pools = new Dictionary<Obstacle, Queue<Obstacle>>();
        }

        private void Start()
        {
            DOTween
                .Sequence()
                .SetDelay(config.spawnEvery)
                .OnStepComplete(SpawnObstacle)
                .SetLoops(-1);
        }

        public void Init(BallController ball)
        {
            _ball = ball;

            _ball.onDeath.AddListener(DestroyAllObstacles);
        }

        private void SpawnObstacle()
        {
            if (!_ball)
                return;

            var stair = stairs.LastStair;
            var prefab = config.obstacles.GetRandom();

            var stairTransform = stair.transform;

            var obstacle = CreateNewObstacle(prefab, stairTransform);
            _activeObstacles.Add(obstacle);

            obstacle.Activate(stair);
        }

        private Queue<Obstacle> GetPool(Obstacle prefab)
        {
            if (_pools.TryGetValue(prefab, out var pool))
            {
                return pool;
            }

            return _pools[prefab] = new Queue<Obstacle>();
        }

        private Obstacle CreateNewObstacle(Obstacle prefab, Transform parent)
        {
            Queue<Obstacle> pool = GetPool(prefab);
            
            if (pool.Any())
            {
                return pool.Dequeue();
            }

            var newObstacle = Instantiate(prefab);

            newObstacle.onHide.AddListener(
                () =>
                {
                    _activeObstacles.Remove(newObstacle);

                    newObstacle.transform.parent = _transform;

                    pool.Enqueue(newObstacle);
                }
            );

            return newObstacle;
        }

        private void DestroyAllObstacles()
        {
            foreach (var obstacle in _activeObstacles)
            {
                obstacle.Hide();
            }
        }
    }
}