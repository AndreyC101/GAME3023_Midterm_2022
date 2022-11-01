using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    [SerializeField]
    List<Item> allItemIndex = new List<Item>();

    List<Item> craftableItems = new List<Item>();

    List<RecipeSlot> recipeSlots = new List<RecipeSlot>();

    [SerializeField]
    GameObject craftingPanel;

    private int displayStartIndex = 0;

    [SerializeField]
    private GameObject scrollUpButton;

    [SerializeField]
    private GameObject scrollDownButton;

    private void Awake()
    {
        GenerateCraftableItems();
    }

    // Start is called before the first frame update
    void Start()
    {
        recipeSlots = new List<RecipeSlot>(
            craftingPanel.transform.GetComponentsInChildren<RecipeSlot>()
            );
        DisplayCraftingGrid();
    }

    void DisplayCraftingGrid()
    {
        bool hasEmptyGridSpaces = false;
        for (int i = 0; i < recipeSlots.Count; i++)
        {
            if (craftableItems.Count > displayStartIndex + i)
            {
                recipeSlots[i].item = craftableItems[displayStartIndex + i];
            }
            else
            {
                recipeSlots[i].item = null;
                hasEmptyGridSpaces = true;
            }
            recipeSlots[i].UpdateGraphic();
        }
        scrollDownButton.SetActive(!hasEmptyGridSpaces);
        scrollUpButton.SetActive(displayStartIndex > 0);
    }

    public void ScrollDown()
    {
        displayStartIndex += 2;
        DisplayCraftingGrid();
    }

    public void ScrollUp()
    {
        displayStartIndex -= 2;
        if (displayStartIndex < 0) displayStartIndex = 0;
        DisplayCraftingGrid();
    }

    private void GenerateCraftableItems()
    {
        for (int i = 0; i < allItemIndex.Count; i++)
        {
            allItemIndex[i].GenerateStatsAndRequirements();
            if (allItemIndex[i].requirements.Count > 0)
            {
                craftableItems.Add(allItemIndex[i]);
            }
        }
    }
}
