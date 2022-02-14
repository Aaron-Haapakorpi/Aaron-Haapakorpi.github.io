using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// temporary test script, unneeded
/// </summary>
public class RenderTest : MonoBehaviour
{
    SpriteRenderer tempRend; //allows to adjust for sorting within the unit layer, for visuals
    public int offset = 0;
    void Awake()
    {
        //tempRend = GetComponent<SpriteRenderer>();
        Camera.main.transparencySortMode = TransparencySortMode.CustomAxis;
        Camera.main.transparencySortAxis = Camera.main.transform.up;  

    }
    void LateUpdate()
    {
        // tempRend.sortingOrder =offset+ (int)Camera.main.WorldToScreenPoint(tempRend.gameObject.transform.position).y * -1;
        //  Debug.Log((int)Camera.main.WorldToScreenPoint(tempRend.bounds.min).y * -1);
    }
}
