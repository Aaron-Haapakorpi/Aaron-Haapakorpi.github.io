using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(menuName = "Weapons/BasicGun")]
public class BasicGun : Gun
{
    private GameObject b;
    public int poolSize=10;
    public override void Initialize(GameObject playerObj, ObjectPool pool)
    {
        currentClip = clipSize;
        currentReserveAmmo = reserveAmmoSize;
        bullets = pool;
        playerShootScript = playerObj.GetComponent<PlayerShooting>();
        bullets.Initialize(poolSize, bullet);
        if (damage != -1) bullet.GetComponent<Bullet>().damage = damage;
        if (!cam) cam = Camera.main;
    }

    public override void Shoot()
    {
       

            AudioManager.instance.PlaySound(shootSound);
            b = bullets.GetNext();
            if (b.activeSelf == true)
            {
                b = Instantiate(bullet);
                b.SetActive(false);
                bullets.AddObject(b);
            }
            //b.transform.rotation = shoot.transform.rotation; used when player also rotates, doesn't rotate curently
            b.transform.eulerAngles = CalculateDirection(playerShootScript.transform);
            b.transform.position = playerShootScript.weaponRenderer.transform.position + (Vector3)playerShootScript.gunOffSet;
            b.SetActive(true);
            currentClip--;
        
    }


}