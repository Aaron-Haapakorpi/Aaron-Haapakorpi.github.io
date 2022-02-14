using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// base bullet class, will contain basic bullet behaviour, bullets are set to kinematic, not using real world physics, also need enemy layer for collision with enemies
/// bullet type ideas, homing, bouncing, piercing, could also just set as bool parameters and use a generic bullet script with parameters set for diversity
/// could have scripts that just act as components added instead of booleans, e.g homing, bouncing etc
/// sine wave type bullet?, enemy bullet idea rotate around player
/// </summary>
public abstract class Bullet :MonoBehaviour
{
    // Start is called before the first frame update
    protected Rigidbody2D rb;
    public LayerMask wallLayer;
    public LayerMask damagingLayer;
    public int damage = 2;

    protected bool bulletFading = false;

    [SerializeField]
    protected SpriteRenderer spriterenderer;
    protected Color startColor;
    
    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name + "col");
        //on collision with wall sets bullet inactive, currently uses collision
        if (((1<<collision.gameObject.layer) & wallLayer)!=0)
        {
            CollisionEffect(collision);
            gameObject.SetActive(false);
        }
        else if (((1 << collision.gameObject.layer) & damagingLayer) != 0)
        {
            DamageEnemy(collision);
            CollisionEffect(collision);
            gameObject.SetActive(false);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //on trigger with wall sets bullet inactive
        if (((1 << collision.gameObject.layer) & wallLayer) != 0)
        {
            gameObject.SetActive(false);
        }
        else if (((1 << collision.gameObject.layer) & damagingLayer) != 0)
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            GlobalReferences.instance.InstantiateCombatText(damage.ToString(), transform.position);
            gameObject.SetActive(false);
        }

    }

    protected void CollisionEffect(Collision2D collision)
    {
        var collisionNormal = collision.contacts[0].normal;
        var instance = GlobalReferences.instance.sparkPool.GetNext();
        instance.SetActive(true);
        instance.transform.position = collision.contacts[0].point;
        instance.transform.GetComponent<ParticleSystem>().Play();
        instance.transform.right = (collision.transform.position - transform.position) * -1;
    }

    protected void DamageEnemy(in Collision2D collision)
    {
        collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
        GlobalReferences.instance.InstantiateCombatText(damage.ToString(),transform.position);
    }


    protected IEnumerator FadeDisappear(float aValue, float aTime)
    {
        float alpha = startColor.a;
        Color temp;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            temp = spriterenderer.color;
            temp.a = Mathf.Lerp(alpha, aValue, t);
            spriterenderer.color = temp;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}

