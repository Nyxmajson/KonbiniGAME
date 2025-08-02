using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;
    public Inventory inventory;
    public string itemName;

    public bool Interact(Interactor interactor)
    {
        var item = inventory.GetItemByName(itemName);
        if (item == null)
        {
            Debug.LogWarning($"Item '{itemName}' non trouvé dans Inventory.");
            return false;
        }

        // On regarde directement si l'item est déjà pris (hasItem) au lieu de isTaken local
        if (!item.hasItem)
        {
            if (inventory.collectedItems.Count >= 8)
            {
                Debug.Log("Inventaire plein !");
                return false;
            }

            item.hasItem = true;
            if (!inventory.collectedItems.Contains(item))
                inventory.collectedItems.Add(item);

            inventory.UpdateInventoryDisplay();

            Debug.Log($"{_prompt} ajouté à l'inventaire.");
        }
        else
        {
            // Retirer de l’inventaire
            item.hasItem = false;
            if (inventory.collectedItems.Contains(item))
                inventory.collectedItems.Remove(item);

            inventory.UpdateInventoryDisplay();

            Debug.Log($"{_prompt} retiré de l'inventaire.");
        }

        return true;
    }
}
