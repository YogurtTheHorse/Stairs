using System;
using Managers;
using Scoreboards;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnterScoreMenu : MonoBehaviour
    {
        public Scoreboard scoreboard;

        public ScoresCounter scoreCounter;

        public HighScoresMenu highScoresMenu;
        
        public TMP_InputField textInput;
        
        private GameObject _gameObject;

        private void Awake()
        {
            _gameObject = gameObject;
            _gameObject.SetActive(false);
        }

        public void Activate()
        {
            _gameObject.SetActive(true);
        }
        
        public void Ok()
        {
            scoreboard.AddEntry(
                new Score {
                    author = textInput.text,
                    score = scoreCounter.score
                }
            );
            Cancel();
        }

        public void Cancel()
        {
            _gameObject.SetActive(false);
            highScoresMenu.Activate();
        }
    }
}