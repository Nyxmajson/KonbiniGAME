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
}

public class GameDesign : MonoBehaviour
{
    [Header("Gameplay References")]
    public Inventory inventory;

    [Header("Level Settings")]
    public List<LevelData> levels;
    public int currentLevel = 0;

    [Header("UI References")]
    public List<TMP_Text> itemTexts;  // 8 max
    public List<Image> itemChecks;    // 8 max (optionnel)

    [Header("UI Colors")]
    public Color neutralColor;

    private List<ItemData> requiredItems = new List<ItemData>();

    public void StartLevel()
    {
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
                requiredItems.Add(item);
            else
                Debug.LogWarning($"Item ID {id} non trouvé dans l'inventaire.");
        }

        Debug.Log("Niveau lancé : " + level.levelName);

        // Affichage aléatoire des items dans l’UI
        List<ItemData> randomizedItems = new List<ItemData>(requiredItems);
        Shuffle(randomizedItems);

        for (int i = 0; i < itemTexts.Count; i++)
        {
            if (i < randomizedItems.Count)
            {
                itemTexts[i].text = randomizedItems[i].itemName;
                itemTexts[i].color = neutralColor;
            }
            else
            {
                itemTexts[i].text = "";
            }
        }
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
