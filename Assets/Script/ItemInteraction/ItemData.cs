using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemData
{
    public int itemID;
    public string itemName;

    public bool hasItem;
    public bool isAnomaly;

    public ObjetPath objetPath; // Script attach� � l�objet 3D du panier
    public GameObject visualObject; // Mod�le 3D � activer

    public Image inactiveImage;
    public Image activeImage;
}
