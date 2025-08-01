using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Global References")]
    private PlayerControls controls;
    private bool CheckListPressed;
    private bool BagPressed;
    public PlayerMovementAdvanced PMA;

    [Header("Items")]
    public List<ItemData> items = new List<ItemData>();

    // Accès pratique :
    public ItemData GetItemByName(string name)
    {
        return items.Find(i => i.itemName == name);
    }

    [Header("Liste UI Element")]
    // Remplacer par les images d'état des items dans le futur
    public Color activeColor;
    public Color anomalyColor;
    public Color neutralColor; 
    
    [Header("Inventory/Checklist UI")]
    public GameObject CheckList;
    public GameObject Bag;
    public bool isOpenCheckList;
    public bool isOpenBag;
    public List<TMP_Text> inventorySlotTexts = new List<TMP_Text>();
    public List<Image> inventorySlotImages = new List<Image>();
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
        if (CheckListPressed)
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

        if (BagPressed)
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

        if (isOpenCheckList && isOpenBag) 
        { 
            PMA.moveSpeed = 0;
            PMA.sprintOn = false;
            PMA.walkOn = false;
            PMA.slowOn = false;

        }
    }

    public void UpdateInventoryDisplay()
    {
        // Clear UI
        for (int i = 0; i < inventorySlotTexts.Count; i++)
        {
            inventorySlotTexts[i].text = "Empty";
            inventorySlotImages[i].color = neutralColor;
        }

        // Affiche les items collectés
        for (int i = 0; i < collectedItems.Count && i < 8; i++)
        {
            inventorySlotTexts[i].text = collectedItems[i].itemName;
            inventorySlotImages[i].color = collectedItems[i].isAnomaly ? anomalyColor : activeColor;
        }
    }

}
