
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/ShotGun")]
public class ShotGun : Gun
{
    
   
    private GameObject b;

    public float angleBetweenShots = 10;
    public float bulletsPerShot = 5;
    public int poolSize = 10;
    private void Awake()
    {
        bullet.GetComponent<Bullet>().damage = damage;
    }
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
        float currentAngle = (bulletsPerShot * angleBetweenShots)/2;

        var shootAngles = CalculateDirection(playerShootScript.transform);
        for(int i=0; i < bulletsPerShot;i++)
        {
            b = bullets.GetNext();
            if (b.activeSelf == true)
            {
                b = Instantiate(bullet);
                b.SetActive(false);
                bullets.AddObject(b);
            }
            b.transform.eulerAngles = shootAngles;
            b.transform.rotation *= Quaternion.Euler(0, 0, currentAngle);
            b.transform.position = playerShootScript.weaponRenderer.transform.position + (Vector3)playerShootScript.gunOffSet;
            b.SetActive(true);
            currentAngle -= angleBetweenShots;
        }
        currentClip--;
    }


}