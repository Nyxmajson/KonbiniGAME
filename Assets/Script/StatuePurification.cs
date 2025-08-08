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
    public int currentStack; 
    public GameObject[] stackPreviewImages;
    public GameObject[] purificationStackIndicators;

    [Header("UI Hold Feedback")]
    public Slider purificationHoldSlider; 
    public float holdTime = 2f; // Temps requis
    public float currentHold = 0f;
    public bool isHoldingPurification = false;

    [Header("Références")]
    public Inventory inventory;
    public GameObject confirmationPanel;
    public GameObject infoPanelIfSelected;
    public GameObject selectionCursor;
    private List<ItemData> selectedItems = new List<ItemData>();
    private RectTransform cursorTransform;
    public Vector3 cursorOffset = new Vector3(0, 40, 0);
    public GameObject bagFirstButton;
    public EventSystem eventSystem;
    public bool SubmitPressed;
    public bool SubmitPurificationPressed;
    public bool CancelPressed;
    public bool isPurifying = false;
    public GameObject playerUI;

    [Header("Game State")]
    public bool isDanger = false;
    public bool isTranquille = true;

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
    private void Start()
    {
        if (selectionCursor != null)
            cursorTransform = selectionCursor.GetComponent<RectTransform>();
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
    private void Update()
    {
        if (isPurifying && SubmitPressed)
        {
            SubmitPressed = false;

            GameObject selectedGO = eventSystem.currentSelectedGameObject;
            if (selectedGO != null)
            {
                var btn = selectedGO.GetComponent<ItemButtonPurification>();
                if (btn != null && btn.itemData != null)
                {
                    ToggleItemSelection(btn.itemData);
                }
            }
        }

        if (isPurifying)
        {
            UpdateSelectionCursor();
            UpdateSlotSelectionColors();
        }

        if (!isPurifying || !confirmationPanel.activeSelf) return;

        if (SubmitPurificationPressed)
        {

            isHoldingPurification = true;
            currentHold += Time.deltaTime;
            purificationHoldSlider.value = currentHold / holdTime;

            if (currentHold >= holdTime)
            {
                ConfirmPurification();
                ResetHold();
            }
        }
        else if (isHoldingPurification)
        {
            ResetHold(); // Annule si on relâche avant la fin
        }
    }
    private void ResetHold()
    {
        isHoldingPurification = false;
        currentHold = 0f;
        purificationHoldSlider.value = 0f;
    }
    private void UpdateSelectionCursor()
    {
        GameObject selectedGO = eventSystem.currentSelectedGameObject;

        if (selectedGO != null && selectedGO.GetComponent<ItemButtonPurification>() != null)
        {
            // Affiche le curseur et place-le
            if (!selectionCursor.activeSelf)
                selectionCursor.SetActive(true);

            cursorTransform.position = selectedGO.transform.position + cursorOffset; // Suivi direct
        }
        else
        {
            // Rien de sélectionné ou élément non pertinent
            selectionCursor.SetActive(false);
        }
    }

    public void ToggleItemSelection(ItemData item)
    {
        if (selectedItems.Contains(item))
        {
            selectedItems.Remove(item);
        }
        else
        {
            if (selectedItems.Count >= currentStack)
            {
                Debug.Log("Pas assez de stacks pour purifier cet objet.");
                StartCoroutine(ShakeItemSlot(item));
                return;
            }

            selectedItems.Add(item);
        }

        infoPanelIfSelected.SetActive(selectedItems.Count > 0);
        UpdateSlotSelectionColors();
        UpdateStackPreviewUI();
    }

    public void OpenPurificationInventory()
    {
            isPurifying = true;
            inventory.Bag.SetActive(true);
            inventory.BagStatue.SetActive(true);
            inventory.isOpenBag = true;
            inventory.PMA.enabled = false;
            inventory.PC.enabled = false;
            inventory.PCcinematic.enabled = false;
            playerUI.SetActive(false);
            inventory.PC.SwitchCameraStyle(PlayerCamera.CameraStyle.Camera2);
            inventory.PCcinematic.SwitchCameraStyle(PlayerCamera.CameraStyle.Camera2);

            // Désactiver Gameplay et activer UI
            inventory.controls.Gameplay.Disable();
            inventory.controls.UI.Enable();

            // Sélection auto d’un bouton pour la manette
            eventSystem.SetSelectedGameObject(null);
            eventSystem.SetSelectedGameObject(bagFirstButton);

            confirmationPanel.SetActive(true); // Toujours visible
            infoPanelIfSelected.SetActive(false); // Masqué tant qu’aucun item sélectionné
    }

    public void ClosePurificationInventory()
    {
        isPurifying = false;
        inventory.Bag.SetActive(false);
        inventory.BagStatue.SetActive(false);
        inventory.isOpenBag = false;
        inventory.PMA.enabled = true;
        inventory.PC.enabled = true;
        inventory.PCcinematic.enabled = true;
        playerUI.SetActive(true); 
        inventory.PC.SwitchCameraStyle(PlayerCamera.CameraStyle.Basic);
        inventory.PCcinematic.SwitchCameraStyle(PlayerCamera.CameraStyle.Basic);

        // Désactiver UI et activer Gameplay
        inventory.controls.Gameplay.Enable();
        inventory.controls.UI.Disable();

        infoPanelIfSelected.SetActive(false);
        confirmationPanel.SetActive(false); 
        selectionCursor.SetActive(false);
    }

    public void ConfirmPurification()
    {
        if (selectedItems.Count == 0)
        {
            Debug.Log("Aucun objet sélectionné.");
            return;
        }

        if (selectedItems.Count > currentStack)
        {
            Debug.Log("Pas assez de stacks pour purifier les objets sélectionnés.");
            StartCoroutine(ShakeStackIndicators());
            return;
        }

        foreach (var item in selectedItems)
        {
            if (item.isAnomaly)
            {
                item.isAnomaly = false;
                Debug.Log($"{item.displayName} a été purifié !");
            }
        }

        currentStack -= selectedItems.Count;

        selectedItems.Clear();
        UpdatePurificationStackUI();
        UpdateSlotSelectionColors();
        UpdateStackPreviewUI();

        //confirmationPanel.SetActive(false);
    }

    private void UpdatePurificationStackUI()
    {
        for (int i = 0; i < purificationStackIndicators.Length; i++)
        {
            purificationStackIndicators[i].SetActive(i < currentStack);
        }
    }

    private void UpdateSlotSelectionColors()
    {
        GameObject selectedGO = eventSystem.currentSelectedGameObject;

        for (int i = 0; i < inventory.inventorySlotImages.Count; i++)
        {
            var image = inventory.inventorySlotImages[i];
            var icon = inventory.inventorySlotIcon[i];
            var buttonWrapper = icon.GetComponent<ItemButtonPurification>();
            if (buttonWrapper == null) continue;

            var item = buttonWrapper.itemData;
            bool isHovered = selectedGO == image.gameObject || selectedGO == icon.gameObject;
            bool isSelected = selectedItems.Contains(item);

            if (isSelected)
                image.color = inventory.activeColor;
            else if (isHovered)
                image.color = inventory.hoverColor;
            else
                image.color = inventory.neutralColor;
        }
    }

    private void UpdateStackPreviewUI()
    {
        for (int i = 0; i < purificationStackIndicators.Length; i++)
        {
            bool usedForPreview = i < selectedItems.Count;
            bool usedForReal = i >= currentStack;

            if (usedForPreview && i < currentStack)
            {
                stackPreviewImages[i].SetActive(true); // Grisé ou autre
                purificationStackIndicators[i].SetActive(false); // Cache stack "active"
            }
            else
            {
                stackPreviewImages[i].SetActive(false);
                purificationStackIndicators[i].SetActive(i < currentStack);
            }
        }
    }

    private IEnumerator ShakeStackIndicators()
    {
        float elapsed = 0f;
        float duration = 0.3f;
        float strength = 8f;

        Vector3[] originalPositions = new Vector3[purificationStackIndicators.Length];

        for (int i = 0; i < purificationStackIndicators.Length; i++)
        {
            if (purificationStackIndicators[i].activeSelf)
                originalPositions[i] = purificationStackIndicators[i].transform.localPosition;
        }

        while (elapsed < duration)
        {
            float offsetX = Mathf.Sin(elapsed * 40f) * strength;

            for (int i = 0; i < purificationStackIndicators.Length; i++)
            {
                if (purificationStackIndicators[i].activeSelf)
                {
                    purificationStackIndicators[i].transform.localPosition = originalPositions[i] + new Vector3(offsetX, 0, 0);
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < purificationStackIndicators.Length; i++)
        {
            if (purificationStackIndicators[i].activeSelf)
                purificationStackIndicators[i].transform.localPosition = originalPositions[i];
        }
    }

    private IEnumerator ShakeItemSlot(ItemData item)
    {
        int index = inventory.collectedItems.IndexOf(item);
        if (index < 0 || index >= inventory.inventorySlotImages.Count)
            yield break;

        var slot = inventory.inventorySlotImages[index];
        var originalPos = slot.transform.localPosition;
        float elapsed = 0f;
        float duration = 0.3f;
        float strength = 8f;

        while (elapsed < duration)
        {
            float offsetX = Mathf.Sin(elapsed * 40f) * strength;
            slot.transform.localPosition = originalPos + new Vector3(offsetX, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        slot.transform.localPosition = originalPos;
    }

}
