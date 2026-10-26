
using Managers;
using UnityEngine;
using UnityEngine.UIElements;

namespace Screens
{
    [RequireComponent(typeof(UIDocument))]
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;

        private UIDocument _document;
        private Label _coinsText;
        private Label _messageText;
        private ScrollView _content;

        private void OnEnable()
        {
            _document = GetComponent<UIDocument>();

            VisualElement root = _document.rootVisualElement;

            _coinsText = root.Q<Label>("coin-balance");
            _messageText = root.Q<Label>("shop-feedback");
            _content = root.Q<ScrollView>("shop-items");

            if (shopManager == null)
            {
                Debug.LogError("ShopScreen heeft geen ShopManager.", this);
                return;
            }

            if (_coinsText == null || _messageText == null ||
                _content == null)
            {
                Debug.LogError(
                    "ShopScreen mist UI-elementen in de UXML.",
                    this);
                return;
            }

            ConfigureGrid(_content);

            shopManager.CoinsChanged += UpdateCoins;

            UpdateCoins(shopManager.Coins);
            RefreshShop();
        }

        private void OnDisable()
        {
            if (shopManager != null)
            {
                shopManager.CoinsChanged -= UpdateCoins;
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

        private void UpdateCoins(int coins)
        {
            if (_coinsText != null)
            {
                _coinsText.text = $"Coins: {coins}";
            }
        }

        private void RefreshShop()
        {
            if (_content == null || shopManager == null)
            {
                return;
            }

            _content.Clear();

            foreach (var item in shopManager.Items)
            {
                string itemId = item.id;
                bool isOwned = shopManager.IsOwned(itemId);

                var card = new VisualElement();
                card.AddToClassList("item-card");

                var nameLabel = new Label(item.itemName);
                nameLabel.AddToClassList("item-title");

                var descriptionLabel = new Label(item.description);
                descriptionLabel.AddToClassList("item-description");

                var priceLabel = new Label($"{item.price} coins");
                priceLabel.AddToClassList("item-price");

                var buyButton = new Button();
                buyButton.AddToClassList("item-button");
                buyButton.text = isOwned ? "Gekocht" : "Kopen";
                buyButton.SetEnabled(!isOwned);
                buyButton.clicked += () => PurchaseItem(itemId);

                card.Add(nameLabel);
                card.Add(descriptionLabel);
                card.Add(priceLabel);
                card.Add(buyButton);

                _content.Add(card);
            }
        }

        private void PurchaseItem(string itemId)
        {
            ShopManager.PurchaseResult result =
                shopManager.TryPurchase(itemId);

            switch (result)
            {
                case ShopManager.PurchaseResult.Success:
                    ShowMessage("Aankoop gelukt!");
                    RefreshShop();
                    break;

                case ShopManager.PurchaseResult.NotEnoughCoins:
                    ShowMessage("Je hebt niet genoeg coins.");
                    break;

                case ShopManager.PurchaseResult.ItemAlreadyOwned:
                    ShowMessage("Je hebt dit item al gekocht.");
                    RefreshShop();
                    break;

                case ShopManager.PurchaseResult.InvalidItem:
                    ShowMessage("Dit shop-item bestaat niet.");
                    break;
            }
        }

        private void ShowMessage(string message)
        {
            if (_messageText != null)
            {
                _messageText.text = message;
            }
        }
    }
}
