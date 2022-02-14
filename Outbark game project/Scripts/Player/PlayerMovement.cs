using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles player movement. uses an event to determine current active movements, e.g. dodge
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;
    [SerializeField]
    float xSmoothTime = 0.1f;
    [SerializeField]
    float ySmoothTime = 0.1f;

    public float dodgeSpeed = 3f;
    public float dodgeDuration = 0.3f;

    Rigidbody2D rb;
    float velocityXSmoothing;
    float velocityYSmoothing;
    float targetX;
    float targetY;
    PlayerController input;
    [SerializeField]
    float dodgeSmoothTime = 0.05f;
    [HideInInspector]
    public Vector2 directionalInput;


    public delegate void CurrentMovements();
    public event CurrentMovements currentMovements;

    PlayerInventory inventory;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerController>();
        currentMovements += HandleMovementVelocity;
        inventory = GetComponent<PlayerInventory>();
    }
    private void FixedUpdate()
    {
        if (currentMovements != null) currentMovements.Invoke();
    }
    /// <summary>
    /// handles player velocity change based on input
    /// </summary>
    private void HandleMovementVelocity()
    {
       // directionalInput.Normalize(); //is this necessary? forgot, if removed, controller works as expected, though animation speed should be adjusted then based on speed or maybe get another animation for walking
        float targetX = directionalInput.x * (speed+inventory.statModifiers.movementSpeedIncrease) * (1+inventory.statModifiers.movementSpeedIncreasePercentage);
        float targetY = directionalInput.y * (speed + inventory.statModifiers.movementSpeedIncrease) * (1 + inventory.statModifiers.movementSpeedIncreasePercentage);
        Vector2 velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, targetX, ref velocityXSmoothing, xSmoothTime),
            Mathf.SmoothDamp(rb.velocity.y, targetY, ref velocityYSmoothing, ySmoothTime));
       
        rb.velocity = velocity;
    }
    /// <summary>
    /// initializes dodge rolling, gives player initial roll speed
    /// </summary>
    /// <param name="direct"></param>
    public void DodgeRollInit(in Vector2 direct)
    {
        rb.velocity = direct * dodgeSpeed;
        currentMovements = DodgeRoll;
        StartCoroutine(AllowMovement(dodgeDuration));
    }
    /// <summary>
    /// handles player rolling based on initial direction, smooths movement to 0 with the duration of dodgeSmoothTime
    /// </summary>
    public void DodgeRoll()
    {
        Vector2 velocity = new Vector2(Mathf.SmoothDamp(rb.velocity.x, 0, ref velocityXSmoothing, dodgeSmoothTime),
        Mathf.SmoothDamp(rb.velocity.y, 0, ref velocityYSmoothing, dodgeSmoothTime));
        rb.velocity = velocity;
    }

    public void SetDirectionalInput(in Vector2 _directionalInput)
    {
        directionalInput = _directionalInput;
    }

    /// <summary>
    /// allows player movement after dodge roll is finished
    /// </summary>
    /// <param name="timeToWait"></param>
    /// <returns></returns>
    IEnumerator AllowMovement(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        input.AllowAll();
        currentMovements = HandleMovementVelocity;
    }
}
