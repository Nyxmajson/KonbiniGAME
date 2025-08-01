using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drink_4 : MonoBehaviour, IInteractable
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
            inventory.text_drink4.text = $"{_prompt}";
            inventory.img_drink4.color = inventory.activeColor;
            isTaken = true;
            inventory.HasDrink4 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyDrink4 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_drink4.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyDrink4 = false;
            inventory.HasDrink4 = false;
            isTaken = false;
            inventory.text_drink4.text = $"empty";
            inventory.img_drink4.color = inventory.neutralColor;
        }
        return true;
    }
}
