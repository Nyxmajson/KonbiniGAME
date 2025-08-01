using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine_3 : MonoBehaviour, IInteractable
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
            inventory.text_magazine3.text = $"{_prompt}";
            inventory.img_magazine3.color = inventory.activeColor;
            isTaken = true;
            inventory.HasMagazine3 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyMagazine3 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_magazine3.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyMagazine3 = false;
            inventory.HasMagazine3 = false;
            isTaken = false;
            inventory.text_magazine3.text = $"empty";
            inventory.img_magazine3.color = inventory.neutralColor;
        }
        return true;
    }
}
