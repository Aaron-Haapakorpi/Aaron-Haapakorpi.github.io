using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/HealthItem")]
public class HealthItem : Item
{
    public Sprite sprite;

    [SerializeField]
    private int healValue = 1;
    public override void OnReceived()
    {
        GlobalReferences.instance.players[0].GetComponent<PlayerDamageable>().Heal(healValue);
    }
}
