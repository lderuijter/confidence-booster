using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject homePanel;
        [SerializeField] private GameObject addAchievementPanel;
        [SerializeField] private GameObject historyPanel;
        [SerializeField] private GameObject shopPanel;
        [SerializeField] private GameObject inventoryPanel;

        private GameObject[] _panels;

        private void Awake()
        {
            _panels = new[] { homePanel, addAchievementPanel, historyPanel, shopPanel, inventoryPanel };
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
    }
}