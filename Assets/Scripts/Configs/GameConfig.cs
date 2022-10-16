using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(menuName = "Configs/Game Config", fileName = "GameConfig", order = 0)]
    public class GameConfig : ScriptableObject
    {
        public AudioClip explosion;
    }
}