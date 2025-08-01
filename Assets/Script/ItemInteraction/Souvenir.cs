using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Souvenir : MonoBehaviour, IInteractable
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
            inventory.text_souvenir.text = $"{_prompt}";
            inventory.img_souvenir.color = inventory.activeColor;
            isTaken = true;
            inventory.HasSouvenir = true;

            if (isAnomaly)
            {
                inventory.isAnomalySouvenir = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_souvenir.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalySouvenir = false;
            inventory.HasSouvenir = false;
            isTaken = false;
            inventory.text_souvenir.text = $"empty";
            inventory.img_souvenir.color = inventory.neutralColor;
        }
        return true;
    }
}
