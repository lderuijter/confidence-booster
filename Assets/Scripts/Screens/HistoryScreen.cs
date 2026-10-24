using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class HistoryScreen : MonoBehaviour
    {
        // De manager waar de opgeslagen prestaties in staan
        [SerializeField] private AchievementManager achievementManager;

        // Het object in de Scroll View waar de regels in komen te staan
        [SerializeField] private Transform content;

        // Het voorbeeld van één regel, dat we kopiëren voor elke prestatie
        [SerializeField] private TMP_Text itemPrefab;

        // Unity roept dit aan elke keer dat het panel zichtbaar wordt
        private void OnEnable()
        {
            // Eerst de oude regels weghalen, anders komen er dubbele bij
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            var achievements = achievementManager.Achievements;

            // Van achter naar voren lopen, zodat de nieuwste prestatie bovenaan staat
            for (int i = achievements.Count - 1; i >= 0; i--)
            {
                // Maak een kopie van de prefab binnen de Scroll View
                TMP_Text item = Instantiate(itemPrefab, content);

                // Zet de datum en de omschrijving in de regel
                item.text = $"{achievements[i].date}  -  {achievements[i].description}";
            }
        }
    }
}