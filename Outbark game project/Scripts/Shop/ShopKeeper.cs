using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// shop character
/// </summary>
public class ShopKeeper : MonoBehaviour,IInteractable
{
    public List<Purchase> purchaseList = new List<Purchase>();
    public PurchasePool purchasePool;
    public int numberOfPurchases = 3;
    bool initialized = false;

    private List<int> listRandomizer = new List<int>();

    private void Awake()
    {
        if (numberOfPurchases > 3) numberOfPurchases = 3;
    }

    private void PickPurchasesFromPool(in PlayerController interactor)
    {
        purchaseList.Clear();

        var inventory = interactor.GetComponent<PlayerInventory>();
        for (int i = 0; i < purchasePool.purchasePool.Count; i++)
        {
            if (!inventory.ContainsItem(purchasePool.purchasePool[i].item)){
                purchasePool.currentPurchasePool.Add(purchasePool.purchasePool[i]);
            }
            
        }
        for(int i=0; i < purchasePool.currentPurchasePool.Count; i++)
        {
            listRandomizer.Add(i);
        }
        if (numberOfPurchases > purchasePool.currentPurchasePool.Count)
        {
            numberOfPurchases = purchasePool.currentPurchasePool.Count;
        }
        for (int i = 0; i < numberOfPurchases; i++)
        {
            int rNumber = Random.Range(0, listRandomizer.Count-i);
            purchaseList.Add(purchasePool.currentPurchasePool[i]);
            listRandomizer.Remove(rNumber);
        }
    }

    private void UpdateReceivedItems(in PlayerController interactor)
    {
        for (int i = 0; i < purchaseList.Count; i++)
        {
            if (interactor.GetComponent<PlayerInventory>().ContainsItem(purchaseList[i].item))
            {
                purchaseList.RemoveAt(i);
                i -= 1;
            }
        }
    }
    public void Interact(PlayerController interactor)
    {
        if (!initialized)
        {
            purchasePool.currentPurchasePool.Clear();
            PickPurchasesFromPool(interactor);
            initialized = true;
        }
        else
        {
            UpdateReceivedItems(interactor);
        }
        GlobalReferences.instance.shopMenu.ToggleShopMenu();
        if (Time.timeScale == 1) return;
        GlobalReferences.instance.shopMenu.gameObject.SetActive(true);
        StaticFunctions.CopyList(ref purchaseList, ref GlobalReferences.instance.shopMenu.purchaseList);
        GlobalReferences.instance.shopMenu.ClearShopUI();
        GlobalReferences.instance.shopMenu.InitializeShop();    
        
    }
}
