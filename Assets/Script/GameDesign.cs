using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class LevelData
{
    public string levelName;
    [Tooltip("IDs des objets requis pour ce niveau")]
    public List<int> requiredItemIDs = new List<int>(); // IDs à remplir dans l'inspecteur

    [Tooltip("Nombre d'objets anormaux dans ce niveau")]
    [Range(0, 8)]
    public int Anomalies = 0;
}

public class GameDesign : MonoBehaviour
{
    [Header("Gameplay References")]
    [SerializeField] private Inventory inventory;
    [SerializeField] private doorAutomatic doorAutomatic;

    [Header("Level Settings")]
    public List<LevelData> levels;
    public int currentLevel = 0;

    [Header("UI-UX References")]
    public List<TMP_Text> itemTexts; 
    public List<Image> itemChecks; 
    public Color collectedColor = Color.green;
    public Color missingColor = Color.red;
    public Light EntryLight;

    [Header("UI Colors")]
    public Color textColor;

    private List<ItemData> requiredItems = new List<ItemData>();

    public void StartLevel()
    {

        foreach (var item in requiredItems)
        {
            Debug.Log($"{item.itemName} | Anomaly: {item.isAnomaly}");
        }

        if (levels.Count == 0 || currentLevel >= levels.Count)
        {
            Debug.LogWarning("Aucun niveau valide.");
            return;
        }

        LevelData level = levels[currentLevel];
        requiredItems.Clear();

        foreach (int id in level.requiredItemIDs)
        {
            ItemData item = inventory.items.Find(i => i.itemID == id);
            if (item != null)
            {
                item.isAnomaly = false; // reset au cas où
                requiredItems.Add(item);
            }
            else
            {
                Debug.LogWarning($"Item ID {id} non trouvé dans l'inventaire.");
            }
        }
        ApplyAnomalies(level.Anomalies);

        Debug.Log("Niveau lancé : " + level.levelName);

        // Affichage aléatoire des items dans l’UI
        List<ItemData> randomizedItems = new List<ItemData>(requiredItems);
        Shuffle(randomizedItems);

        for (int i = 0; i < itemTexts.Count; i++)
        {
            if (i < randomizedItems.Count)
            {
                itemTexts[i].text = randomizedItems[i].displayName;
                itemTexts[i].color = textColor;
            }
            else
            {
                itemTexts[i].text = "";
            }
        }

        for (int i = 0; i < itemTexts.Count; i++)
        {
            if (i < randomizedItems.Count)
            {
                itemTexts[i].text = randomizedItems[i].displayName;

                if (itemChecks != null && i < itemChecks.Count)
                {
                    itemChecks[i].sprite = randomizedItems[i].iconItem;
                    itemChecks[i].enabled = true;
                }
            }
            else
            {
                itemTexts[i].text = "";
                if (itemChecks != null && i < itemChecks.Count)
                {
                    itemChecks[i].enabled = false;
                }
            }
        }
        UpdateChecklistUI();
    }

    public void CheckCollectedItems()
    {
        var collected = inventory.collectedItems;

        List<string> missingItems = new List<string>();

        foreach (var expected in requiredItems)
        {
            bool found = collected.Exists(c => c.itemID == expected.itemID);
            if (!found)
                missingItems.Add(expected.itemName);
        }

        if (missingItems.Count == 0)
        {
            Debug.Log("Tous les bons items ont été collectés. Niveau réussi !");
            WinLevel();
        }
        else
        {
            Debug.LogWarning("Il manque : " + string.Join(", ", missingItems));
            LoseLevel();
        }

        UpdateChecklistUI();
    }

    public void UpdateChecklistUI()
    {
        for (int i = 0; i < requiredItems.Count && i < itemTexts.Count; i++)
        {
            bool isCollected = inventory.collectedItems.Exists(c => c.itemID == requiredItems[i].itemID);

            itemTexts[i].color = isCollected ? collectedColor : missingColor;

            if (itemChecks != null && i < itemChecks.Count)
                itemChecks[i].color = isCollected ? collectedColor : missingColor;
        }
    }

    private void ApplyAnomalies(int numberOfAnomalies)
    {
        if (requiredItems.Count == 0 || numberOfAnomalies <= 0) return;

        numberOfAnomalies = Mathf.Min(numberOfAnomalies, requiredItems.Count);

        List<int> indices = new List<int>();
        for (int i = 0; i < requiredItems.Count; i++) indices.Add(i);
        Shuffle(indices);

        for (int i = 0; i < numberOfAnomalies; i++)
        {
            int index = indices[i];
            requiredItems[index].isAnomaly = true;
            Debug.Log($"Anomalie affectée à : {requiredItems[index].itemName}");
        }
    }

    public void WinLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Count)
        {
            Debug.Log("Jeu terminé !");
            currentLevel = 0;
        }
        StartLevel();
    }

    public void LoseLevel()
    {
        Debug.Log("Game Over");
        currentLevel = 0;
        StartLevel();
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rnd = Random.Range(i, list.Count);
            T temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
        }
    }
}
