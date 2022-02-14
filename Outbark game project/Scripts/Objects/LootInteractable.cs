using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootInteractable : MonoBehaviour,IInteractable
{
    private Item item;
    public GameObject lootPopUpPrefab;

    public void SetItem(in Item item)
    {
        this.item = item;
    }

    public void Interact(PlayerController interactor)
    {
        item.OnReceived();
        var g = Instantiate(lootPopUpPrefab);
        g.GetComponent<LootReceivePopup>().SetText(item.itemName);
        Destroy(gameObject);
    }
}
