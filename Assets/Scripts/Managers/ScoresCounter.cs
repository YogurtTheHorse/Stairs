using TMPro;
using UnityEngine;

namespace Managers
{
    public class ScoresCounter : MonoBehaviour
    {
        public int score = 0;
    
        public TextMeshProUGUI scoresText;
        private BallController _ball;

        public void Start()
        {
            SyncText();
        }

        public void SyncText()
        {
            if (!scoresText) return;

            scoresText.text = score.ToString();
        }

        public void Init(BallController ball)
        {
            _ball = ball;
            _ball.ballAtZenith.AddListener(AddScore);
            score = 0;
            
            SyncText();
        }

        private void AddScore()
        {
            score++;
        
            SyncText();
        }
    }
}