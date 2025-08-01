using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack_2 : MonoBehaviour, IInteractable
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
            inventory.text_snack2.text = $"{_prompt}";
            inventory.img_snack2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasSnack2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalySnack2 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_snack2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalySnack2 = false;
            inventory.HasSnack2 = false;
            isTaken = false;
            inventory.text_snack2.text = $"empty";
            inventory.img_snack2.color = inventory.neutralColor;
        }
        return true;
    }
}
