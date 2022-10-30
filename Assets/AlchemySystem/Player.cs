using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class PlayerStat
{
    private string statName;
    public string StatName
    {
        get { return statName; }
    }

    private int statValue;
    public int StatValue
    {
        get { return statValue; }
    }

    public PlayerStat(string name, int value)
    {
        statName = name;
        statValue = value;
    }
}

public class Player : MonoBehaviour
{
    public List<PlayerStat> playerStats = new List<PlayerStat>();

    public const int StatIndex_Health = 0;
    public const int StatIndex_MaxHealth = 1;
    public const int StatIndex_Mana = 2;
    public const int StatIndex_MaxMana = 3;


    private void Awake()
    {
        InitializeBaseStats();
    }

    void Start()
    {
        DrawStatMenu();
    }

    void InitializeBaseStats()
    {
        AddStat("Health", 20);
        AddStat("MaxHealth", 50);
        AddStat("Mana", 25);
        AddStat("MaxMana", 50);
    }

    void AddStat(string newStatName, int newStatBaseValue)
    {
        for (int i = 0; i < playerStats.Count; i++)
        {
            if (playerStats[i].StatName == newStatName)
            {
                Debug.Log($"Player already contains stat {newStatName}");
                return;
            }
        }
        PlayerStat newStat = new PlayerStat(newStatName, newStatBaseValue);
        playerStats.Add(newStat);
    }

    public void ModifyStat(string statName, int valueToAdd)
    {
        //edge cases for health, max health, mana, and max mana modifications
        if (string.Equals(statName, playerStats[StatIndex_Health].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newHealth = Mathf.Clamp(playerStats[StatIndex_Health].StatValue + valueToAdd, 0, playerStats[StatIndex_MaxHealth].StatValue);
            playerStats[StatIndex_Health] = new PlayerStat("Health", newHealth);
            return;
        }
        if (string.Equals(statName, playerStats[StatIndex_MaxHealth].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMaxHealth = playerStats[StatIndex_MaxHealth].StatValue + valueToAdd;
            int newHealth = playerStats[StatIndex_Health].StatValue + valueToAdd;
            playerStats[StatIndex_MaxMana] = new PlayerStat("MaxHealth", newMaxHealth);
            playerStats[StatIndex_Health] = new PlayerStat("Health", newHealth);
            return;
        }
        if (string.Equals(statName, playerStats[StatIndex_Mana].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMana = Mathf.Clamp(playerStats[StatIndex_Mana].StatValue + valueToAdd, 0, playerStats[StatIndex_MaxMana].StatValue);
            playerStats[StatIndex_Mana] = new PlayerStat("Mana", newMana);
            return;
        }
        if (string.Equals(statName, playerStats[StatIndex_MaxMana].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMaxMana = playerStats[StatIndex_MaxMana].StatValue + valueToAdd;
            int newMana = playerStats[StatIndex_Mana].StatValue + valueToAdd;
            playerStats[StatIndex_MaxMana] = new PlayerStat("MaxMana", newMaxMana);
            playerStats[StatIndex_Mana] = new PlayerStat("Mana", newMana);
            return;
        }

        //by default, search that the stat to modify exists
        for (int i = 0; i < playerStats.Count; i++)
        {
            if (string.Equals(statName, playerStats[i].StatName, System.StringComparison.OrdinalIgnoreCase))
            {
                int newValue = playerStats[i].StatValue + valueToAdd;
                playerStats[i] = new PlayerStat(statName, newValue);
                Debug.Log($"{statName} stat successfully modified");
                DrawStatMenu();
                return;
            }
            Debug.Log($"No Stat by name {statName} found in player");
        }
    }

    public void DrawStatMenu()
    {
        //TODO, also add scroll buttons to support multiple stats
    }
}
