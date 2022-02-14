using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Purchase object for shop system, contans price, item, possibly description and a sprite to display
/// </summary>
[CreateAssetMenu(menuName = "Items/Purchase")]
public class Purchase : ScriptableObject
{
    public Item item;
    public int price;
    public Sprite purchaseLogo;
    public string description;
}
