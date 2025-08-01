using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paiement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerCamera playerCamera;

    [Header("Design")]
    public bool Stade1;
    public bool Stade2;
    public bool Stade3;
    public List<ScriptableObject> listInventory;

    [Header("UX Element")]
    public GameObject CaisseFB;
    public Color GoodItemColor;
    public Color BadItemColor;

}
