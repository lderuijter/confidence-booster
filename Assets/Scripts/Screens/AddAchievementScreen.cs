using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class AddAchievementScreen : MonoBehaviour
    {
        [SerializeField] private AchievementManager achievementManager;
        [SerializeField] private TMP_InputField descriptionInput;
        [SerializeField] private Button addButton;
        [SerializeField] private TMP_Text messageText;

        private void Awake()
        {
            addButton.onClick.AddListener(OnAddClicked);
        }

        // Maak de melding leeg zodra het panel getoond wordt
        private void OnEnable()
        {
            messageText.text = "";
        }

        private void OnAddClicked()
        {
            AddAchievementResult result = achievementManager.TryAddAchievement(descriptionInput.text);

            switch (result)
            {
                case AddAchievementResult.Success:
                    descriptionInput.text = "";
                    messageText.text = "Prestatie toegevoegd! Je hebt 10 coins verdiend.";
                    break;
                case AddAchievementResult.EmptyDescription:
                    messageText.text = "Vul eerst een prestatie in.";
                    break;
                case AddAchievementResult.AlreadyAddedToday:
                    messageText.text = "Je hebt vandaag al een prestatie toegevoegd.";
                    break;
            }
        }
    }
}