using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/CoinItem")]
public class CoinItem : Item
{
    public Sprite coinSprite;

    [SerializeField]
    private int coinValue = 1;
    public override void OnReceived()
    {
        AudioManager.instance.PlaySound(AudioType.SFXCoin);
        GlobalReferences.instance.players[0].GetComponent<PlayerInventory>().IncrementCoinCount(coinValue);
    }
}
