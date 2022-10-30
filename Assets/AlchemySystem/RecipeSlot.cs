using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeSlot : MonoBehaviour
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
    GameObject progressBar;

    [SerializeField]
    Image progressBarFill;

    private bool craftingInProgress = false;
    private float craftTime = 1.5f;
    private float currentCraftingTime = 0.0f;
    private float fillBarTargetWidth = 58.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerInventory = FindObjectOfType<Inventory>();
        UpdateGraphic();
    }

    private void FixedUpdate()
    {
        if (craftingInProgress)
        {
            currentCraftingTime += Time.fixedDeltaTime;
            float progress = currentCraftingTime / craftTime;
            float fillBarWidth = Mathf.Lerp(0.0f, fillBarTargetWidth, progress);
            progressBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 7.0f);
            if (progress >= 1.0f)
            {
                CraftItemInSlot();
            }
        }
        progressBar.SetActive(craftingInProgress);
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
            if (craftingInProgress) InterruptCrafting();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item != null)
        {
            TryBeginCrafting();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (item != null && craftingInProgress) InterruptCrafting();
    }

    private void TryBeginCrafting()
    {
        if (CanCraftItem())
        {
            craftingInProgress = true;
        }
    }

    private void InterruptCrafting()
    {
        craftingInProgress = false;
        currentCraftingTime = 0.0f;
        progressBarFill.GetComponent<RectTransform>().sizeDelta = new Vector2(0.0f, 7.0f);
        progressBar.SetActive(false);
    }

    private void CraftItemInSlot()
    {
        InterruptCrafting();
        for (int i = 0; i < item.requirements.Count; i++)
        {
            bool sacrificeSuccessful = playerInventory.ConsumeItemForCrafting(item.requirements[i]);
            if (!sacrificeSuccessful)
            {
                return;
            }
        }
        playerInventory.AddItemToInventory(item, 1);
    }
}
