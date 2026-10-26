
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Managers
{
    public class ShopManager : MonoBehaviour
    {
        private GameData _data;

        private readonly List<ShopItemData> _items = new()
        {
            new ShopItemData
            {
                id = "item_001",
                itemName = "Ster",
                description = "Een ster voor je collectie.",
                price = 20
            },
            new ShopItemData
            {
                id = "item_002",
                itemName = "Diamant",
                description = "Een bijzondere diamant.",
                price = 30
            },
            new ShopItemData
            {
                id = "item_003",
                itemName = "Trofee",
                description = "Een trofee voor je collectie.",
                price = 50
            }
        };

        public enum PurchaseResult
        {
            Success,
            ItemAlreadyOwned,
            NotEnoughCoins,
            InvalidItem
        }

        public IReadOnlyList<ShopItemData> Items => _items;
        public int Coins => _data.coins;
        public List<string> InventoryItemIds => _data.inventoryItemIds;

        public event System.Action<int> CoinsChanged;

        private void Awake()
        {
            _data = SaveManager.Load();
        }

        public PurchaseResult TryPurchase(string itemId)
        {
            ShopItemData item = _items.FirstOrDefault(
                item => item.id == itemId);

            if (item == null || item.price < 0)
            {
                return PurchaseResult.InvalidItem;
            }

            if (_data.inventoryItemIds.Contains(itemId))
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

            return PurchaseResult.Success;
        }

        public bool IsOwned(string itemId)
        {
            return _data.inventoryItemIds.Contains(itemId);
        }
    }
}
