using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class HomeScreen : MonoBehaviour
    {
        [SerializeField] private AchievementManager achievementManager;
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text todayText;

        // Wordt uitgevoerd elke keer dat het panel zichtbaar wordt
        private void OnEnable()
        {
            achievementManager.CoinsChanged += UpdateCoins;
            UpdateCoins(achievementManager.Coins);
            UpdateTodayStatus();
        }

        private void OnDisable()
        {
            achievementManager.CoinsChanged -= UpdateCoins;
        }

        private void UpdateCoins(int coins)
        {
            coinsText.text = $"Coins: {coins}";
        }
        
        private void UpdateTodayStatus()
        {
            todayText.text = achievementManager.HasAddedToday()
                ? "Vandaag: prestatie toegevoegd"
                : "Vandaag: nog geen prestatie toegevoegd";
        }
    }
}