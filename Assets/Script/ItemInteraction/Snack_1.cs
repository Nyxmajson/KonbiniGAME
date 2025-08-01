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

    public bool Interact(Interactor interactor)
    {
        if (isAnomaly) Debug.Log($"{_prompt} is Anomaly");
        inventory.text_snack1.text = $"{_prompt}";

        return true;
    }

}
