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

    public bool Interact(Interactor interactor)
    {
        if (isAnomaly) Debug.Log($"{_prompt} is Anomaly");
        inventory.text_stuff2.text = $"{_prompt}";

        return true;
    }

}
