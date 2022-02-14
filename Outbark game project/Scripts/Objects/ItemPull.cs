using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// coin script that moves towards player when player is in range and increments player coin count on entering collider
/// could make it generic so you specify item and call onitempurchased
/// </summary>
public class ItemPull : MonoBehaviour
{
    [SerializeField]
    float range = 6;
    GameObject target;
    public LayerMask playerMask;
    public float pullSpeed = 8; //accelerate instead?
 
    [SerializeField]
    Item item; //might make an coin item script so same script can be used for other items

    private void Update()
    {
        var checkOverlap = Physics2D.OverlapCircle(transform.position, range,playerMask);
        if (checkOverlap)
        {
            target = checkOverlap.gameObject;
        }
        else
        {
            target = null;
        }
        if (target != null)
        {
            float step = Time.deltaTime * pullSpeed;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.gameObject.CompareTag("Player"))
        {
            //Debug.Log("coin increment");
            //collision.transform.gameObject.GetComponent<PlayerInventory>().IncrementCoinCount(1);
            item.OnReceived();
    
            //Destroy(gameObject); //might have a pooling solution instead
            gameObject.SetActive(false);
        }
    }
}
