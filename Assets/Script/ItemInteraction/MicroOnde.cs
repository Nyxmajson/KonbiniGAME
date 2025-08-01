using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroOnde : MonoBehaviour, IInteractable
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
            inventory.text_microonde.text = $"{_prompt}";
            inventory.img_microonde.color = inventory.activeColor;
            isTaken = true;
            inventory.HasMicroonde = true;

            if (isAnomaly)
            {
                inventory.isAnomalyMicroonde = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_microonde.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyMicroonde = false;
            inventory.HasMicroonde = false;
            isTaken = false;
            inventory.text_microonde.text = $"empty";
            inventory.img_microonde.color = inventory.neutralColor;
        }
        return true;
    }
}
