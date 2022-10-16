using UI;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "ScoresConfig", menuName = "Configs/Scores config", order = 0)]
    public class ScoresConfig : ScriptableObject
    {
        public int maxScoresCount = 5;
        
        public ScoreEntryUI scoresObject;
    }
}