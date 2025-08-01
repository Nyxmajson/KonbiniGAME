using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_2 : MonoBehaviour, IInteractable
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
            inventory.text_fruit2.text = $"{_prompt}";
            inventory.img_fruit2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasFruit2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyFruit2 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_fruit2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyFruit2 = false;
            inventory.HasFruit2 = false;
            isTaken = false;
            inventory.text_fruit2.text = $"empty";
            inventory.img_fruit2.color = inventory.neutralColor;
        }
        return true;
    }
}
