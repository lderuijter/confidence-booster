
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    [RequireComponent(typeof(UIDocument))]
    public class InventoryScreen : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;

        private UIDocument _document;
        private ScrollView _content;
        private Label _emptyInventoryText;

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();

            VisualElement root = _document.rootVisualElement;

            _content = root.Q<ScrollView>("inventory-items");
            _emptyInventoryText = root.Q<Label>("inventory-empty");

            if (inventoryManager == null)
            {
                Debug.LogError(
                    "InventoryScreen heeft geen InventoryManager.",
                    this);
                return;
            }

            if (_content == null || _emptyInventoryText == null)
            {
                Debug.LogError(
                    "InventoryScreen mist UI-elementen in de UXML.",
                    this);
                return;
            }

            ConfigureGrid(_content);

            inventoryManager.InventoryChanged += RefreshInventory;
            RefreshInventory();
        }

        private void OnDisable()
        {
            if (inventoryManager != null)
            {
                inventoryManager.InventoryChanged -= RefreshInventory;
            }
        }

        private void ConfigureGrid(ScrollView scrollView)
        {
            scrollView.contentContainer.style.flexDirection =
                FlexDirection.Row;

            scrollView.contentContainer.style.flexWrap = Wrap.Wrap;

            scrollView.contentContainer.style.alignContent =
                Align.FlexStart;
        }

        private void RefreshInventory()
        {
            if (_content == null || inventoryManager == null ||
                _emptyInventoryText == null)
            {
                return;
            }

            _content.Clear();

            var items = inventoryManager.OwnedItems;
            bool isEmpty = items.Count == 0;

            _content.style.display =
                isEmpty ? DisplayStyle.None : DisplayStyle.Flex;

            _emptyInventoryText.style.display =
                isEmpty ? DisplayStyle.Flex : DisplayStyle.None;

            foreach (var item in items)
            {
                var card = new VisualElement();
                card.AddToClassList("item-card");

                var nameLabel = new Label(item.itemName);
                nameLabel.AddToClassList("item-title");

                var descriptionLabel = new Label(item.description);
                descriptionLabel.AddToClassList("item-description");

                var priceLabel = new Label($"{item.price} coins");
                priceLabel.AddToClassList("item-price");

                card.Add(nameLabel);
                card.Add(descriptionLabel);
                card.Add(priceLabel);

                _content.Add(card);
            }
        }
    }
}
