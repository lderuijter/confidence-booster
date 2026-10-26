
using Managers;
using TMPro;
using UnityEngine;

namespace Screens
{
    public class InventoryScreen : MonoBehaviour
    {
        [SerializeField] private InventoryManager inventoryManager;
        [SerializeField] private Transform content;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject emptyInventoryText;

        private void OnEnable()
        {
            if (inventoryManager == null)
            {
                Debug.LogError("InventoryScreen heeft geen InventoryManager.");
                return;
            }

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

        private void RefreshInventory()
        {
            if (content == null || itemPrefab == null)
            {
                Debug.LogError(
                    "InventoryScreen mist Content of Item Prefab.");
                return;
            }

            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            var items = inventoryManager.OwnedItems;

            if (emptyInventoryText != null)
            {
                emptyInventoryText.SetActive(items.Count == 0);
            }

            foreach (var item in items)
            {
                GameObject itemObject = Instantiate(itemPrefab, content);
                Transform itemTransform = itemObject.transform;

                TMP_Text nameText =
                    itemTransform.Find("ItemNameText")?.GetComponent<TMP_Text>();

                TMP_Text descriptionText =
                    itemTransform.Find("DescriptionText")?.GetComponent<TMP_Text>();

                TMP_Text priceText =
                    itemTransform.Find("PriceText")?.GetComponent<TMP_Text>();

                if (nameText == null || descriptionText == null ||
                    priceText == null)
                {
                    Debug.LogError(
                        "InventoryItemPrefab mist een verwacht tekstveld.",
                        itemObject);

                    continue;
                }

                nameText.text = item.itemName;
                descriptionText.text = item.description;
                priceText.text = $"{item.price} coins";
            }
        }
    }
}
