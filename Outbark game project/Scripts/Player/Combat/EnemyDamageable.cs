using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// handles what happens when an enemy takes damage
/// </summary>

public class EnemyDamageable : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    public event Action EnemyDeathEvent;
    public event Action EnemyTakeDamageEvent;

    [SerializeField]
    private float maxHealth = 100;
    [SerializeField]
    private float invulnTime = 1;
    private float invulnTimer = 0;
    [SerializeField]
    private float currentHealth;
    void Awake()
    {
        currentHealth = maxHealth;
    }

    /// <summary>
    /// Assumes enemies are used with an object pool, sets gameobject to false on death
    /// </summary>
    public void Die()
    {
        EnemyDeathEvent?.Invoke();
        gameObject.SetActive(false);
    }
    public void TakeDamage(float damage)
    {
        if (Time.time<invulnTimer) return;
        invulnTimer = invulnTime+Time.time;
        EnemyTakeDamageEvent?.Invoke();
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }
    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }
}
