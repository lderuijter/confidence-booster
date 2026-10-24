using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class HomeScreen : MonoBehaviour
    {
        [SerializeField] private AchievementManager achievementManager;
        [SerializeField] private TMP_Text coinsText;

        // Wordt uitgevoerd elke keer dat het panel zichtbaar wordt
        private void OnEnable()
        {
            achievementManager.CoinsChanged += UpdateCoins;
            UpdateCoins(achievementManager.Coins);
        }

        private void OnDisable()
        {
            achievementManager.CoinsChanged -= UpdateCoins;
        }

        private void UpdateCoins(int coins)
        {
            coinsText.text = $"Coins: {coins}";
        }
    }
}