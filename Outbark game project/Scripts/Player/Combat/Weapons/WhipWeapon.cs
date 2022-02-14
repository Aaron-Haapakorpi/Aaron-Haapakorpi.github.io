using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// melee whip weapon, unfinished
/// </summary>
public class WhipWeapon : Gun
{
    private GameObject weaponObject;
    public int poolSize = 1;
    public override void Initialize(GameObject obj, ObjectPool pool)
    {
        currentClip = clipSize;
        currentReserveAmmo = reserveAmmoSize;
        bullets = pool;
        playerShootScript = obj.GetComponent<PlayerShooting>();
        if (damage != -1) bullet.GetComponent<Bullet>().damage = damage;
        if (!cam) cam = Camera.main;
    }

    public override void Shoot()
    {
        weaponObject = bullets.GetNext();
        weaponObject.SetActive(true);
        weaponObject.GetComponent<Animator>().SetBool("Attack",true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
