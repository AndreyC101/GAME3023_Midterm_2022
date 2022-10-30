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

    public List<Inventory.ItemStack> requirements = new List<Inventory.ItemStack>();
    public List<PlayerStat> effects = new List<PlayerStat>();

    public void Use()
    {
        Debug.Log("Used item: " + name);
        if (effects.Count > 0)
        {
            Player player = FindObjectOfType<Player>();
            for (int i = 0; i < effects.Count; i++)
            {
                player.ModifyStat(effects[i].StatName, effects[i].StatValue);
                Debug.Log($"Player {effects[i].StatName} modified by {effects[i].StatValue}");
            }
        }
        FindObjectOfType<Inventory>().OnItemUsed(this);
    }

    //returns full description with effects and if needed, required crafting materials
    public string GetDescription(bool withRequirements)
    {
        var stringBuilder = new System.Text.StringBuilder();
        stringBuilder.Append(description);
        for (int i = 0; i < effects.Count; i++)
        {
            stringBuilder.AppendLine($"{(effects[i].StatValue > 0 ? "+" : "-")} {effects[i].StatValue} to {effects[i].StatName}");
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
}
