using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attribute which allows right click->Create
[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject //Extending SO allows us to have an object which exists in the project, not in the scene
{
    public Sprite icon;
    public string description = "";
    public bool isConsumable = false;

    [SerializeField]
    private List<Item> CraftingRequirements = new List<Item>();
    [SerializeField]
    private List<int> CraftingRequirementAmounts = new List<int>();

    [SerializeField]
    private List<string> StatsToModify = new List<string>();
    [SerializeField]
    private List<int> StatModifiers = new List<int>();

    public List<ItemStack> requirements = new List<ItemStack>();
    public List<PlayerStat> effects = new List<PlayerStat>();

    public void Use()
    {
        Debug.Log("Used item: " + name);
        if (effects.Count > 0)
        {
            Player player = FindObjectOfType<Player>();
            for (int i = 0; i < effects.Count; i++)
            {
                player.ModifyStat(effects[i]);
                Debug.Log($"Player {effects[i].StatName} modified by {effects[i].StatValue}");
            }
        }
        FindObjectOfType<Inventory>().OnItemUsed(this);
    }

    //returns full description with effects and if needed, required crafting materials
    public string GetDescription(bool withRequirements)
    {
        var stringBuilder = new System.Text.StringBuilder();
        stringBuilder.AppendLine(description);
        for (int i = 0; i < effects.Count; i++)
        {
            stringBuilder.AppendLine($" {(effects[i].StatValue > 0 ? "+" : "-")}{effects[i].StatValue} {effects[i].StatName}");
        }
        if (withRequirements)
        {
            stringBuilder.AppendLine("Requires: ");
            for (int i = 0; i < requirements.Count; i++)
            {
                stringBuilder.AppendLine($"{requirements[i].StackSize} x {requirements[i].StackedItemName}");
            }
        }
        return stringBuilder.ToString();
    }

    public void GenerateStatsAndRequirements()
    {
        for (int i = 0; i < CraftingRequirements.Count; i++)
        {
            int amountRequired;
            if (i >= CraftingRequirementAmounts.Count || CraftingRequirementAmounts[i] < 1) amountRequired = 1;
            else amountRequired = CraftingRequirementAmounts[i];
            ItemStack requirement = new ItemStack(CraftingRequirements[i], amountRequired);
            requirements.Add(requirement);
        }
        for (int i = 0; i < StatsToModify.Count; i++)
        {
            int modifier;
            if (i >= StatModifiers.Count) modifier = 1;
            else modifier = StatModifiers[i];
            PlayerStat statModifier = new PlayerStat(StatsToModify[i], modifier);
            effects.Add(statModifier);
        }
    }
}
