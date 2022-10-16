using DG.Tweening;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Obstacles", fileName = "ObstaclesConfig", order = 0)]
    public class ObstaclesConfig : ScriptableObject
    {
        public float spawnEvery = 3;

        public Obstacle[] obstacles;

        public float timeToLive = 10f;
        
        [Header("Hiding")]
        public float hidingDuration = 0.1f;

        public Ease hidingEase = Ease.InQuad;

        [Header("Jump")] public int stepsInJump = 2;

        public float jumpPower = 3f;

        public float jumpDuration = 0.5f;
    }
}