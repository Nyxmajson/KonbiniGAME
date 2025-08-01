using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink_1 : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;
    public Inventory inventory;

    [Header("Design")]
    public bool isAnomaly;
    public bool isTaken;

    public bool Interact(Interactor interactor)
    {
        if (isAnomaly) Debug.Log($"{_prompt} is Anomaly");

        if (!isTaken)
        {
            inventory.text_drink1.text = $"{_prompt}";
            inventory.img_drink1.color = inventory.activeColor;
            isTaken = true;
            inventory.HasDrink1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyDrink1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_drink1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyDrink1 = false;
            inventory.HasDrink1 = false;
            isTaken = false;
            inventory.text_drink1.text = $"empty";
            inventory.img_drink1.color = inventory.neutralColor;
        }
        return true;
    }
}
