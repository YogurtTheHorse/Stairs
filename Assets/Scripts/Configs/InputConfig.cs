using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/Input config", order = 0)]
    public class InputConfig : ScriptableObject
    {
        public float coyoteTime = 0.1f;

        [Header("Swipes")] public float minSwipeLength = 50f;

        public float minSwipeDuration = 0.1f;
    }
}