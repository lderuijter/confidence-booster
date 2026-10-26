
using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class ShopScreen : MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private TMP_Text coinsText;
        [SerializeField] private TMP_Text messageText;

        private void OnEnable()
        {
            if (shopManager == null)
            {
                Debug.LogError("ShopScreen heeft geen ShopManager.");
                return;
            }

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

        private void UpdateCoins(int coins)
        {
            if (coinsText != null)
            {
                coinsText.text = $"Coins: {coins}";
            }
        }

        private void RefreshShop()
        {
            if (content == null || itemPrefab == null)
            {
                Debug.LogError("ShopScreen mist Content of Item Prefab.");
                return;
            }

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in shopManager.Items)
            {
                GameObject itemObject = Instantiate(itemPrefab, content);
                Transform itemTransform = itemObject.transform;

                TMP_Text nameText =
                    itemTransform.Find("ItemNameText")?.GetComponent<TMP_Text>();

                TMP_Text descriptionText =
                    itemTransform.Find("DescriptionText")?.GetComponent<TMP_Text>();

                TMP_Text priceText =
                    itemTransform.Find("PriceText")?.GetComponent<TMP_Text>();

                Button button =
                    itemTransform.Find("BuyButton")?.GetComponent<Button>();

                if (nameText == null || descriptionText == null ||
                    priceText == null || button == null)
                {
                    Debug.LogError(
                        "ShopItemPrefab mist een verwacht tekstveld of BuyButton.",
                        itemObject);

                    continue;
                }

                nameText.text = item.itemName;
                descriptionText.text = item.description;
                priceText.text = $"{item.price} coins";

                bool isOwned = shopManager.IsOwned(item.id);
                button.interactable = !isOwned;

                TMP_Text buttonText =
                    button.GetComponentInChildren<TMP_Text>();

                if (buttonText != null)
                {
                    buttonText.text = isOwned ? "Gekocht" : "Kopen";
                }

                string itemId = item.id;
                button.onClick.AddListener(() => PurchaseItem(itemId));
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
            if (messageText != null)
            {
                messageText.text = message;
            }
        }
    }
}
