using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/StairsConfig", fileName = "StairsConfig", order = 0)]
    public class StairsConfig : ScriptableObject
    {
        public Stair stairPrefab;

        public Vector2 stepSize = Vector2.one * 0.5f;

        public float stairWidth = 4f;
    }
}