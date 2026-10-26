using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Managers
{
    public class InventoryManager : MonoBehaviour
    {
        [SerializeField] private ShopManager shopManager;

        private List<ShopItemData> _ownedItems = new();

        // Alleen de items die de speler daadwerkelijk bezit.
        public IReadOnlyList<ShopItemData> OwnedItems => _ownedItems;

        public event Action InventoryChanged;

        private void OnEnable()
        {
            if (shopManager == null)
            {
                Debug.LogError("InventoryManager heeft geen ShopManager.");
                return;
            }

            shopManager.InventoryChanged += RefreshInventory;
            RefreshInventory();
        }

        private void OnDisable()
        {
            if (shopManager != null)
            {
                shopManager.InventoryChanged -= RefreshInventory;
            }
        }

        // Bouwt de inventory opnieuw op vanuit de gekochte item-ID's.
        public void RefreshInventory()
        {
            if (shopManager == null)
            {
                return;
            }

            _ownedItems = shopManager.Items
                .Where(item => shopManager.IsOwned(item.id))
                .ToList();

            InventoryChanged?.Invoke();
        }

        // Controleert of een item in de inventory zit.
        public bool HasItem(string itemId)
        {
            return _ownedItems.Any(item => item.id == itemId);
        }
    }
}