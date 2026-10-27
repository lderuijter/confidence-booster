using System;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Managers
{
    public enum AddAchievementResult
    {
        Success,
        EmptyDescription,
        AlreadyAddedToday
    }

    public class AchievementManager : MonoBehaviour
    {
        private const int CoinsPerAchievement = 10;

        public int Coins => GameSession.Current.coins;

        public List<AchievementData> Achievements => GameSession.Current.achievements;

        // Wordt afgevuurd als de coins veranderen, zodat de UI zich kan bijwerken
        public event Action<int> CoinsChanged;

        private string Today()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public bool HasAddedToday()
        {
            return GameSession.Current.lastAchievementDate == Today();
        }

        // Probeert een prestatie toe te voegen en geeft terug wat er is gebeurd
        public AddAchievementResult TryAddAchievement(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                return AddAchievementResult.EmptyDescription;
            }

            // Maximaal één prestatie per dag
            if (HasAddedToday())
            {
                return AddAchievementResult.AlreadyAddedToday;
            }

            GameData data = GameSession.Current;

            AchievementData achievement = new AchievementData();
            achievement.description = description.Trim();
            achievement.date = Today();
            data.achievements.Add(achievement);

            data.lastAchievementDate = Today();
            data.coins += CoinsPerAchievement;

            GameSession.Save();
            CoinsChanged?.Invoke(data.coins);

            return AddAchievementResult.Success;
        }
    }
}