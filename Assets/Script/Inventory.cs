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

    [Header("References Items")]
    [SerializeField] private Snack_1 snack1;
    [SerializeField] private Snack_2 snack2;
    [SerializeField] private Stuff_1 stuff1;
    [SerializeField] private Stuff_2 stuff2;
    [SerializeField] private Cigarette cigarette;
    [SerializeField] private Souvenir souvenir;
    [SerializeField] private MicroOnde microonde;
    [SerializeField] private Magazine_1 magazine1;
    [SerializeField] private Magazine_2 magazine2;
    [SerializeField] private Magazine_3 magazine3;
    [SerializeField] private Drink_1 drink1;
    [SerializeField] private Drink_2 drink2;
    [SerializeField] private Drink_3 drink3;
    [SerializeField] private Drink_4 drink4;
    [SerializeField] private Fruit_1 fruit1;
    [SerializeField] private Fruit_2 fruit2;
    [SerializeField] private Legume_1 legume1;
    [SerializeField] private Legume_2 legume2;

    [Header("Items")]
    public bool HasSnack1 = false;
    public bool HasSnack2 = false;
    public bool HasStuff1 = false;
    public bool HasStuff2 = false;
    public bool HasCigarette = false;
    public bool HasSouvenir = false;
    public bool HasMicroonde = false;
    public bool HasMagazine1 = false;
    public bool HasMagazine2 = false;
    public bool HasMagazine3 = false;
    public bool HasDrink1 = false;
    public bool HasDrink2 = false;
    public bool HasDrink3 = false;
    public bool HasDrink4 = false;
    public bool HasFruit1 = false;
    public bool HasFruit2 = false;
    public bool HasLegume1 = false;
    public bool HasLegume2 = false;
    [Space]
    public bool isAnomalySnack1 = false;
    public bool isAnomalySnack2 = false;
    public bool isAnomalyStuff1 = false;
    public bool isAnomalyStuff2 = false;
    public bool isAnomalyCigarette = false;
    public bool isAnomalySouvenir = false;
    public bool isAnomalyMicroonde = false;
    public bool isAnomalyMagazine1 = false;
    public bool isAnomalyMagazine2 = false;
    public bool isAnomalyMagazine3 = false;
    public bool isAnomalyDrink1 = false;
    public bool isAnomalyDrink2 = false;
    public bool isAnomalyDrink3 = false;
    public bool isAnomalyDrink4 = false;
    public bool isAnomalyFruit1 = false;
    public bool isAnomalyFruit2 = false;
    public bool isAnomalyLegume1 = false;
    public bool isAnomalyLegume2 = false;

    [Header("Liste UI Element")]
    public GameObject CheckList;
    public GameObject Bag;
    public bool isOpenCheckList;
    public bool isOpenBag;
    // Remplacer par les images d'état des items dans le futur
    public Color activeColor;
    public Color anomalyColor;
    public Color neutralColor;

    [Space]
    public TMP_Text text_snack1;
    public TMP_Text text_snack2;
    public TMP_Text text_stuff1;
    public TMP_Text text_stuff2;
    public TMP_Text text_cigarette;
    public TMP_Text text_souvenir;
    public TMP_Text text_microonde;
    public TMP_Text text_magazine1;
    public TMP_Text text_magazine2;
    public TMP_Text text_magazine3;
    public TMP_Text text_drink1;
    public TMP_Text text_drink2;
    public TMP_Text text_drink3;
    public TMP_Text text_drink4;
    public TMP_Text text_fruit1;
    public TMP_Text text_fruit2;
    public TMP_Text text_legume1;
    public TMP_Text text_legume2;
    [Space]
    public Image img_snack1;
    public Image img_snack2;
    public Image img_stuff1;
    public Image img_stuff2;
    public Image img_cigarette;
    public Image img_souvenir;
    public Image img_microonde;
    public Image img_magazine1;
    public Image img_magazine2;
    public Image img_magazine3;
    public Image img_drink1;
    public Image img_drink2;
    public Image img_drink3;
    public Image img_drink4;
    public Image img_fruit1;
    public Image img_fruit2;
    public Image img_legume1;
    public Image img_legume2;

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
            }
            else
            {
                Debug.Log("Close Baglist");
                Bag.SetActive(false);
                isOpenBag = false;
                BagPressed = false;
            }
        }
    }

}
