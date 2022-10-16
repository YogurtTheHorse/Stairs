using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Infinite Resetter", fileName = "InfiniteResetConfig", order = 0)]
    public class InfiniteResetConfig : ScriptableObject
    {
        public float resetDistance = 1000;
    }
}