using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// currently handles idle animations and when to use blend tree for running. might transfer blend tree like behaviaour to code if needed.
/// 
/// </summary>
[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    const string playerIdleUp = "playerIdleUp";
    const string playerIdleTopRight = "playerIdleTopRight";
    const string playerIdleTopLeft = "playerIdleTopLeft";

    const string playerIdleLeft = "playerIdleLeft";
    const string playerIdleRight = "playerIdleRight";

    const string playerIdleDown = "playerIdleDown";
    const string playerIdleBottomLeft = "playerIdleBottomLeft";

    const string playerIdleBottomRight = "playerIdleBottomRight";

    const string RunBlending = "RunBlending";

    const string playerDeathUp = "playerDeathUp";
    const string playerDeathTopRight = "playerDeathTopRight";
    const string playerDeathTopLeft = "playerDeathTopLeft";
    const string playerDeathLeft = "playerDeathLeft";
    const string playerDeathRight = "playerDeathRight";
    const string playerDeathDown = "playerDeathDown";
    const string playerDeathBottomLeft = "playerDeathBottomLeft";
    const string playerDeathBottomRight = "playerDeathBottomRight";

    bool running = true;
    private Animator animator;
    private PlayerController controller;

    private Vector2 previousVelocity = Vector2.zero;

    private Rigidbody2D rb;


    public SpriteRenderer upperBody;

    bool dead = false;
    public int previousDir;

    PlayerShooting shooting;
    void Awake()
    {
        shooting = GetComponent<PlayerShooting>();
        animator = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
   
    void Update()
    {
        if (dead) return;
        var currentVel = rb.velocity;
        //animator values set with vleocity instead of input, player slides a little when running
        previousDir = controller.CalculateDirection(previousVelocity.normalized);
        if (currentVel.magnitude <= 0.2)
        {
            PlayIdleAnimation(previousDir);
        }
        else
        {
            animator.SetFloat("Horizontal", rb.velocity.normalized.x);
            animator.SetFloat("Vertical", rb.velocity.normalized.y);
            running = true;
            animator.Play(RunBlending);
            UpdateWeaponPosition();

        }
        if (previousDir == 4 || previousDir == 0)
        {
            upperBody.enabled = true;
        }
        else upperBody.enabled = false;

        previousVelocity = currentVel;
        

    }
    private void PlayIdleAnimation(int previousDir)
    {
        if (!running) return;
        running = false;
        //Debug.Log(Mathf.Atan2(input.directionalInput.x, input.directionalInput.y) * Mathf.Rad2Deg);

        //Debug.Log("input" + previousInp + " and " + previousDir);
        upperBody.enabled = false;
        switch (previousDir)
        {
            case 0:
                animator.Play(playerIdleUp);
                break;
            case 1:
                animator.Play(playerIdleTopRight);
                break;
            case 2:
                animator.Play(playerIdleRight);
                break;
            case 3:
                animator.Play(playerIdleBottomRight);
                break;
            case 4:
                animator.Play(playerIdleDown);
                upperBody.enabled = true;
                break;
            case 5:
                animator.Play(playerIdleBottomLeft);
                break;
            case 6:
                animator.Play(playerIdleLeft);
                break;
            case 7:
                animator.Play(playerIdleTopLeft);
                break;
        }
    }

    public void PlayDeathExplosion()
    {
        dead = true;
        animator.Play("playerDeathExplosion");
    }

    public void PlayDeathAnimationOld()
    {
        dead = true;
        switch (previousDir)
        {
            case 0:
                animator.Play(playerDeathUp);
                break;
            case 1:
                animator.Play(playerDeathTopRight);
                break;
            case 2:
                animator.Play(playerDeathRight);
                break;
            case 3:
                animator.Play(playerDeathBottomRight);
                break;
            case 4:
                animator.Play(playerDeathDown);
                break;
            case 5:
                animator.Play(playerDeathBottomLeft);
                break;
            case 6:
                animator.Play(playerDeathLeft);
                break;
            case 7:
                animator.Play(playerDeathTopLeft);
                break;
        }
    }

    public void UpdateWeaponPosition()
    {
        shooting.weaponRenderer.gameObject.transform.localPosition = shooting.GetNewWeaponPosition(previousDir);
    }
}
