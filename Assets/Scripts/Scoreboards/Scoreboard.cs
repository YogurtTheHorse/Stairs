using System.Collections.Generic;
using System.IO;
using Configs;
using UnityEngine;

namespace Scoreboards
{
    public class Scoreboard : MonoBehaviour
    {
        public ScoresConfig config;

        private string SavePath => Path.Combine(Application.persistentDataPath, "scores.json");


        public List<Score> AddEntry(Score score)
        {
            var savedScores = GetSavedScores();
            var scoreAdded = false;

            for (var i = 0; i < savedScores.highScores.Count; i++)
            {
                if (score.score > savedScores.highScores[i].score)
                {
                    savedScores.highScores.Insert(i, score);
                    scoreAdded = true;
                    break;
                }
            }

            if (!scoreAdded && savedScores.highScores.Count < config.maxScoresCount)
            {
                savedScores.highScores.Add(score);
            }

            SaveScores(savedScores);

            return savedScores.highScores;
        }

        public List<Score> GetScores() => GetSavedScores().highScores;

        private ScoreboardSaveData GetSavedScores()
        {
            if (!File.Exists(SavePath))
            {
                File.Create(SavePath).Dispose();
                return new ScoreboardSaveData();
            }

            using var stream = new StreamReader(SavePath);
            var json = stream.ReadToEnd();

            return JsonUtility.FromJson<ScoreboardSaveData>(json);
        }

        private void SaveScores(ScoreboardSaveData scoreboardSaveData)
        {
            using var stream = new StreamWriter(SavePath);
            var json = JsonUtility.ToJson(scoreboardSaveData, true);

            stream.Write(json);
        }
    }
}