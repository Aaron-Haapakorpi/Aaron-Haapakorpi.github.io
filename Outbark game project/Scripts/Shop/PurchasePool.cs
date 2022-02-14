using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Items/PurchasePool")]
public class PurchasePool : ScriptableObject
{
    public List<Purchase> purchasePool;

    //currently not using this but probably should be
    public List<Purchase> currentPurchasePool;

    public void RemovePurchaseFromPool(Purchase purchase)
    {
        currentPurchasePool.Remove(purchase);
    }

    public void RemovePurchaseFromPool(int index)
    {
        currentPurchasePool.RemoveAt(index);
    }
    

}
