
using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour
    {
        // Wrapper die Unity gebruikt om de JSON-lijst in te lezen.
        [Serializable]
        private class ShopItemList
        {
            public List<ShopItemData> items;
        }

        private GameData _data;
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

        // Gedeelde gegevens die door SaveManager worden bewaard.
        public int Coins => _data.coins;
        public List<string> InventoryItemIds => _data.inventoryItemIds;

        public event Action<int> CoinsChanged;
        public event Action InventoryChanged;

        private void Awake()
        {
            _data = SaveManager.Load();

            // Zorg dat de inventorylijst beschikbaar is.
            if (_data.inventoryItemIds == null)
            {
                _data.inventoryItemIds = new List<string>();
            }

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

            if (_data.coins < item.price)
            {
                return PurchaseResult.NotEnoughCoins;
            }

            _data.coins -= item.price;
            _data.inventoryItemIds.Add(itemId);

            SaveManager.Save(_data);

            CoinsChanged?.Invoke(_data.coins);
            InventoryChanged?.Invoke();

            return PurchaseResult.Success;
        }

        // Controleert of de speler dit item al bezit.
        public bool IsOwned(string itemId)
        {
            return _data.inventoryItemIds.Contains(itemId);
        }
    }
}
