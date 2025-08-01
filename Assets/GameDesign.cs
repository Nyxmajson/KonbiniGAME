using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDesign : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Paiement paiement;
    [SerializeField] private PlayerMovementAdvanced PMA;

    [Header("Design")]
    public int NombreDeNiveau;
    public int NumeroNiveauActuel;

    public struct Level
    {
        public string namelevel;
        public TMP_Text list1;
        public Image imgCheck1;
        public TMP_Text list2;
        public Image imgCheck2;
        public TMP_Text list3;
        public Image imgCheck3;
        public TMP_Text list4;
        public Image imgCheck4;
        public TMP_Text list5;
        public Image imgCheck5;
        public TMP_Text list6;
        public Image imgCheck6;
        public TMP_Text list7;
        public Image imgCheck7;
        public TMP_Text list8;
        public Image imgCheck8;
    }

}
