using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// test script for basic chest loot, currently just calls the item's onpurchased method
/// probably will have some animation for it etc for opening a chest, might spawn an item instead
/// </summary>
public class LootChest : MonoBehaviour, IInteractable
{
    //could have different rarities of loot etc
    public List<Item> lootPossibilities;
    public List<ChestGraphic> chestGraphicPossibilities;
    private bool opened = false;
    public bool locked = false;
    new SpriteRenderer renderer;


    [SerializeField]
    private GameObject defaultLootPrefab;

    private int chestGraphicIndex = -1;
    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        try
        {
            int randomNumber = Random.Range(0, chestGraphicPossibilities.Count);
            renderer.sprite = chestGraphicPossibilities[randomNumber].closed;
            chestGraphicIndex = randomNumber; 
        }
        catch
        {
            Debug.LogError("no chest sprites assigned");
        }
    }
    public void Interact(PlayerController interactor)
    {
        if (opened) return;
        if (locked)
        {
            if (interactor.GetComponent<PlayerInventory>().GetChestKeyCount() > 0)
            {
                interactor.GetComponent<PlayerInventory>().RemoveChestKey();
            }
            else return;
        }
        renderer.sprite = chestGraphicPossibilities[chestGraphicIndex].open;
        opened = true;
        if (lootPossibilities != null)
        {

            var playerInventory = interactor.GetComponent<PlayerInventory>();
            foreach(var possibility in lootPossibilities)
            {
                if (playerInventory.ContainsItem(possibility))
                {
                    lootPossibilities.Remove(possibility);
                }
            }

            int randomNumber = Random.Range(0, lootPossibilities.Count - 1);
            SpawnLoot(lootPossibilities[randomNumber]);
        }
    }

    private void SpawnLoot(Item item)
    {
        var spawnedLoot = Instantiate(defaultLootPrefab);
        spawnedLoot.GetComponent<LootInteractable>().SetItem(item);
        if(item.itemSprite!=null) spawnedLoot.GetComponent<SpriteRenderer>().sprite = item.itemSprite;
        spawnedLoot.transform.position = transform.position;
        spawnedLoot.transform.position += new Vector3(-0.3f, 0.1f, 0);
        gameObject.layer = 0;

    }

    
}
[System.Serializable]
public class ChestGraphic
{
    public Sprite open;
    public Sprite closed;
}
