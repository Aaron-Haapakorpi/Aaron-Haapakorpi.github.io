using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// contains behaviour for a basic bullet with multiple modifiers so can be modified for different weapons. more specialized bullets will be in their own scripts
/// </summary>
public class BasicBullet : Bullet
{
   
    public float speed = 10;
    public float drag = 0; 
    public bool bounces = false;
    public bool piercing = false;
    private int timesPierced = 0;
    public int pierceMax = 4;

    private Vector2 test;

    private int bounceTimer = 4;
    private int waitFixedSteps = 4;

    private float currentSpeed = 5;

    public int angleVariance = 30;

    private float elapsedTime = 0;

    //maybe a curved bullet property?

    [SerializeField]
    float fadeTimer=0.5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
        if (spriterenderer == null) spriterenderer = GetComponent <SpriteRenderer>();
        startColor = spriterenderer.color;
    }

    private void OnEnable()
    {
        var q = Quaternion.AngleAxis(Random.Range(-angleVariance, angleVariance), Vector3.forward);
        transform.rotation *= q;
        currentSpeed = speed;
        rb.velocity = transform.right.normalized * speed;
        timesPierced = 0;
        bulletFading = false;
        spriterenderer.color = startColor;
        bounceTimer = waitFixedSteps;
        
        // rb.AddForce(transform.right.normalized * speed,ForceMode2D.Impulse)
    }
    
    private void FixedUpdate()
    {
        bounceTimer++;
        if (drag == 0) return;
        var testDrag = (1 - Time.deltaTime * drag);
        rb.velocity *= testDrag;
        currentSpeed *= testDrag;
        if (currentSpeed < 0.5f &&!bulletFading)
        {
            bulletFading = true;
            elapsedTime = 0;
            
        }
        //other way to do it
        /* if (drag == 0) return;
        rb.velocity = rb.velocity.normalized * currentSpeed;
        var testDrag = (1 - Time.deltaTime * drag);
        currentSpeed *= testDrag;
        if (currentSpeed < 0.5f &&!bulletFading)
        {
            bulletFading = true;
            elapsedTime = 0;
            
        }
        */
    }

    private void Update()
    {
        if (bulletFading)
        {   
            Color temp;
            temp = spriterenderer.color;
            temp.a = Mathf.Lerp(startColor.a, 0, elapsedTime/fadeTimer);
            spriterenderer.color = temp;
            elapsedTime += Time.deltaTime;
            if (elapsedTime > fadeTimer)
            {
                bulletFading = false;
                gameObject.SetActive(false);
            }
        }
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (!bounces)
        {
            base.OnCollisionEnter2D(collision);
            return;
        }

        ;
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            CollisionEffect(collision);
            if (bounceTimer < waitFixedSteps) return;
            bounceTimer = 0;
            Bounce(collision);
        }
        else if (((1 << collision.gameObject.layer) & damagingLayer) != 0)
        {
            DamageEnemy(collision);
            if (piercing)
            {
                timesPierced++;
                if (timesPierced <= pierceMax)
                {
                    return;
                } 
            }
            CollisionEffect(collision);

            gameObject.SetActive(false);
        }
    }
   
    /// <summary>
    /// Wall bouncing bullets, does not slow down bullets currently but can be added e.g. also should probably do the bouncing purely with raycasts because collision point produces errors
    /// </summary>
    /// <param name="collision"></param>
    public void Bounce(in Collision2D collision)
    {
     
        var l = collision.contacts[0];
        //collision contact not the same as surface normal
        RaycastHit2D hit = Physics2D.Raycast(l.point - rb.velocity.normalized, rb.velocity.normalized, 1.2f, wallLayer);

        if (hit)
        {
            AudioManager.instance.PlaySound(AudioType.SFXBasicBounce);
            var dir = Vector2.Reflect(rb.velocity.normalized, hit.normal);
            rb.velocity = dir * rb.velocity.magnitude;
            Debug.DrawRay(l.point, hit.normal, Color.black, 6f);
            test = hit.point;

        }
        else
        {
            AudioManager.instance.PlaySound(AudioType.SFXBasicBounce);
            Debug.LogError("some issue with raycasting to wall");
            Debug.DrawRay(l.point - rb.velocity.normalized, rb.velocity.normalized*1.2f, Color.cyan, 10f);   
            rb.velocity = Vector2.Reflect(rb.velocity.normalized, l.normal);
        }
    }
    private void OnDrawGizmos()
    {
        if (test != null) Gizmos.DrawWireSphere(test, 0.05f);
    }

}
