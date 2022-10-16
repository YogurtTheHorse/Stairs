using System;
using System.Collections.Generic;

namespace Scoreboards
{
    [Serializable]
    public class ScoreboardSaveData
    {
        public List<Score> highScores = new List<Score>();
    }
}