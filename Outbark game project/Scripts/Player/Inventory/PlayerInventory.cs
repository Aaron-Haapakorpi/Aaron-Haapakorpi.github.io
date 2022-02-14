using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
///  class for player inventory and currency handling, should probably hold guns and items player has
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    // Start is called before the first frame update
    private int coins = 0;
    private int chestKeyCount = 0;
    private int bossKeyCount = 0;
    public Action OnCoinPickUp;//not currently used
    List<Item> items = new List<Item>();
    List<BasicPassive> passives = new List<BasicPassive>();
    public PlayerStatModifiers statModifiers = new PlayerStatModifiers();
    public Action OnPassiveAdded;
    public Action OnPassiveRemoved;

    private void Awake()
    {
        GlobalReferences.instance.inventory = this;
    }

    private void Start()
    {
        for(int i=0; i< gameObject.GetComponent<PlayerShooting>().guns.Count;i++)
        {
            items.Add(GetComponent<PlayerShooting>().guns[i]);
        }
    }
    public int GetCoinCount()
    {
        return coins;
    }

    public int GetChestKeyCount()
    {
        return chestKeyCount;
    }

    public void AddBossKey()
    {
        bossKeyCount += 1;
    }

    public int GetBossKeyCount()
    {
        return bossKeyCount;
    }

    public void IncrementCoinCount(int increment)
    {
        coins += increment;
        if(GlobalReferences.instance!=null)GlobalReferences.instance.combatUi.coinCountText.text = coins.ToString();
    }

    public void AddChestKey()
    {
        chestKeyCount += 1;
    }
    public void RemoveChestKey()
    {
        chestKeyCount -= 1;
    }
    
    public void AddPassive(in BasicPassive passive)
    {
        items.Add(passive);
        passives.Add(passive);
        statModifiers.damageIncreasePercentage += passive.damageIncreacePercentage;
        statModifiers.attackSpeedIncreasePercentage += passive.attackSpeedIncreasePercentage;
        statModifiers.dropChanceIncreasePercentage += passive.dropChanceIncreasePercentage;
        statModifiers.magSizeIncreasePercentage += passive.magSizeIncreasePercentage;
        statModifiers.movementSpeedIncreasePercentage += passive.movementSpeedIncreasePercentage;
        statModifiers.reloadSpeedReductionPercentage += passive.reloadSpeedIncreasePercentage;
        statModifiers.maxBulletsIncreasePercentage += passive.maxBulletsIncreasePercentage;

        statModifiers.damageIncrease += passive.damageIncreace;
        statModifiers.attackSpeedIncrease += passive.attackSpeedIncrease;
        statModifiers.dropChanceIncrease += passive.dropChanceIncrease;
        statModifiers.magSizeIncrease += passive.magSizeIncrease;
        statModifiers.movementSpeedIncrease += passive.movementSpeedIncrease;
        statModifiers.reloadSpeedReduction += passive.reloadSpeedIncrease;
        statModifiers.maxBulletsIncrease += passive.maxBulletsIncrease;
        OnPassiveAdded?.Invoke();
    }

    public void AddItem(in Item item)
    {
        items.Add(item);
    }

    public void RemovePassive(in BasicPassive passive)
    {
        passives.Remove(passive);
        statModifiers.damageIncreasePercentage -= passive.damageIncreacePercentage;
        statModifiers.attackSpeedIncreasePercentage -= passive.attackSpeedIncreasePercentage;
        statModifiers.dropChanceIncreasePercentage -= passive.dropChanceIncreasePercentage;
        statModifiers.magSizeIncreasePercentage -= passive.magSizeIncreasePercentage;
        statModifiers.movementSpeedIncreasePercentage -= passive.movementSpeedIncreasePercentage;
        statModifiers.reloadSpeedReductionPercentage -= passive.reloadSpeedIncreasePercentage;
        statModifiers.maxBulletsIncreasePercentage -= passive.maxBulletsIncreasePercentage;

        statModifiers.damageIncrease -= passive.damageIncreace;
        statModifiers.attackSpeedIncrease -= passive.attackSpeedIncrease;
        statModifiers.dropChanceIncrease -= passive.dropChanceIncrease;
        statModifiers.magSizeIncrease -= passive.magSizeIncrease;
        statModifiers.movementSpeedIncrease -= passive.movementSpeedIncrease;
        statModifiers.reloadSpeedReduction -= passive.reloadSpeedIncrease;
        statModifiers.maxBulletsIncrease -= passive.maxBulletsIncrease;
        OnPassiveRemoved?.Invoke();
    }

    public bool ContainsItem(in Item item)
    {
        return items.Contains(item);
    }


}
public class PlayerStatModifiers
{
    public float damageIncreasePercentage = 0;
    public float attackSpeedIncreasePercentage = 0;
    public float movementSpeedIncreasePercentage = 0;
    public float dropChanceIncreasePercentage = 0;
    public float reloadSpeedReductionPercentage = 0;
    public float magSizeIncreasePercentage = 0;
    public float maxBulletsIncreasePercentage = 0;

    public float damageIncrease = 0;
    public float attackSpeedIncrease = 0;
    public float movementSpeedIncrease = 0;
    public float dropChanceIncrease = 0;
    public float reloadSpeedReduction = 0;
    public float magSizeIncrease = 0;
    public float maxBulletsIncrease = 0;
}

