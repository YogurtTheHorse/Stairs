using DG.Tweening;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "BallConfig", menuName = "Configs/Ball config", order = 0)]
    public class BallConfig : ScriptableObject
    {
        public GameObject destroyEffect;
        
        [Header("Ball")]
        public float ballRadius = 0.125f;

        public float ballAverageSpeed = 1f;

        public float jumpHeight = 0.8f;

        [Header("Side jump")]
        public float sideJumpHeight = 0.2f;

        public float sideJumpLength = 0.1f;

        public float sideJumpSpeed = 0.5f;

        [Header("Easings")]
        public Ease jumpEase = Ease.OutCirc;

        public Ease fallEase = Ease.InCirc;

        [Header("Sounds")] public AudioClip jump;
    }
}