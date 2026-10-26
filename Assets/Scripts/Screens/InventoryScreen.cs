using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class InventoryScreen : MonoBehaviour
    {
        // Beheert de gekochte items.
        [SerializeField] private InventoryManager inventoryManager;

        // Content van de Scroll View.
        [SerializeField] private Transform content;

        // Prefab waarmee ieder inventory-item wordt getoond.
        [SerializeField] private GameObject itemPrefab;

        private void OnEnable()
        {
            inventoryManager.InventoryChanged += RefreshInventory;
            RefreshInventory();
        }

        private void OnDisable()
        {
            inventoryManager.InventoryChanged -= RefreshInventory;
        }

        // Bouwt de inventoryweergave opnieuw op.
        private void RefreshInventory()
        {
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            foreach (var item in inventoryManager.OwnedItems)
            {
                GameObject itemObject = Instantiate(itemPrefab, content);

                TMP_Text[] texts =
                    itemObject.GetComponentsInChildren<TMP_Text>();

                texts[0].text = item.itemName;
                texts[1].text = item.description;
                texts[2].text = $"{item.price} coins";
            }
        }
    }
}