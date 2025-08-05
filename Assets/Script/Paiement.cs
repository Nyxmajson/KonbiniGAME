using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86;

public class Paiement : MonoBehaviour
{
    [Header("References")]
    public Inventory inventary;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private PlayerCamera playerCameraCinematic;
    public GameObject playerUI;
    [SerializeField] private PlayerMovementAdvanced PMA;
    [SerializeField] private PlayerCamera PC;

    [Header("Ecran Caisse UX")]
    public Image Caisse1Feedback;
   // public Image Caisse2Feedback;
    public Color GoodItemColor;
    public Color BadItemColor;
    public Color defaultItemColor;

    [Header("Paiement Logs")]
    [SerializeField] private int totalItemsToProcess = 0;
    [SerializeField] private int finishedItems = 0;
    [SerializeField] private Vendeur vendeur;

    public void SetVendeurReference(Vendeur v)
    {
        vendeur = v;
    }

    public IEnumerator ActivateItemsWithDelay(float delay)
    {
        playerCamera.SwitchCameraStyle(PlayerCamera.CameraStyle.Camera3);
        playerCameraCinematic.SwitchCameraStyle(PlayerCamera.CameraStyle.Camera3);
        playerUI.SetActive(false);

        totalItemsToProcess = 0;
        finishedItems = 0;

        List<ItemData> itemsCopy = new List<ItemData>(inventary.collectedItems);

        foreach (var item in itemsCopy)
        {
            if (item.hasItem && item.visualObject != null)
            {
                totalItemsToProcess++;

                item.visualObject.SetActive(true);

                // Clone du visuel de l'item
                GameObject clone = GameObject.Instantiate(item.visualObject, inventary.panierParent);

                // Nettoyage des composants existants (�viter de dupliquer des r�f�rences)
                if (clone.GetComponent<ObjetPath>() != null)
                    Destroy(clone.GetComponent<ObjetPath>());

                // Copie du script ObjetPath avec nouvelle instance
                ObjetPath pathScript = clone.AddComponent<ObjetPath>();

                // Copie les valeurs depuis l�original (tu peux en rajouter si besoin)
                ObjetPath original = item.visualObject.GetComponent<ObjetPath>();
                pathScript.patrolPoints = original.patrolPoints;
                pathScript.pointAnalyse = original.pointAnalyse;
                pathScript.speed = original.speed;
                pathScript.parentDuringPath = original.parentDuringPath;
                pathScript.dropZone = original.dropZone;

                // Init et lancement
                pathScript.Init(item, this);
                pathScript.Startpatrol = true;

                // L�original est marqu� comme plus en possession
                item.hasItem = false;

                yield return new WaitForSeconds(delay);
            }
        }

        // Si aucun item n�est actif, on �value directement (s�curit�)
        if (totalItemsToProcess == 0)
        {
            vendeur?.AnalysePaiement();
        }
    }

    public void DisplayItemFeedback(ItemData item)
    {
        if (item == null) return;

        if (item.isAnomaly)
        {
            Caisse1Feedback.color = BadItemColor;
            //Caisse2Feedback.color = BadItemColor;
        }
        else
        {
            Caisse1Feedback.color = GoodItemColor;
            //Caisse2Feedback.color = GoodItemColor;
        }
    }

    public void NotifyItemFinished()
    {
        finishedItems++;

        if (finishedItems >= totalItemsToProcess)
        {
            // On retire tous les items de l'inventaire une fois le processus termin�
            inventary.collectedItems.Clear();
            PMA.enabled = true;
            PC.enabled = true;

            Caisse1Feedback.color = defaultItemColor;
            //Caisse2Feedback.color = defaultItemColor;

            playerCamera.SwitchCameraStyle(PlayerCamera.CameraStyle.Basic);
            playerCameraCinematic.SwitchCameraStyle(PlayerCamera.CameraStyle.Basic);
            playerUI.SetActive(true);

            // Met � jour l'affichage
            inventary.UpdateInventoryDisplay();

            vendeur.TousLesObjetsSontArriv�s();
        }
        if (!inventary.HasAnyItem())
        {
            Debug.Log("Inventaire vid� apr�s le paiement.");
        }
    }
}
