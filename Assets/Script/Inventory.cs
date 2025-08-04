using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("Global References")]
    public PlayerControls controls;
    private bool CheckListPressed;
    private bool BagPressed;
    public PlayerMovementAdvanced PMA;
    public PlayerCamera PC;
    public PlayerCamera PCcinematic;
    [SerializeField] private StatuePurification statuePurification;

    [Header("Items")] 
    public Transform panierParent; 
    public List<ItemData> items = new List<ItemData>();

    // Accès pratique :
    public ItemData GetItemByName(string name)
    {
        return items.Find(i => i.itemName == name);
    }

    [Header("Liste UI Element")]
    // Remplacer par les images d'état des items dans le futur
    public Color activeColor;
    public Color hoverColor;
    public Color anomalyColor;
    public Color neutralColor; 
    
    [Header("Inventory/Checklist UI")]
    public GameObject CheckList;
    public GameObject Bag;
    public GameObject BagStatue;
    public bool isOpenCheckList;
    public bool isOpenBag;
    public List<TMP_Text> inventorySlotTexts = new List<TMP_Text>();
    public List<Image> inventorySlotImages = new List<Image>();
    public List<Image> inventorySlotIcon= new List<Image>();
    public List<ItemData> collectedItems = new List<ItemData>();

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Checklist.performed += ctx => CheckListPressed = true;
        controls.Gameplay.Checklist.canceled += ctx => CheckListPressed = false;

        controls.Gameplay.Bag.performed += ctx => BagPressed = true;
        controls.Gameplay.Bag.canceled += ctx => BagPressed = false;
    }
    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
        controls.UI.Disable();
    }

    private void Update()
    {
        if (CheckListPressed && !statuePurification.isPurifying)
        {
            if (!isOpenCheckList)
            {
                Debug.Log("Open Checklist");
                CheckList.SetActive(true);
                isOpenCheckList = true;
                CheckListPressed = false;
                PMA.moveSpeed = PMA.slowSpeed;
                PMA.sprintOn = false;
                PMA.walkOn = false;
            }
            else
            {
                Debug.Log("Close Checklist");
                CheckList.SetActive(false);
                isOpenCheckList = false;
                CheckListPressed = false;
            }
        }

        if (BagPressed && !statuePurification.isPurifying)
        {

            if (!isOpenBag)
            {
                Debug.Log("Open Baglist");
                Bag.SetActive(true);
                isOpenBag = true;
                BagPressed = false;
                PMA.moveSpeed = PMA.slowSpeed;
                PMA.sprintOn = false;
                PMA.walkOn = false;
            }
            else
            {
                Debug.Log("Close Baglist");
                Bag.SetActive(false);
                isOpenBag = false;
                BagPressed = false;
            }
        }

        if (statuePurification.CancelPressed && statuePurification.isPurifying)
        {
            statuePurification.ClosePurificationInventory();
            statuePurification.CancelPressed = false;
        }
    }

    public void UpdateInventoryDisplay()
    {
        // Clear UI
        for (int i = 0; i < inventorySlotTexts.Count; i++)
        {
            inventorySlotTexts[i].text = "Empty";
            inventorySlotImages[i].color = neutralColor; 
            inventorySlotIcon[i].sprite = null;
            inventorySlotIcon[i].gameObject.SetActive(false);
        }

        // Affiche les items collectés
        for (int i = 0; i < collectedItems.Count && i < 8; i++)
        {
            inventorySlotTexts[i].text = collectedItems[i].displayName;
            inventorySlotImages[i].color = hoverColor;
            inventorySlotIcon[i].sprite = collectedItems[i].iconItem; //avant c'était items[i].iconItem donc si j'ai un truc à tout moment c'est ici le prob
            inventorySlotIcon[i].gameObject.SetActive(true);

            var buttonGO = inventorySlotImages[i].GetComponent<Button>();
            if (buttonGO != null)
            {
                var wrapper = buttonGO.GetComponent<ItemButtonPurification>();
                if (wrapper == null)
                    wrapper = buttonGO.gameObject.AddComponent<ItemButtonPurification>();

                wrapper.itemData = collectedItems[i];
                wrapper.statuePurification = statuePurification;
            }
        }

    }

    public bool HasAnyItem()
    {
        return collectedItems.Count > 0;
    }

}
