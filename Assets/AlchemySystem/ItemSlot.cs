using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//Holds reference to item, manages visibility in the Inventory panel
public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item = null;

    private Inventory playerInventory;

    [SerializeField]
    private TMPro.TextMeshProUGUI descriptionText;
    [SerializeField]
    private TMPro.TextMeshProUGUI nameText;

    [SerializeField]
    Image itemIcon;

    [SerializeField]
    TextMeshProUGUI itemCountText;

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
            itemCountText.gameObject.SetActive(false);
        }
        else
        {
            //set sprite to the one from the item
            itemIcon.sprite = item.icon;
            itemIcon.gameObject.SetActive(true);
            itemCountText.gameObject.SetActive(true);
            itemCountText.text = playerInventory.CheckItemCount(item).ToString();
        }
    }

    public void UseItemInSlot()
    {
        if (CanUseItem())
        {
            item.Use();
            if (playerInventory.CheckItemCount(item) == 0)
            {
                item = null;
                UpdateGraphic();
            }
        }
    }

    private bool CanUseItem()
    {
        return (item != null && item.isConsumable && playerInventory.CheckItemCount(item) > 0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            descriptionText.text = item.GetDescription(false);
            nameText.text = item.name;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(item != null)
        {
            descriptionText.text = "";
            nameText.text = "";
        }
    }
}
