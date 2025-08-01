using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack_1 : MonoBehaviour, IInteractable
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
            inventory.text_snack1.text = $"{_prompt}";
            inventory.img_snack1.color = inventory.activeColor;
            isTaken = true; 
            inventory.HasSnack1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyMicroonde = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_snack1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyMicroonde = false;
            inventory.HasSnack1 = false;
            inventory.isAnomalySnack1 = false;
            isTaken = false;
            inventory.text_snack1.text = $"empty";
            inventory.img_snack1.color = inventory.neutralColor;
        }
        return true;
    }

}
