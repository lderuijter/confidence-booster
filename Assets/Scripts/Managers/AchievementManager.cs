using System;
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
    
    // Draait eerder dan andere scripts, zodat de data al geladen is als het home-scherm de coins opvraagt
    [DefaultExecutionOrder(-100)]
    public class AchievementManager : MonoBehaviour
    {
        private const int CoinsPerAchievement = 10;

        private GameData _data;

        public int Coins => _data.coins;

        // Wordt afgevuurd als de coins veranderen, zodat de UI zich kan bijwerken
        public event Action<int> CoinsChanged;

        private void Awake()
        {
            _data = SaveManager.Load();
        }

        private string Today()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        public bool HasAddedToday()
        {
            return _data.lastAchievementDate == Today();
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

            AchievementData achievement = new AchievementData();
            achievement.description = description.Trim();
            achievement.date = Today();
            _data.achievements.Add(achievement);

            _data.lastAchievementDate = Today();
            _data.coins += CoinsPerAchievement;

            SaveManager.Save(_data);
            CoinsChanged?.Invoke(_data.coins);

            return AddAchievementResult.Success;
        }
    }
}