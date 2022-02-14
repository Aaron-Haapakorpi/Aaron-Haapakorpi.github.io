using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// only movement speed increases work currently, need to add other ones in code
/// </summary>
[CreateAssetMenu(menuName = "Passives/BasicPassive")]
public class BasicPassive: Item
{
    public float damageIncreacePercentage = 0;
    public float attackSpeedIncreasePercentage = 0;
    public float movementSpeedIncreasePercentage = 0;
    public float dropChanceIncreasePercentage = 0;
    public float reloadSpeedIncreasePercentage = 0;
    public float magSizeIncreasePercentage = 0;
    public float maxBulletsIncreasePercentage = 0;
    public float bulletSpeedIncreasePercentage = 0;

    public float damageIncreace = 0;
    public float attackSpeedIncrease = 0;
    public float movementSpeedIncrease = 0;
    public float dropChanceIncrease = 0;
    public float reloadSpeedIncrease = 0;
    public float magSizeIncrease = 0;
    public float maxBulletsIncrease = 0;


    // Start is called before the first frame update
    public override void OnReceived()
    {
        var p = GlobalReferences.instance.players[0];
        p.GetComponent<PlayerInventory>().AddPassive(this);
    }
}
