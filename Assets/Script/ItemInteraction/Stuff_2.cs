using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stuff_2 : MonoBehaviour, IInteractable
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
            inventory.text_stuff2.text = $"{_prompt}";
            inventory.img_stuff2.color = inventory.activeColor;
            isTaken = true;
            inventory.HasStuff2 = true;

            if (isAnomaly)
            {
                inventory.isAnomalyStuff2 = true;
                Debug.Log($"{_prompt} is Anomaly");
                inventory.img_stuff2.color = inventory.anomalyColor;
            }
        }
        else
        {
            inventory.isAnomalyStuff2 = false;
            inventory.HasStuff2 = false;
            isTaken = false;
            inventory.text_stuff2.text = $"empty";
            inventory.img_stuff2.color = inventory.neutralColor;
        }
        return true;
    }

}
