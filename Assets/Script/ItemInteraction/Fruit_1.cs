using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit_1 : MonoBehaviour,IInteractable
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
            inventory.text_fruit1.text = $"{_prompt}";
            inventory.img_fruit1.color = inventory.activeColor;
            isTaken = true;
            inventory.HasFruit1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyFruit1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_fruit1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyFruit1 = false;
            inventory.HasFruit1 = false;
            isTaken = false;
            inventory.text_fruit1.text = $"empty";
            inventory.img_fruit1.color = inventory.neutralColor;
        }
        return true;
    }
}
