using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct ItemStack
{
    private Item stackedItem;
    public Item StackedItem
    {
        get { return stackedItem; }
    }
    public string StackedItemName
    {
        get { return stackedItem.name; }
    }

    private int stackSize;
    public int StackSize
    {
        get { return stackSize; }
    }

    public ItemStack(Item stackedItem, int stackSize)
    {
        this.stackedItem = stackedItem;
        this.stackSize = stackSize;
    }
}


public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<Item> StartingInventory = new List<Item>();
    [SerializeField]
    private List<int> StartingInventoryCounts = new List<int>();

    public List<ItemStack> inventoryItems = new List<ItemStack>();

    List<ItemSlot> itemSlots = new List<ItemSlot>();

    [SerializeField]
    GameObject inventoryPanel;

    private int displayStartIndex = 0;

    [SerializeField]
    private GameObject scrollDownButton;

    [SerializeField]
    private GameObject scrollUpButton;

    private void Awake()
    {
        GenerateStartingInventory();
    }

    void Start()
    {
        itemSlots = new List<ItemSlot>(
            inventoryPanel.transform.GetComponentsInChildren<ItemSlot>()
            );
        DisplayInventoryGrid();
    }

    //Menu and navigation
    void DisplayInventoryGrid()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].StackSize == 0)
            {
                inventoryItems.RemoveAt(i);
            }
        }
        bool hasEmptyGridSpaces = false;
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (inventoryItems.Count > displayStartIndex + i)
            {
                itemSlots[i].SlotItem(inventoryItems[displayStartIndex + i].StackedItem); //.item = inventoryItems[displayStartIndex + i].StackedItem;
            }
            else 
            {
                itemSlots[i].item = null;
                hasEmptyGridSpaces = true;
            }
            itemSlots[i].UpdateGraphic();
        }
        scrollDownButton.SetActive(!hasEmptyGridSpaces);
        scrollUpButton.SetActive(displayStartIndex > 0);
    }

    public void ScrollDown()
    {
        displayStartIndex += 4;
        DisplayInventoryGrid();
    }

    public void ScrollUp()
    {
        displayStartIndex -= 4;
        if (displayStartIndex < 0) displayStartIndex = 0;
        DisplayInventoryGrid();
    }

    private void GenerateStartingInventory()
    {
        for (int i = 0; i < StartingInventory.Count; i++)
        {
            int count;
            if (i >= StartingInventoryCounts.Count || StartingInventoryCounts[i] < 1) count = 1;
            else count = StartingInventoryCounts[i];
            ItemStack newStack = new ItemStack(StartingInventory[i], count);
            inventoryItems.Add(newStack);
        }
    }


    //Crafting System
    private int GetItemIndex(Item requiredItem)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].StackedItem == requiredItem) return i;
        }
        return -1;
    }

    public bool CheckItemAvailability(ItemStack requiredItem)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].StackedItem == requiredItem.StackedItem && inventoryItems[i].StackSize >= requiredItem.StackSize) return true;
        }
        return false;
    }

    public int CheckItemCount(Item requiredItem)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].StackedItem == requiredItem) return inventoryItems[i].StackSize;
        }
        return 0;
    }

    public bool ConsumeItemForCrafting(ItemStack requiredItem)
    {
        int itemIndex = GetItemIndex(requiredItem.StackedItem);
        if (itemIndex >= 0)
        {
            ItemStack newStack = new ItemStack(requiredItem.StackedItem, inventoryItems[itemIndex].StackSize - requiredItem.StackSize);
            inventoryItems[itemIndex] = newStack;
            DisplayInventoryGrid();
            return true;
        }
        else
        {
            Debug.LogError($"Item {requiredItem.StackedItemName} was not found in player inventory or amount carried was insufficient");
            return false;
        }
    }

    public void AddItemToInventory(Item newItem, int amount)
    {
        int itemIndex = GetItemIndex(newItem);
        if (itemIndex >= 0)
        {
            ItemStack newStack = new ItemStack(newItem, inventoryItems[itemIndex].StackSize + amount);
            inventoryItems[itemIndex] = newStack;
        }
        else
        {
            ItemStack newStack = new ItemStack(newItem, amount);
            inventoryItems.Add(newStack);
        }
        DisplayInventoryGrid();
    }

    public void OnItemUsed(Item usedItem)
    {
        int itemIndex = GetItemIndex(usedItem);
        if (itemIndex >= 0)
        {
            ItemStack newStack = new ItemStack(usedItem, inventoryItems[itemIndex].StackSize - 1);
            inventoryItems[itemIndex] = newStack;
        }
        else
        {
            Debug.Log($"{usedItem.name} not found in player inventory");
        }
    }
}
