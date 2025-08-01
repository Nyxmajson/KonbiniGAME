using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;
    public Inventory inventory;
    public string itemName; // Nom exact utilisé dans Inventory
    public bool isAnomaly;

    private bool isTaken = false;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>(); // Tu peux aussi l’injecter si besoin
    }

    public bool Interact(Interactor interactor)
    {
        var item = inventory.GetItemByName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"Item '{itemName}' non trouvé dans Inventory.");
            return false;
        }

        if (!isTaken)
        {
            if (inventory.collectedItems.Count >= 8)
            {
                Debug.Log("Inventaire plein !");
                return false;
            }

            item.hasItem = true;
            item.isAnomaly = isAnomaly;
            isTaken = true;

            // Ajout à l'inventaire du joueur
            inventory.collectedItems.Add(item);

            // Mise à jour UI visible du joueur
            inventory.UpdateInventoryDisplay();

            Debug.Log($"{_prompt} ajouté à l'inventaire.");
        }
        else
        {
            // Retirer de l’inventaire
            item.hasItem = false;
            item.isAnomaly = false;
            isTaken = false;

            inventory.collectedItems.Remove(item);

            // Mettre à jour l’UI
            inventory.UpdateInventoryDisplay();

            Debug.Log($"{_prompt} retiré de l'inventaire.");
        }

        return true;
    }
}
