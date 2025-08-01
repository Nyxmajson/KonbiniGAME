using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legume_2 : MonoBehaviour, IInteractable
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
            inventory.text_legume2.text = $"{_prompt}";
            inventory.img_legume2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasLegume2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyLegume1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_legume2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyLegume1 = false;
            inventory.HasLegume2 = false;
            isTaken = false;
            inventory.text_legume2.text = $"empty";
            inventory.img_legume2.color = inventory.neutralColor;
        }
        return true;
    }
}
