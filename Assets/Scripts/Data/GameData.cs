using System;
using System.Collections.Generic;

namespace Data
{
    [Serializable]
    public class GameData
    {
        public int coins;
        public string lastAchievementDate = "";

        public List<AchievementData> achievements = new();
        public List<string> inventoryItemIds = new();
    }

    [Serializable]
    public class AchievementData
    {
        public string description;
        public string date;
    }
}