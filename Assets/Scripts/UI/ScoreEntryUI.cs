using Scoreboards;
using TMPro;
using UnityEngine;

namespace UI
{
    public class ScoreEntryUI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;

        public void SetScore(int place, Score score)
        {
            scoreText.SetText($"{place}. {score.author} - {score.score}");
        }
    }
}