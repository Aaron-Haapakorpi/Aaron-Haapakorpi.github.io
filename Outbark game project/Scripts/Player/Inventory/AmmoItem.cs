using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/AmmoItem")]
public class AmmoItem : Item
{
    public Sprite sprite;

    [SerializeField]
    private int ammoValue = 1;
    public override void OnReceived()
    {
        GlobalReferences.instance.players[0].GetComponent<PlayerShooting>().PickUpAmmo(ammoValue);
    }
}
