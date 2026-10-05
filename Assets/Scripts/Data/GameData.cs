using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public int coins = 0;
        public string lastAchievementDate = "";

        public List<AchievementData> achievements = new List<AchievementData>();
        public List<string> inventoryItemIds = new List<string>();
    }

    [Serializable]
    public class AchievementData
    {
        public string description;
        public string date;
    }
}