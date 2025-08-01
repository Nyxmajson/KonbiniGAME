using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink_2 : MonoBehaviour, IInteractable
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
            inventory.text_drink2.text = $"{_prompt}";
            inventory.img_drink2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasDrink2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyDrink2 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_drink2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyDrink2 = false;
            inventory.HasDrink2 = false;
            isTaken = false;
            inventory.text_drink2.text = $"empty";
            inventory.img_drink2.color = inventory.neutralColor;
        }
        return true;
    }
}
