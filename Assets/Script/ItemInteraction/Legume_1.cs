using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legume_1 : MonoBehaviour, IInteractable
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
            inventory.text_legume1.text = $"{_prompt}";
            inventory.img_legume1.color = inventory.activeColor;
            isTaken = true;
            inventory.HasLegume1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyLegume1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_legume1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyLegume1 = false;
            inventory.HasLegume1 = false;
            isTaken = false;
            inventory.text_legume1.text = $"empty";
            inventory.img_legume1.color = inventory.neutralColor;
        }
        return true;
    }
}
