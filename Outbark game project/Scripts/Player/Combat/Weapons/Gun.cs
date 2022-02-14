using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// abstract scriptableobject gun, holds generic properties a gun has, sprites, cooldown, clip size etc
/// </summary>
public abstract class Gun : Item //changed to inherit from item
{
    public float coolDown;
    public float reloadTime = 1;
    public AudioType impactSound;
    public AudioType shootSound;
    public AudioType reloadSound;
    public List<Sprite> weaponSprites = new List<Sprite>();

    
    public Sprite UISprite;

    public float clipSize; //need floats for weapons with constant dmg so can -time.deltatime, prob okay for ints as well?
    public float reserveAmmoSize;

    public int damage = -1; //overrides damage ste in bullet
    public GameObject bullet;
    protected ObjectPool bullets;

    [HideInInspector]
    public Vector2 gunMuzzle = Vector2.zero;

    
    public float currentClip;
    public float currentReserveAmmo;

    public abstract void Initialize(GameObject obj, ObjectPool pool);
    public abstract void Shoot();

    protected Camera cam;
    protected PlayerShooting playerShootScript;

    public Vector3 reticlePosition;
    

    public void Reload()
    {
        if (currentReserveAmmo >= clipSize)
        {
            currentReserveAmmo -= clipSize - currentClip;
            currentClip = clipSize;
        }
        else
        {
            currentClip = currentReserveAmmo;
            currentReserveAmmo = 0;
        }
    }

    public Vector3 CalculateDirection(in Transform transform)
    {
        return reticlePosition;
    }

    public Vector3 CalculateDirectionMouse(Vector3 mousePos)
    {
        Vector3 mouse = mousePos; //used to be input.mouse position, but using new input system now
        Vector3 screenPoint = cam.WorldToScreenPoint(playerShootScript.transform.localPosition);
        Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, angle);
    }

    //maybe a charge time variable?

    //not an instance of a scriptable object, aka same weapon same ammo, can change e.g. by creating instances of them,
    //or creating a struct/class for each weapon that holds the current ammo etc and gets max values from the weapon itself, probably makes the most sense
    //will only change if necessary, e.g. multiple players or multiple instances of the same gun
    public override void OnReceived()
    {
        //not referenced, doesn't work, probably should access with a method parameter to inventory and have guns in inventory also, using a global reference for now
        //playerShootScript.guns.Add(this);
        var p = GlobalReferences.instance.players[0];
        p.GetComponent<PlayerShooting>().OnNewGunReceived(this);
        p.GetComponent<PlayerInventory>().AddItem(this);
    }
}
