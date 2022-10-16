using System.Collections.Generic;
using Configs;
using Managers;
using Scoreboards;
using UnityEngine;

namespace UI
{
    public class HighScoresMenu : MonoBehaviour
    {
        public ScoresConfig config;

        public Bootstrapper bootstrapper;

        public Transform scoresList;
        
        public Scoreboard scoreboard;
        
        private GameObject _gameObject;

        private void Awake()
        {
            _gameObject = gameObject;
            _gameObject.SetActive(false);
        }

        public void Activate()
        {
            _gameObject.SetActive(true);

            foreach (Transform scoreObject in scoresList)
            {
                Destroy(scoreObject.gameObject);
            }

            List<Score> scores = scoreboard.GetScores();
            
            for (var i = 0; i < scores.Count && i < 5; i++)
            {
                var score = scores[i];
                var entry = Instantiate(config.scoresObject, scoresList);

                entry.SetScore(i + 1, score);
            }
        }

        public void Proceed()
        {
            _gameObject.SetActive(false);
            bootstrapper.Restart();
        }
    }
}