using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public struct PlayerStat
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

    [SerializeField]
    private TMP_Text HPDisplay;
    [SerializeField]
    private TMP_Text ManaDisplay;

    [SerializeField]
    private TMP_Text[] StatNameDisplays;

    [SerializeField]
    private TMP_Text[] StatAmountDisplays;

    [SerializeField]
    private GameObject StatScrollUpButton;
    [SerializeField]
    private GameObject StatScrollDownButton;

    private int displayStartIndex = 4;
    private const int statDisplayCount = 5;

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
        AddStat("Max Health", 50);
        AddStat("Mana", 25);
        AddStat("Max Mana", 50);

        AddStat("Strength", 10);
        AddStat("Dexterity", 10);
        AddStat("Intelligence", 10);
        AddStat("Defense", 10);
        AddStat("Endurance", 10);
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

    public void ModifyStat(PlayerStat newStat)
    {
        //edge cases for health, max health, mana, and max mana modifications
        if (string.Equals(newStat.StatName, playerStats[StatIndex_Health].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newHealth = Mathf.Clamp(playerStats[StatIndex_Health].StatValue + newStat.StatValue, 0, playerStats[StatIndex_MaxHealth].StatValue);
            playerStats[StatIndex_Health] = new PlayerStat("Health", newHealth);
            DrawStatMenu();
            return;
        }
        if (string.Equals(newStat.StatName, playerStats[StatIndex_MaxHealth].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMaxHealth = playerStats[StatIndex_MaxHealth].StatValue + newStat.StatValue;
            int newHealth = playerStats[StatIndex_Health].StatValue + newStat.StatValue;
            playerStats[StatIndex_MaxHealth] = new PlayerStat("Max Health", newMaxHealth);
            playerStats[StatIndex_Health] = new PlayerStat("Health", newHealth);
            DrawStatMenu();
            return;
        }
        if (string.Equals(newStat.StatName, playerStats[StatIndex_Mana].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMana = Mathf.Clamp(playerStats[StatIndex_Mana].StatValue + newStat.StatValue, 0, playerStats[StatIndex_MaxMana].StatValue);
            playerStats[StatIndex_Mana] = new PlayerStat("Mana", newMana);
            DrawStatMenu();
            return;
        }
        if (string.Equals(newStat.StatName, playerStats[StatIndex_MaxMana].StatName, System.StringComparison.OrdinalIgnoreCase))
        {
            int newMaxMana = playerStats[StatIndex_MaxMana].StatValue + newStat.StatValue;
            int newMana = playerStats[StatIndex_Mana].StatValue + newStat.StatValue;
            playerStats[StatIndex_MaxMana] = new PlayerStat("Max Mana", newMaxMana);
            playerStats[StatIndex_Mana] = new PlayerStat("Mana", newMana);
            DrawStatMenu();
            return;
        }

        //by default, search that the stat to modify exists
        for (int i = 0; i < playerStats.Count; i++)
        {
            if (string.Equals(newStat.StatName, playerStats[i].StatName, System.StringComparison.OrdinalIgnoreCase))
            {
                int newValue = playerStats[i].StatValue + newStat.StatValue;
                playerStats[i] = new PlayerStat(newStat.StatName, newValue);
                Debug.Log($"{newStat.StatName} stat successfully modified");
                DrawStatMenu();
                return;
            }
            Debug.Log($"No Stat by name {newStat.StatName} found in player");
        }
    }

    public void DrawStatMenu()
    {
        HPDisplay.text = $"{playerStats[StatIndex_Health].StatValue}/{playerStats[StatIndex_MaxHealth].StatValue}";
        ManaDisplay.text = $"{playerStats[StatIndex_Mana].StatValue}/{playerStats[StatIndex_MaxMana].StatValue}";

        for (int i = 0; i < statDisplayCount; i++)
        {
            StatNameDisplays[i].text = $"{playerStats[i+displayStartIndex].StatName}";
            StatAmountDisplays[i].text = $"{playerStats[i + displayStartIndex].StatValue}";
        }

        StatScrollUpButton.SetActive(displayStartIndex > 4);
        StatScrollDownButton.SetActive(displayStartIndex < playerStats.Count - statDisplayCount);
    }

    public void ScrollDown()
    {
        displayStartIndex++;
        if (displayStartIndex >= playerStats.Count - statDisplayCount) displayStartIndex = playerStats.Count - statDisplayCount;
        DrawStatMenu();
    }

    public void ScrollUp()
    {
        displayStartIndex--;
        if (displayStartIndex < 4) displayStartIndex = 4;
        DrawStatMenu();
    }
}
