using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// Handles what happens when player takes damages. receiving damage, visual effects, respawn, animation etc.
/// </summary>
public class PlayerDamageable : MonoBehaviour,IDamageable
{
    // Start is called before the first frame update
    public event Action PlayerDeathEvent;
    public event Action PlayerTakeDamageEvent;
    public event Action PlayerHealEvent;
    private SpriteRenderer playerRenderer;
    private SpriteRenderer playerRenderer2;

    [SerializeField]
    private float maxHealth=100;
    [SerializeField]
    private float invulnTime = 1;
    private float invulnTimer= 0;
    private float currentHealth;

    private bool isDead = false;
    void Awake()
    {
        playerRenderer = GetComponent<SpriteRenderer>();
        playerRenderer2 = transform.GetChild(1).GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        PlayerDeathEvent += GetComponent<PlayerController>().RemoveAll;
        PlayerDeathEvent += GetComponent<PlayerAnimation>().PlayDeathExplosion;
        
    }

    private void Start()
    {
        if (GlobalReferences.instance.combatUi != null)
        {
            PlayerTakeDamageEvent += UpdateHealthImage;
            PlayerHealEvent += UpdateHealthImage;
        }
            
    }


    public void Die()
    {
        
        GlobalReferences.instance.deathUI.GetStatistics();
        AudioManager.instance.StopSoundNoOverLap();
        AudioManager.instance.PlaySound(AudioType.SFXPlayerDeath);
        GlobalReferences.instance.deathUI.gameObject.SetActive(true);
        GetComponent<PlayerShooting>().weaponRenderer.sprite = null;
        
        isDead = true;
        PlayerDeathEvent?.Invoke();
        Debug.Log("ded");
    }
    public void Heal(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        PlayerHealEvent?.Invoke();
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;
        if (Time.time < invulnTimer) return;
        invulnTimer = invulnTime + Time.time;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        else
        AudioManager.instance.PlaySound(AudioType.SFXPlayerDamaged);
        BlinkOnDamage(invulnTime);
        PlayerTakeDamageEvent?.Invoke();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
    }

    private void UpdateHealthText()
    {
        GlobalReferences.instance.combatUi.healthCountText.text = currentHealth.ToString();
    }

    private void UpdateHealthImage()
    {
        GlobalReferences.instance.combatUi.healthFillImage.fillAmount = (float)currentHealth/maxHealth;
    }

    private void BlinkOnDamage(float duration)
    {
        StartCoroutine(BlinkRoutine(duration));
    }

    private IEnumerator BlinkRoutine(float duration)
    {
        float timePassed = 0;
        while (timePassed < duration)
        {
            playerRenderer.enabled = false;
            playerRenderer2.enabled = false;
            yield return new WaitForSeconds(0.2f);
            playerRenderer.enabled = true;
            playerRenderer2.enabled =true;
            yield return new WaitForSeconds(0.2f);
            timePassed += 0.4f;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }
}
