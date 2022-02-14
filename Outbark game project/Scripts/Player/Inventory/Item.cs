using UnityEngine;
/// <summary>
/// items are scriptable objects since guns also are, guns count as items
/// </summary>
public abstract class Item:ScriptableObject
{
    public string itemName;
    public Sprite itemSprite;
    //maybe better to name e.g. OnPickUp, might have other ways of obtaining
    public virtual void OnReceived(){}
}
