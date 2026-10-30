using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject addAchievementPanel;
        [SerializeField] private GameObject historyPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject inventoryPanel;

        [SerializeField] private Button homeButton;
        [SerializeField] private Button addAchievementButton;
        [SerializeField] private Button historyButton;
        [SerializeField] private Button shopButton;
        [SerializeField] private Button inventoryButton;

        private GameObject[] _panels;

        private void Awake()
        {
            _panels = new[] { homePanel, addAchievementPanel, historyPanel, shopPanel, inventoryPanel };

            homeButton.onClick.AddListener(ShowHome);
            addAchievementButton.onClick.AddListener(ShowAddAchievement);
            historyButton.onClick.AddListener(ShowHistory);
            shopButton.onClick.AddListener(ShowShop);
            inventoryButton.onClick.AddListener(ShowInventory);
        }

        private void Start()
        {
            ShowHome();
        }

        public void ShowHome() => ShowPanel(homePanel);
        public void ShowAddAchievement() => ShowPanel(addAchievementPanel);
        public void ShowHistory() => ShowPanel(historyPanel);
        public void ShowShop() => ShowPanel(shopPanel);
        public void ShowInventory() => ShowPanel(inventoryPanel);

        private void ShowPanel(GameObject target)
        {
            foreach (var panel in _panels)
            {
                panel.SetActive(panel == target);
            }
        }
        
        public void ResetProgress()
        {
            // Reset de opgeslagen voortgang.
            GameSession.Reset();

            // Herlaadt de scene zodat alle schermen worden bijgewerkt.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}