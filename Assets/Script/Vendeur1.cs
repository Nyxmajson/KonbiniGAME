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

    public bool Stade1 = false;
    public bool Stade2 = false;
    public bool Stade3 = false;


    public bool Interact(Interactor interactor)
    {
        if (!Stade1 && !Stade2 && !Stade3)
        {
            paiement.SetVendeurReference(this);
            StartCoroutine(paiement.ActivateItemsWithDelay(1.5f));
            return true;
        }
        else _prompt = "";
            return true;
    }

    public void TousLesObjetsSontArrivés()
    {
        EvaluerInventaire();
    }
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
        {
            Debug.Log("Aucun item anormal");
        }
        else if (CompteurAnomaly == 1)
        {
            Debug.Log("1 anomalie détectée.");
        }
        else
        {
            Debug.Log("Plusieurs anomalies !");
        }
            
    }
}
