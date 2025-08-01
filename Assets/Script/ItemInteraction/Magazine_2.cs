using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_2 : MonoBehaviour, IInteractable
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
            inventory.text_magazine2.text = $"{_prompt}";
            inventory.img_magazine2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasMagazine2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyMagazine2 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_magazine2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyMagazine2 = false;
            inventory.HasMagazine2 = false;
            isTaken = false;
            inventory.text_magazine2.text = $"empty";
            inventory.img_magazine2.color = inventory.neutralColor;
        }
        return true;
    }
}
