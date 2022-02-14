using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// holds information for shooting guns, playerinput handles the input itself
/// </summary>
public class PlayerShooting : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public Gun currentGun;
    public SpriteRenderer weaponRenderer;

    [HideInInspector]
    public Vector2 gunOffSet;
    public Vector2 gunPosition;
    private float cdTimer = 0;

    //maybe should have guns in the inventory instead
    public List<Gun> guns = new List<Gun>();
    public List<GunActiveInformation> gunActiveInfo= new List<GunActiveInformation>();
    public List<Vector3> gunPositionsPerAngle;
    public List<Vector3> gunMuzzlePositionPerAngle;//probably will do this according to pivot position, need new spritesheets first

    public TextMeshProUGUI clipBulletText;
    public TextMeshProUGUI reserveBulletText;

    public ParticleSystem OnShotParticle; //should maybe be on gun script if we want different particles for different guns, also position should be updated based on gun muzzle position
    int currentGunIndex = 0;

    private Camera cam;
    private Coroutine screenshakeroutine;

    [SerializeField]
    private GameObject combatFloatingText;

    new PlayerAnimation animation;

    int previousWeaponSpriteIndex = 0;
    public GunActiveInformation GetCurrentGunInfo()
    {
        return gunActiveInfo[currentGunIndex];
    }
    private void Awake()
    {
        animation = GetComponent<PlayerAnimation>();
        if (!cam) cam = Camera.main;
        if (guns != null)
        {
            clipBulletText.text = Mathf.Round(guns[0].currentClip).ToString();
            //GlobalReferences.instance.combatUi.bulletFillImage.fillAmount = guns[0].currentClip / guns[0].clipSize;
            if (reserveBulletText != null) ChangeBulletText(reserveBulletText, guns[0].currentReserveAmmo);
        }
        
    }
    void Start()
    {
        InitializeFloatingCombatText();
        foreach (var weapon in guns)
        {
            weapon.Initialize(gameObject, gameObject.AddComponent<ObjectPool>());
            GunActiveInformation info = new GunActiveInformation();
            info.gun = weapon;
            gunActiveInfo.Add(info);
        }
        currentGun = guns[0];

    }

    public void InitializeFloatingCombatText()
    {
        var textPool = gameObject.AddComponent<ObjectPool>();
        textPool.Initialize(10, combatFloatingText.gameObject);
        GlobalReferences.instance.damageCombatTextPool = textPool;
    }
    public void ChangeBulletText(in TextMeshProUGUI txt, float bulletCount)
    {
        txt.text = Mathf.Round(bulletCount).ToString();
    }

    public void ChangeImageFill(in Image image, float amount)
    {
        image.fillAmount = amount;
    }

    public void Shoot()
    {
        currentGun.Shoot();
        if (clipBulletText != null) ChangeBulletText(clipBulletText, currentGun.currentClip);
        StartCoroutine(UpdateClipBulletCount(currentGun.currentClip / currentGun.clipSize));
        //if (OnShotParticle != null) OnShotParticle.Play(); //each gun should probably have its own effect, also maybe the bullet should play the effect

        //screenshakeroutine = StartCoroutine(ScreenShake(cam.transform.localPosition,0.1f,0.01f)); //should not really be here, temporary
    }

    public IEnumerator ScreenShake(Vector3 initPos, float shakeDuration, float magnitude)
    {
        float dur = 0;
        while (dur < shakeDuration)
        {
            cam.transform.localPosition = initPos + Random.insideUnitSphere * magnitude;
            dur += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        cam.transform.localPosition = initPos;
    }
    public void PickUpAmmo(float ammo)
    {
        currentGun.currentReserveAmmo += ammo;
        if (currentGun.currentReserveAmmo > currentGun.reserveAmmoSize)
        {
            currentGun.currentReserveAmmo = currentGun.reserveAmmoSize;
        }
        StartCoroutine(UpdateReserveBulletCount(currentGun.currentReserveAmmo / currentGun.reserveAmmoSize));
    }
    public void Reload()
    {
        currentGun.Reload();
        if (clipBulletText == null) return;
        //ChangeBulletText(clipBulletText, currentGun.currentClip);
       // ChangeImageFill(GlobalReferences.instance.combatUi.bulletFillImage, currentGun.currentClip / currentGun.clipSize);
        ChangeBulletText(reserveBulletText, currentGun.currentReserveAmmo);
       // ChangeImageFill(GlobalReferences.instance.combatUi.reserveBulletFillImage, currentGun.currentReserveAmmo / currentGun.reserveAmmoSize);
        StartCoroutine(UpdateClipBulletCount(currentGun.currentClip / currentGun.clipSize));
        StartCoroutine(UpdateReserveBulletCount(currentGun.currentReserveAmmo / currentGun.reserveAmmoSize));
    }
    private void Update()
    {
        for(int i= 0; i< gunActiveInfo.Count; i++)
        {
            gunActiveInfo[i].currentCooldownTimer += Time.deltaTime;

        }
    }

    public void UpdateWeaponSpriteMouse()
    {
        var dir = CalculateWeaponDir();
        int ind = CalculateRotation(dir);

        var angle = ((ind) * 45) * Mathf.Deg2Rad;
        var pos = StaticFunctions.PositionOnUnitCircle(angle);
        gunOffSet = pos*0.5f;
        if (currentGun.weaponSprites.Count >= ind + 1)
        {
            weaponRenderer.sprite = currentGun.weaponSprites[ind];
            previousWeaponSpriteIndex = ind;

        }
    }

    public void UpdateWeaponSpriteGamepad(Vector2 dir)
    {
        int ind = CalculateRotation(dir);
        gunOffSet = dir.normalized * 0.4f;
        if (currentGun.weaponSprites.Count >= ind + 1)
        {
            weaponRenderer.sprite = currentGun.weaponSprites[ind];
            previousWeaponSpriteIndex = ind;

        }
    }
    public void SwapWeapon()
    {
        currentGunIndex += 1;

        if (currentGunIndex >= guns.Count)
        {
            currentGunIndex = 0;
        }
        currentGun = guns[currentGunIndex];
        weaponRenderer.sprite = currentGun.weaponSprites[previousWeaponSpriteIndex];

        ChangeImageFill(GlobalReferences.instance.combatUi.reserveBulletFillImage, currentGun.currentReserveAmmo / currentGun.reserveAmmoSize);
        ChangeImageFill(GlobalReferences.instance.combatUi.bulletFillImage, currentGun.currentClip / currentGun.clipSize);
        GlobalReferences.instance.combatUi.weaponUIImage.sprite = currentGun.UISprite;
    }

    public void OnNewGunReceived(Gun gun)
    {
        gun.Initialize(gameObject, gameObject.AddComponent<ObjectPool>());
        guns.Add(gun);
        GunActiveInformation info = new GunActiveInformation();
        info.gun = gun;
        gunActiveInfo.Add(info);
    }

    /// <summary>
    /// 0= up, 7 = top left
    /// </summary>
    /// <param name="ang"></param>
    /// <returns></returns>
    public int CalculateRotation(Vector2 ang)
    {
        var angle = Mathf.Atan2(ang.x, ang.y) * Mathf.Rad2Deg;
        int dir = 0;
        if (angle > -22.5 && angle < 22.5) dir = 0;
        else if (angle > 22.5 && angle < 67.5) dir = 1;
        else if (angle > 67.5 && angle < 112.5) dir = 2;
        else if (angle > 112.5 && angle < 157.5) dir = 3;
        else if (angle > 157.5 || angle < -157.5) dir = 4;
        else if (angle > -157.5 && angle < -112.5) dir = 5;
        else if (angle > -112.5 && angle < -67.5) dir = 6;
        else if (angle > -67.5 && angle < -22.5) dir = 7;
        return dir;

    }

    public Vector2 CalculateWeaponDir()
    {
        Vector3 mouse = Input.mousePosition;
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.localPosition);
        Vector3 offset = new Vector2(mouse.x - screenPoint.x, mouse.y - screenPoint.y);

        return offset;

    }

    public Vector3 GetNewWeaponPosition(int rotation)
    {
        Vector3 test = Vector3.zero;
        switch(rotation){
            case 0:
                test.x = -0.1f;
                test.y = 0.4f;
                break;
            case 1:
                test.x = -0.15f;
                test.y = 0.4f;
                break;
            case 2:
                test.x = 0.1f;
                test.y = 0.4f;
                break;
            case 3:
                test.x = 0;
                test.y = 0.35f;
                break;
            case 4:
                test.x = -0.1f;
                test.y = 0.3f;
                break;
            case 5:
                test.x = -0.1f;
                test.y = 0.4f;
                break;
            case 6:
                test.x = -0.05f;
                test.y = 0.45f;
                break;
            case 7:
                test.x = 0.2f;
                test.y = 0.45f;
                break;
        }
        return test;
    }

    public IEnumerator UpdateClipBulletCount(float target)
    {
        Image img = GlobalReferences.instance.combatUi.bulletFillImage;
        float currentFill = img.fillAmount;
        float time = 0;

        while(time < 0.5f)
        {
            img.fillAmount = (Mathf.Lerp(img.fillAmount, target, time/0.5f));
            time += Time.deltaTime;
            yield return null;
        }
      
    }

    public IEnumerator UpdateReserveBulletCount(float target)
    {
        Image img = GlobalReferences.instance.combatUi.reserveBulletFillImage;
        float currentFill = img.fillAmount;
        float time = 0;

        while (time < 0.5f)
        {
            img.fillAmount = (Mathf.Lerp(img.fillAmount, target, time / 0.5f));
            time += Time.deltaTime;
            yield return null;
        }
    }
}

//might add total bullets etc here also if it ends up necessary
public class GunActiveInformation
{
    public Gun gun;
    public float currentCooldownTimer = 0;
}
