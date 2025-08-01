using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Paiement : MonoBehaviour
{
    [Header("References")]
    public Inventory inventary;
    [SerializeField] private PlayerCamera playerCamera;

    [Header("UX Element")]
    public Image Caisse1Feedback;
    public Image Caisse2Feedback;
    public Color GoodItemColor;
    public Color BadItemColor;

    private int totalItemsToProcess = 0;
    private int finishedItems = 0;
    private Vendeur1 vendeur;

    public void SetVendeurReference(Vendeur1 v)
    {
        vendeur = v;
    }

    public IEnumerator ActivateItemsWithDelay(float delay)
    {
        playerCamera.SwitchCameraStyle(PlayerCamera.CameraStyle.Camera3);

        totalItemsToProcess = 0;
        finishedItems = 0;

        foreach (var item in inventary.collectedItems)
        {
            if (item.hasItem && item.visualObject != null)
            {
                totalItemsToProcess++;

                item.visualObject.SetActive(true);

                if (item.objetPath != null)
                {
                    item.objetPath.Init(item, this);
                    item.objetPath.Startpatrol = true;
                }

                yield return new WaitForSeconds(delay);
            }
        }

        // Si aucun item n’est actif, on évalue directement (sécurité)
        if (totalItemsToProcess == 0)
        {
            vendeur?.EvaluerInventaire();
        }
    }

    public void DisplayItemFeedback(ItemData item)
    {
        if (item == null) return;

        if (item.isAnomaly)
        {
            Caisse1Feedback.color = BadItemColor;
            Caisse2Feedback.color = BadItemColor;
        }
        else
        {
            Caisse1Feedback.color = GoodItemColor;
            Caisse2Feedback.color = GoodItemColor;
        }
    }

    public void NotifyItemFinished()
    {
        finishedItems++;

        if (finishedItems >= totalItemsToProcess)
        {
            vendeur.TousLesObjetsSontArrivés();
        }
    }
}
