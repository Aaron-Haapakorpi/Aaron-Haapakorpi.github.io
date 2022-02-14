using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Weapons/LaserGun")]
public class LaserWeapon : Gun
{
    // Start is called before the first frame update
    private GameObject b;

    public Transform laserHit;
    public int poolSize = 10;
    public override void Initialize(GameObject obj, ObjectPool pool)
    {
        currentClip = clipSize;
        currentReserveAmmo = reserveAmmoSize;
        bullets = pool;
        playerShootScript = obj.GetComponent<PlayerShooting>();
        bullets.Initialize(poolSize, bullet);
        if (damage != -1) bullet.GetComponent<Bullet>().damage = damage;
        if (!cam) cam = Camera.main;
    }

    public override void Shoot()
    {
        AudioManager.instance.PlaySoundNoOverLap(shootSound);
        b = bullets.GetNext();
        b.transform.eulerAngles = CalculateDirection(playerShootScript.transform);
        b.transform.position = (Vector2)playerShootScript.weaponRenderer.transform.position + playerShootScript.gunOffSet;
        b.SetActive(true);
        currentClip -= Time.deltaTime*10;

    }
}
