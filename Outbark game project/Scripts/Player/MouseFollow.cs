using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Currently unused script, rotates object to look toward mouse position
/// </summary>
public class MouseFollow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int angleOffset = 0;
    // Update is called once per frame
    void Update()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle+angleOffset);
    }
}
