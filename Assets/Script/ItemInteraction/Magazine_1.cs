using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_1 : MonoBehaviour, IInteractable
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
            inventory.text_magazine1.text = $"{_prompt}";
            inventory.img_magazine1.color = inventory.activeColor;
            isTaken = true;
            inventory.HasMagazine1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyMagazine1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_magazine1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyMagazine1 = false;
            inventory.HasMagazine1 = false;
            isTaken = false;
            inventory.text_magazine1.text = $"empty";
            inventory.img_magazine1.color = inventory.neutralColor;
        }
        return true;
    }
}
