using UnityEngine;
using UnityEngine.UI;

public class ItemButtonPurification : MonoBehaviour
{
    [Header("Références")]
    public ItemData itemData; // L’objet associé à ce bouton
    public StatuePurification statuePurification; // Référence à la statue

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
            Debug.LogWarning("Références manquantes dans ItemButtonPurification.");
        }
    }
}
