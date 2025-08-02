using UnityEngine;
using UnityEngine.UI;

public class ItemButtonPurification : MonoBehaviour
{
    [Header("R�f�rences")]
    public ItemData itemData; // L�objet associ� � ce bouton
    public StatuePurification statuePurification; // R�f�rence � la statue

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnClickItem);
        }
    }

    public void OnClickItem()
    {
        if (statuePurification != null && itemData != null)
        {
            statuePurification.SelectItemToPurify(itemData);
        }
        else
        {
            Debug.LogWarning("R�f�rences manquantes dans ItemButtonPurification.");
        }
    }
}
