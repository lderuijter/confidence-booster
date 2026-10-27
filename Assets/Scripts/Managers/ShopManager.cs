using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Managers
{
    // Draait eerder dan andere scripts, zodat de items al geladen zijn als de inventory ze opvraagt
    [DefaultExecutionOrder(-100)]
    public class ShopManager : MonoBehaviour
    {
        // Wrapper die Unity gebruikt om de JSON-lijst in te lezen.
        [Serializable]
        private class ShopItemList
        {
            public List<ShopItemData> items;
        }

        private List<ShopItemData> _items = new();

        public enum PurchaseResult
        {
            Success,
            ItemAlreadyOwned,
            NotEnoughCoins,
            InvalidItem
        }

        // De items die uit het JSON-bestand zijn geladen.
        public IReadOnlyList<ShopItemData> Items => _items;

        // Gedeelde gegevens die door GameSession worden bewaard.
        public int Coins => GameSession.Current.coins;
        public List<string> InventoryItemIds => GameSession.Current.inventoryItemIds;

        public event Action<int> CoinsChanged;
        public event Action InventoryChanged;

        private void Awake()
        {
            LoadShopItems();
        }

        // Laadt de shop-items uit Assets/Resources/shop_items.json.
        private void LoadShopItems()
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("shop_items");

            if (jsonFile == null)
            {
                Debug.LogError(
                    "shop_items.json niet gevonden in Assets/Resources.");
                return;
            }

            ShopItemList shopItemList =
                JsonUtility.FromJson<ShopItemList>(jsonFile.text);

            if (shopItemList?.items == null)
            {
                Debug.LogError("De shop-items JSON heeft geen geldige items-lijst.");
                return;
            }

            // Voorkom ongeldige items en dubbele IDs.
            _items = shopItemList.items
                .Where(item => item != null
                    && !string.IsNullOrWhiteSpace(item.id)
                    && item.price >= 0)
                .GroupBy(item => item.id)
                .Select(group => group.First())
                .ToList();

            Debug.Log($"Shop geladen: {_items.Count} items.");
        }

        // Probeert een item te kopen en slaat de wijziging op.
        public PurchaseResult TryPurchase(string itemId)
        {
            ShopItemData item = _items.FirstOrDefault(
                shopItem => shopItem.id == itemId);

            if (item == null || item.price < 0)
            {
                return PurchaseResult.InvalidItem;
            }

            if (IsOwned(itemId))
            {
                return PurchaseResult.ItemAlreadyOwned;
            }

            GameData data = GameSession.Current;

            if (data.coins < item.price)
            {
                return PurchaseResult.NotEnoughCoins;
            }

            data.coins -= item.price;
            data.inventoryItemIds.Add(itemId);

            GameSession.Save();

            CoinsChanged?.Invoke(data.coins);
            InventoryChanged?.Invoke();

            return PurchaseResult.Success;
        }

        // Controleert of de speler dit item al bezit.
        public bool IsOwned(string itemId)
        {
            return GameSession.Current.inventoryItemIds.Contains(itemId);
        }
    }
}