using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can receive damage, seperate implementation for player/'s and enemies
/// </summary>
public interface IDamageable
{
    void Die();

    void TakeDamage(float damage);
}
