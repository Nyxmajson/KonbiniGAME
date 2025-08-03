using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vendeur : MonoBehaviour, IInteractable
{
    public string _prompt;
    public string InteractionPrompt => _prompt;

    [SerializeField] private Inventory inventory;
    [SerializeField] private Paiement paiement; 
    [SerializeField] private GameDesign gameDesign;
    [SerializeField] private doorAutomatic doorAutomatic;
    [SerializeField] private PlayerMovementAdvanced PMA;
    [SerializeField] private PlayerCamera PC;

    public int CompteurAnomaly;
    public int CompteurItem;
    public int CompteurEasterEgg;

    public bool Stade1 = false;
    public Color ColorStade1;
    public bool Stade2 = false;
    public Color ColorStade2;
    public bool Stade3 = false;
    public Color ColorStade3;


    public bool Interact(Interactor interactor)
    {
        if (!Stade1 && !Stade2 && !Stade3 && inventory.HasAnyItem())
        {
            paiement.SetVendeurReference(this);
            Debug.Log("Paiement lancé !");
            StartCoroutine(paiement.ActivateItemsWithDelay(1.5f));
            PMA.enabled = false;
            PC.enabled = false;
        }
        else
        {
            CompteurEasterEgg++;
            Debug.Log("Vous n'avez rien à vendre.");
        }
        if (Stade1) _prompt = "";
        if (Stade2) _prompt = "";
        if (Stade3) _prompt = "";

        if (CompteurEasterEgg == 8) Debug.Log("EASTER EGGS");

        return true;
    }

    public void TousLesObjetsSontArrivés()
    {
        AnalysePaiement();
    }

    public void AnalysePaiement()
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
            paiement.Caisse1Feedback.color = ColorStade1;
            paiement.Caisse2Feedback.color = ColorStade1;
            doorAutomatic.mustOpen = true;
            doorAutomatic.OpenTheDoor();
            gameDesign.EntryLight.color = ColorStade1;
            Debug.Log("Aucun item anormal");
        }
        else if (CompteurAnomaly == 1)
        {
            paiement.Caisse1Feedback.color = ColorStade2;
            paiement.Caisse2Feedback.color = ColorStade2;
            doorAutomatic.mustClose = true;
            doorAutomatic.CloseTheDoor();
            gameDesign.EntryLight.color = ColorStade2;
            Debug.Log("1 anomalie détectée.");
        }
        else
        {
            paiement.Caisse1Feedback.color = ColorStade3;
            paiement.Caisse2Feedback.color = ColorStade3;
            doorAutomatic.mustClose = true;
            doorAutomatic.CloseTheDoor();
            gameDesign.EntryLight.color = ColorStade3;
            Debug.Log("Plusieurs anomalies !");
        }
            
    }
}
