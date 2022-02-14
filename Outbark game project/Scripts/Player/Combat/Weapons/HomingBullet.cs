using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// contains specific behaviour for a homing bullet, needs an assigned target to work, e.g. closest enemy, player used for testing purposes
/// 
/// </summary>
public class HomingBullet : Bullet
{

    [HideInInspector]
    public Transform target;
    public float speed = 5;
    public float rotationSpeed = 4;

    public float drag = 0;


    private float currentSpeed=5;

    Quaternion q;
    Vector3 dir;
    private void Awake()
    {
        currentSpeed = speed;
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }

    private void OnEnable()
    {
        currentSpeed = speed;
        rb.velocity = transform.right.normalized * speed;
    }

    /// <summary>
    /// rotation and movement happens in fixedupdate
    /// </summary>
    private void FixedUpdate()
    {
        //working rotation towards a target
        dir = target.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
       // transform.rotation = Quaternion.Slerp(transform.rotation, q, rotationSpeed * Time.deltaTime); //lerp or slerp?
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, q, rotationSpeed * Time.fixedDeltaTime));
        rb.velocity = transform.right.normalized * currentSpeed;

        //drag testing, should work
        if (drag == 0) return;
        var testDrag = (1 - Time.deltaTime * drag);
        currentSpeed *= testDrag;
        if (currentSpeed < 0.5f) gameObject.SetActive(false);
    }
    private void OnDrawGizmos()
    {
        if (!target) return;
        Debug.DrawRay(transform.position, target.transform.position - transform.position, Color.red);
        Debug.DrawRay(transform.position, transform.right, Color.red);
    }

}
