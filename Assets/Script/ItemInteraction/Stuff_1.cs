using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff_1 : MonoBehaviour, IInteractable
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
            inventory.text_stuff1.text = $"{_prompt}";
            inventory.img_stuff1.color = inventory.activeColor;
            isTaken = true;
            inventory.HasStuff1 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyStuff1 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_stuff1.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyStuff1 = false;
            inventory.HasStuff1 = false;
            isTaken = false;
            inventory.text_stuff1.text = $"empty";
            inventory.img_stuff1.color = inventory.neutralColor;
        }
        return true;
    }

}
