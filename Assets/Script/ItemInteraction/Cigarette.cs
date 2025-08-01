using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cigarette : MonoBehaviour, IInteractable
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
            inventory.text_cigarette.text = $"{_prompt}";
            inventory.img_cigarette.color = inventory.activeColor;
            isTaken = true;
            inventory.HasCigarette = true;

            if (isAnomaly)
            {
                inventory.isAnomalyCigarette = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_cigarette.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyCigarette = false;
            inventory.HasCigarette = false;
            isTaken = false;
            inventory.text_cigarette.text = $"empty";
            inventory.img_cigarette.color = inventory.neutralColor;
        }

        return true;
    }
}
