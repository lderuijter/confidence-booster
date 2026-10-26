using Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Screens
{
    public class ShopScreen : MonoBehaviour
    {
        // De manager die de shop-items en aankopen beheert.
        [SerializeField] private ShopManager shopManager;

        // Het object in de Scroll View waar de shop-items in komen.
        [SerializeField] private Transform content;

        // Het prefab dat we kopiëren voor ieder shop-item.
        [SerializeField] private GameObject itemPrefab;

        // Unity roept dit aan wanneer het shop-panel zichtbaar wordt.
        private void OnEnable()
        {
            // Verwijder oude items om dubbele items te voorkomen.
            foreach (Transform child in content)
            {
                Destroy(child.gameObject);
            }

            // Loop door alle beschikbare shop-items.
            foreach (var shopItem in shopManager.Items)
            {
                // Maak een kopie van het prefab in de Scroll View.
                GameObject itemObject = Instantiate(itemPrefab, content);

                // Zoek de teksten en knop in het aangemaakte item.
                TMP_Text[] texts = itemObject.GetComponentsInChildren<TMP_Text>();
                Button button = itemObject.GetComponentInChildren<Button>();

                // Vul de naam, beschrijving en prijs in.
                texts[0].text = shopItem.itemName;
                texts[1].text = shopItem.description;
                texts[2].text = $"{shopItem.price} coins";

                // Controleer of het item al gekocht is.
                bool isOwned = shopManager.IsOwned(shopItem.id);

                // Pas de knop aan als het item al gekocht is.
                button.interactable = !isOwned;

                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = isOwned ? "Gekocht" : "Kopen";

                // Koppel de aankoop aan de knop.
                string itemId = shopItem.id;
                button.onClick.AddListener(() => PurchaseItem(itemId));
            }
        }

        // Probeert het geselecteerde item te kopen.
        private void PurchaseItem(string itemId)
        {
            ShopManager.PurchaseResult result = shopManager.TryPurchase(itemId);

            switch (result)
            {
                case ShopManager.PurchaseResult.Success:
                    // Vernieuw de shop zodat het item als gekocht wordt getoond.
                    RefreshShop();
                    break;

                case ShopManager.PurchaseResult.NotEnoughCoins:
                    Debug.Log("Je hebt niet genoeg coins.");
                    break;

                case ShopManager.PurchaseResult.ItemAlreadyOwned:
                    Debug.Log("Je hebt dit item al gekocht.");
                    break;

                case ShopManager.PurchaseResult.InvalidItem:
                    Debug.LogWarning("Dit shop-item bestaat niet.");
                    break;
            }
        }

        // Bouwt de lijst opnieuw op met de actuele gegevens.
        private void RefreshShop()
        {
            OnEnable();
        }
    }
}
