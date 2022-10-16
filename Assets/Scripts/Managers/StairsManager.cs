using System;
using System.Collections.Generic;
using System.Linq;
using Configs;
using Unity.Mathematics;
using UnityEngine;

namespace Managers
{
    public class StairsManager : MonoBehaviour
    {
        public StairsConfig stairsConfig;
    
        public Camera mainCamera;

        public Stair startingStair;

        private Transform _transform;

        private LinkedList<Stair> _stairs;
        private Queue<Stair> _stairsPool;
        
        public Stair LastStair => _stairs.Last.Value;

        private void Awake()
        {
            _transform = transform;
            _stairs = new LinkedList<Stair>();
            _stairsPool = new Queue<Stair>();
        }
    
        public void GenerateStairs()
        {
            var viewWidth = 2f * mainCamera.orthographicSize * mainCamera.aspect;
            var stairsWidth = viewWidth / Mathf.Cos(mainCamera.transform.rotation.eulerAngles.y * Mathf.Deg2Rad);

            // 5 added to be sure that there wont be empty space on the edges
            var stairsCount = Mathf.CeilToInt(stairsWidth / stairsConfig.stepSize.x) + 10;

            if (stairsCount % 2 == 0)
            {
                stairsCount++;
            }

            var startPoint = -stairsConfig.stepSize * stairsCount * 0.5f;

            for (var i = 0; i < stairsCount; i++)
            {
                var stair = Instantiate(
                    stairsConfig.stairPrefab,
                    startPoint + i * stairsConfig.stepSize,
                    quaternion.identity,
                    _transform
                );

                if (i > 0)
                {
                    _stairs.Last.Value.nextStair = stair;
                }

                if (i == stairsCount / 2 + 1)
                {
                    startingStair = stair;
                }

                _stairs.AddLast(stair);
            }
        }

        public void AddStep()
        {
            var lastStep = _stairs.Last.Value;
            var nextPos = lastStep.transform.position + (Vector3)stairsConfig.stepSize;
            Stair newStair;

            if (_stairsPool.Any())
            {
                newStair = _stairsPool.Dequeue();
                newStair.transform.position = nextPos;
                newStair.Activate();
            }
            else
            {
                newStair = Instantiate(
                    stairsConfig.stairPrefab,
                    nextPos,
                    quaternion.identity,
                    _transform
                );
            }

            lastStep.nextStair = newStair;
            _stairs.AddLast(newStair);
        }

        public void RemoveStep()
        {
            var firstStep = _stairs.First.Value;
            _stairs.RemoveFirst();

            firstStep.Hide();

            _stairsPool.Enqueue(firstStep);
        }

        public void ResetTo(Vector3 offset)
        {
            foreach (var stair in _stairs)
            {
                stair.transform.position -= offset;
            }
        }
    }
}