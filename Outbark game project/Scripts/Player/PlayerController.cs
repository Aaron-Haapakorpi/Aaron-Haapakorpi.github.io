using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

/// <summary>
/// Handles player input, including movement and abilities, certain inputs can be disabled or enabled by configuring the allowedInputs event
/// </summary>
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    PlayerMovement movement;
    PlayerShooting shooting;

   const string Dodge = "Jump";

    public delegate void AllowedInputs();
    public event AllowedInputs allowedInputs;

    [SerializeField]
    private float dodgeInputCooldown = 2f;
    private float dodgeCDTimer = 0;

    public Vector2 directionalInput;

    private float shootCDTimer = 0;

    bool swappingWeapon = false;
    [SerializeField]
    private float SwapInputCoolDown = 0.5f;

    private PlayerInput inputActions;

    Coroutine shootRoutine;
    [SerializeField]
    PauseMenu pauseMenu;

    Coroutine reloadRoutine;
    bool reloading = false;
    float reloadTimer = 0;

    [SerializeField]
    LayerMask interactableMask;


    //testing outline material swapping
    public Material interactOutlineMaterial;
    [HideInInspector]
    public Collider2D lastMatCollider;
    [HideInInspector]
    public Material lastMat;
    //testing outline material swapping

    private PlayerInventory inventory;

    private void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
        inputActions = new PlayerInput();
        movement = GetComponent<PlayerMovement>();
        shooting = GetComponent<PlayerShooting>();
        pauseMenu.onPause += RemoveAll;
        pauseMenu.onContinue += AllowAll;

    }
    private void Start()
    {
        //test
        GlobalReferences.instance.players.Add(gameObject);
        Debug.Log("setting actions");
        inputActions.Enable();
        inputActions.Standard.Shoot.performed += OnShoot;

        inputActions.Standard.Movement.performed += OnMovement;
        inputActions.Standard.MouseAim.performed += OnMouseAim;
        inputActions.Standard.AnalogAim.performed += OnAnalogAim;
        inputActions.Standard.SwapWeapon.performed += OnSwapWeapon;
        inputActions.UI.Pause.performed += OnPause;
        inputActions.Standard.Reload.performed += OnReload;
        inputActions.Standard.Interact.performed += OnInteract;

    }
    void OnDestroy()
    {
        inputActions.Dispose();
    }

    private void OnEnable()
    {
    }
    /// <summary>
    /// needs modification, also should disable shoot input while reloading
    /// </summary>
    /// <param name="context"></param>
    public void OnReload(InputAction.CallbackContext context) //not yet implemented
    {
        if (reloading) return;
        if (reloadRoutine != null)
        {
            StopCoroutine(reloadRoutine);
        }
        AudioManager.instance.PlaySound(shooting.currentGun.reloadSound);
        reloadRoutine = StartCoroutine(ReloadAfter(shooting.currentGun.reloadTime, shooting.Reload));
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0.1f)
        {
            GlobalReferences.instance.pauseMenu.TogglePauseMenu();
        }
    }
    private void MovementEnd(InputAction.CallbackContext context)
    {
        directionalInput = Vector2.zero;
    }

    public void AllowAll()
    {
        inputActions.Standard.Enable();
    }
    public void RemoveAll()
    {
        Debug.Log("disabling actions");
        StopAllCoroutines();
        inputActions.Standard.Disable();
        directionalInput = Vector2.zero;
        if(shootRoutine!=null)StopCoroutine(shootRoutine);
        SetMovementVector();
    }

    public void RemoveCombatActions()
    {
        directionalInput = Vector2.zero;
        SetMovementVector();
        inputActions.Standard.Shoot.Disable();

        inputActions.Standard.Movement.Disable();
        inputActions.Standard.MouseAim.Disable();
        inputActions.Standard.AnalogAim.Disable();
        inputActions.Standard.SwapWeapon.Disable();
        inputActions.Standard.Reload.Disable();
    }

    public void EnableCombatActions()
    {
        inputActions.Standard.Shoot.Enable();

        inputActions.Standard.Movement.Enable();
        inputActions.Standard.MouseAim.Enable();
        inputActions.Standard.AnalogAim.Enable();
        inputActions.Standard.SwapWeapon.Enable();
        inputActions.Standard.Reload.Enable();
    }

    
    /// <summary>
    /// Sets the player movement script's directional input to current directional input
    /// </summary>
    public void SetMovementVector()
    {
        movement.SetDirectionalInput(directionalInput);
    }

    /// <summary>
    /// Calls player movement script's dodge roll function that handles the movement
    /// currently unused
    /// </summary>
    public void DodgeRollInput()
    {
        if (Input.GetButtonDown(Dodge) && (directionalInput.x != 0 || directionalInput.y != 0))
        {
            if (0 >= dodgeCDTimer)
            {
                dodgeCDTimer = dodgeInputCooldown;
                allowedInputs = null;
                directionalInput.Normalize();
                Debug.Log("roll");
                movement.DodgeRollInit(directionalInput);
            } 
        }
    }
    private void InputTest(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        Debug.Log(value);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        float value = context.ReadValue<float>();
        if (value > 0.05f)
        {
            var hitList = Physics2D.OverlapCircleAll(transform.position, 2, interactableMask);
            if (hitList.Length != 0)
            {
                float smallestDistance = float.MaxValue;
                Collider2D smallestHit = null;

                for (int i = 0; i < hitList.Length; i++)
                {
                    float hitDistance = Vector3.Distance(transform.position, hitList[i].transform.position);
                    if (hitDistance < smallestDistance)
                    {
                        smallestDistance = hitDistance;
                        smallestHit = hitList[i];
                    }
                }
                Debug.Log("interaction occurred");
                smallestHit.transform.GetComponent<IInteractable>().Interact(this);

            }
        }
    }

    //maybe need a onshoot input up method to stop routine or use update instead of coroutine?
    private void OnShoot(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
            float value = context.ReadValue<float>();
        if (value>=0.5f)
        {
            if (shooting.GetCurrentGunInfo().currentCooldownTimer>=shooting.currentGun.coolDown* (1- inventory.statModifiers.attackSpeedIncreasePercentage))
            {
                // var value = context.ReadValue<float>();   
                shooting.GetCurrentGunInfo().currentCooldownTimer = 0;

                shootRoutine= StartCoroutine(RepeatShoot(shooting.currentGun.coolDown));
            }
        }
        else
        {
            AudioManager.instance.StopSoundNoOverLap();
            if(shootRoutine!=null)StopCoroutine(shootRoutine);
        }
        
    }
    IEnumerator RepeatShoot(float cd)
    {
        while (true)
        {
            if (reloading) yield return new WaitForEndOfFrame();
            else if (shooting.currentGun.currentClip > 0)
            {
                shooting.Shoot();
                shooting.GetCurrentGunInfo().currentCooldownTimer = 0;
                if (shooting.currentGun.currentClip == 0) yield return new WaitForEndOfFrame();
                else yield return new WaitForSeconds(shooting.currentGun.coolDown * (1 - inventory.statModifiers.attackSpeedIncreasePercentage));

            }
            else if (!reloading)
            {
                yield return StartCoroutine(ReloadAfter(shooting.currentGun.reloadTime, shooting.Reload)); //gun should handle reloading timer instead
                shooting.GetCurrentGunInfo().currentCooldownTimer = shooting.currentGun.coolDown;
            }
            else yield return new WaitForEndOfFrame();  
        }
    }
    private void OnMovement(InputAction.CallbackContext value) //InputValue value for usage with player input send messages thing
    {
        directionalInput = value.ReadValue<Vector2>(); 
        SetMovementVector();
    }

    private void OnMouseAim(InputAction.CallbackContext value){
        var val = value.ReadValue<Vector2>();
        shooting.UpdateWeaponSpriteMouse();
        shooting.currentGun.reticlePosition = shooting.currentGun.CalculateDirectionMouse(val);//should only calculate when actually shooting but left it for now
    }


    //probably want visual feedback for aiming
    private void OnAnalogAim(InputAction.CallbackContext value)
    {
        var val = value.ReadValue<Vector2>();
        if (val == Vector2.zero) return;
        val.Normalize();
        shooting.UpdateWeaponSpriteGamepad(val);
        var angle = Mathf.Atan2(val.y, val.x) * Mathf.Rad2Deg;
        shooting.currentGun.reticlePosition = new Vector3(0, 0, angle);
    }

    private void OnSwapWeapon(InputAction.CallbackContext value)
    {
        if (swappingWeapon) return;
        var val = value.ReadValue<float>();
        if(val >= 0.05f)
        {
            shooting.SwapWeapon();
            StartCoroutine(WaitForSwapTime(SwapInputCoolDown));
        }
    }
    IEnumerator ReloadAfter(float time, Action a)
    {
        reloading = true;
        reloadTimer = 0;
        if (inventory != null)
            yield return new WaitForSeconds(time * (1 - inventory.statModifiers.reloadSpeedReductionPercentage));

        else yield return new WaitForSeconds(time);
        a.Invoke();
        reloading = false;
    }
    IEnumerator WaitForSwapTime(float time)
    {
        swappingWeapon = true;
        yield return new WaitForSeconds(time);
        swappingWeapon = false;
    }
    /// <summary>
    /// calculates direction based on vector2 input, up = 0, top left = 7, clockwise 
    /// </summary>
    /// <param name="inp"></param>
    /// <returns></returns>
    public int CalculateDirection(Vector2 inp)
    {
        var angle = Mathf.Atan2(inp.x, inp.y) * Mathf.Rad2Deg;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 2); //visual interactable area
    }

    private void Update()
    {
        reloadTimer += Time.deltaTime;
        UpdateGunImageCooldown(shooting.currentGun.coolDown, shooting.GetCurrentGunInfo().currentCooldownTimer);

        InteractionOutlineHighlight();
    }

    private void InteractionOutlineHighlight()
    {
        if (lastMatCollider != null)
        {
            ReturnInteractableMaterial(lastMatCollider, lastMat);
            lastMatCollider = null;
        }
        var hitList = Physics2D.OverlapCircleAll(transform.position, 2, interactableMask);
        if (hitList.Length!=0)
        {
            float smallestDistance = float.MaxValue;
            Collider2D smallestHit = null;

            for (int i = 0; i < hitList.Length; i++)
            {
                float hitDistance = Vector3.Distance(transform.position, hitList[i].transform.position);
                if (hitDistance < smallestDistance)
                {
                    smallestDistance = hitDistance;
                    smallestHit = hitList[i];
                }
            }

            lastMatCollider = smallestHit;
            lastMat = smallestHit.GetComponent<SpriteRenderer>().material;
            //interactOutlineMaterial.color = lastMat.color;
            smallestHit.GetComponent<SpriteRenderer>().material = interactOutlineMaterial;
        }
    }

    private void ReturnInteractableMaterial(Collider2D interactable, Material originalMat)
    {
        interactable.GetComponent<SpriteRenderer>().material = originalMat;
    }
    private void UpdateGunImageCooldown(float weaponCooldown, float currentTimer)
    {
        if (GlobalReferences.instance.combatUi != null)
        {
            if (reloading)
            {
                var fillValue = reloadTimer * (1 - inventory.statModifiers.reloadSpeedReductionPercentage) / shooting.GetCurrentGunInfo().gun.reloadTime;
                var remapChange = (1 - fillValue) * 0.17f;
                fillValue += remapChange;
                GlobalReferences.instance.combatUi.weaponCooldownImage.fillAmount = fillValue;

            }
            else if (weaponCooldown - currentTimer >= 0)
            {
                var fillValue = GlobalReferences.instance.combatUi.weaponCooldownImage.fillAmount = currentTimer / weaponCooldown;
                var remapChange = (1 - fillValue) * 0.17f;
                fillValue += remapChange;
                GlobalReferences.instance.combatUi.weaponCooldownImage.fillAmount = fillValue;
            }
            else GlobalReferences.instance.combatUi.weaponCooldownImage.fillAmount = 1;
        }
    }
}
