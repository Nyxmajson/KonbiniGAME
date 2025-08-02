using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class StatuePurification : MonoBehaviour, IInteractable
{
    [Header("Purification Settings")]
    public int maxPurificationStack = 4;
    private int currentStack;

    public GameObject[] purificationStackIndicators; // Un GameObject par stack

    [Header("Références")]
    public Inventory inventory;
    public GameObject confirmationPanel; // UI à afficher pour confirmer la purification
    //public GameObject selectionCursor; // Pour savoir quel item est sélectionné
    public GameObject bagFirstButton; // À cibler par l’EventSystem
    public EventSystem eventSystem;
    public bool SubmitPressed;
    public bool SubmitPurificationPressed;
    public bool CancelPressed;
    public bool isPurifying = false;
    public GameObject playerUI;

    [Header("Game State")]
    public bool isDanger = false;
    public bool isTranquille = true;

    private ItemData selectedItem;

    private PlayerControls controls;

    private void Awake()
    {
        currentStack = maxPurificationStack;
        UpdatePurificationStackUI();

        controls = new PlayerControls();

        controls.UI.SubmitPurification.performed += ctx => SubmitPurificationPressed = true;
        controls.UI.SubmitPurification.canceled += ctx => SubmitPurificationPressed = false;

        controls.UI.Submit.performed += ctx => SubmitPressed = true;
        controls.UI.Submit.canceled += ctx => SubmitPressed = false;

        controls.UI.Cancel.performed += ctx => CancelPressed = true;
        controls.UI.Cancel.canceled += ctx => CancelPressed = false;
    }

    private void OnEnable()
    {
        controls.UI.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    public string _prompt;
    public string InteractionPrompt => _prompt;

    public bool Interact(Interactor interactor)
    {
        if (isDanger)
        {
            if (currentStack > 0)
            {
                Debug.Log("Statue calme le danger...");
                currentStack--;
                UpdatePurificationStackUI();
                // Ici mettre le code pour calmer le Stade 2 ou 3 du MOOOONSTRE
            }
            else
            {
                Debug.Log("Vous n'avez plus de purification.");
            }

            return true;
        }

        if (currentStack <= 0)
        {
            Debug.Log("Aucune purification restante.");
            return true;
        }

        if (!isPurifying)
        {
            OpenPurificationInventory();
            Debug.Log("Mode Tranquille : Choisissez un objet à purifier.");
        }

        return true;
    }

    public void SelectItemToPurify(ItemData item)
    {
        if (!item.isAnomaly)
        {
            Debug.Log("Cet objet n’est pas une anomalie.");
            return;
        }

        selectedItem = item;
        confirmationPanel.SetActive(true);
        // Ici on peut déclencher une animation, une voix, etc.
    }

    public void OpenPurificationInventory()
    {
            isPurifying = true;
            inventory.Bag.SetActive(true);
            inventory.BagStatue.SetActive(true);
            inventory.isOpenBag = true;
            inventory.PMA.enabled = false;
            inventory.PC.enabled = false;
            playerUI.SetActive(false);

            // Désactiver Gameplay et activer UI
            inventory.controls.Gameplay.Disable();
            inventory.controls.UI.Enable();

            // Sélection auto d’un bouton pour la manette
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(bagFirstButton);
    }

    public void ClosePurificationInventory()
    {
        isPurifying = false;
        inventory.Bag.SetActive(false);
        inventory.BagStatue.SetActive(false);
        inventory.isOpenBag = false;
        inventory.PMA.enabled = true;
        inventory.PC.enabled = true;
        playerUI.SetActive(true);

        // Désactiver UI et activer Gameplay
        inventory.controls.Gameplay.Enable();
        inventory.controls.UI.Disable();
    }

    public void ConfirmPurification()
    {
        if (currentStack <= 0)
        {
            Debug.Log("Plus de purifications restantes !");
            confirmationPanel.SetActive(false);
            return;
        }

        if (selectedItem != null && selectedItem.isAnomaly)
        {
            selectedItem.isAnomaly = false;
            currentStack--;
            UpdatePurificationStackUI();
            Debug.Log($"{selectedItem.displayName} a été purifié !");
        }

        confirmationPanel.SetActive(false);
    }

    public void CancelPurification()
    {
        selectedItem = null;
        confirmationPanel.SetActive(false);
    }

    private void UpdatePurificationStackUI()
    {
        for (int i = 0; i < purificationStackIndicators.Length; i++)
        {
            purificationStackIndicators[i].SetActive(i < currentStack);
        }
    }
}
