using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item = null;

    private Inventory playerInventory;

    [SerializeField]
    private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText;

    [SerializeField]
    Image itemIcon;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
        UpdateGraphic();
    }

    public void SlotItem(Item newItem)
    {
        this.item = newItem;
        UpdateGraphic();
    }

    //Change Icon and count
    public void UpdateGraphic()
    {
        if (item == null)
        {
            itemIcon.gameObject.SetActive(false);
        }
        else
        {
            //set sprite to the one from the item
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
        }
    }

    private bool CanCraftItem()
    {
        if (item == null) return false;
        bool ingredientsMissing = false;
        for (int i = 0; i < item.requirements.Count; i++)
        {
            if (!playerInventory.CheckItemAvailability(item.requirements[i])) ingredientsMissing = true;
        }
        if (ingredientsMissing) Debug.Log($"Not enough resources to craft {item.name}");
        return !ingredientsMissing;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = item.GetDescription(true);
            nameText.text = item.name;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = "";
            nameText.text = "";
        }
    }

    private void CraftItemInSlot()
    {
        for (int i = 0; i < item.requirements.Count; i++)
        {
            bool sacrificeSuccessful = playerInventory.ConsumeItemForCrafting(item.requirements[i]);
            if (!sacrificeSuccessful)
            {
                Debug.LogError($"Could not properly consume items to craft {item.name}");
                return;
            }
        }
        playerInventory.AddItemToInventory(item, 1);
    }

    public void TryCraftItem()
    {
        if (item != null && CanCraftItem())
        {
            Debug.Log($"Crafting {item.name}");
            CraftItemInSlot();
        }
    }
}
