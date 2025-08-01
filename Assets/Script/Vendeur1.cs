using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendeur1 : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;

    public Inventory inventary;
    [SerializeField] private Paiement paiement; 
    [SerializeField] private GameDesign gameDesign;

    public int CompteurAnomaly;
    public int CompteurItem;

    public void EvaluerInventaire()
    {
        CompteurItem = 0;
        CompteurAnomaly = 0;

        foreach (var item in paiement.inventary.collectedItems)
        {
            if (item.hasItem) CompteurItem++;
            if (item.isAnomaly) CompteurAnomaly++;
        }

        if (CompteurAnomaly == 0)
            Debug.Log("Aucun item anormal");
        else if (CompteurAnomaly == 1)
            Debug.Log("1 anomalie détectée.");
        else
            Debug.Log("Plusieurs anomalies !");
    }

    public bool Interact(Interactor interactor)
    {
        paiement.SetVendeurReference(this);
        StartCoroutine(paiement.ActivateItemsWithDelay(1.5f));
        return true;
    }
    public void TousLesObjetsSontArrivés()
    {
        EvaluerInventaire();
        gameDesign.CheckCollectedItems();
    }
}
