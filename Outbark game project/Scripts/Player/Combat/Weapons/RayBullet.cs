using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ray bullet, doesn't handle collision with enemies yet, currently only handles one bounce, can be extended if needed
/// </summary>
public class RayBullet : Bullet 
{
    // Start is called before the first frame update
    public LineRenderer lineRenderer;
    //need a second linerenderer or the line looks jagged when reflected
    public LineRenderer bouncingRay;
    public int maxLaserLength = 10;
    [HideInInspector]
    public int bounceCount = 1;
    private int currentBounces = 0;
    public bool bounces = true;

    private float rayCount=4;//not timpelmented yet
    private float raySpacing=0.5f;

    RaycastHit2D enemyHit;
    RaycastHit2D wallHit;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        bouncingRay = new GameObject().AddComponent<LineRenderer>();
        bouncingRay.material = lineRenderer.material;
        bouncingRay.positionCount = 2;
        bouncingRay.startWidth = lineRenderer.startWidth;
        bouncingRay.endWidth = lineRenderer.endWidth;
    }
    private void OnEnable()
    {
        bouncingRay.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        bouncingRay.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void OnEnemyHit()
    {
        enemyHit.collider.gameObject.GetComponent<IDamageable>().TakeDamage((float)damage*Time.deltaTime);
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, enemyHit.point);
        
        GlobalReferences.instance.InstantiateCombatTextAdditive((float)damage*Time.deltaTime, enemyHit.point);
    }
    void Update()
    {
        bouncingRay.transform.position = lineRenderer
            .transform.position;
        
        enemyHit = Physics2D.Raycast(transform.position, transform.right, maxLaserLength, damagingLayer); 
        
        wallHit = Physics2D.Raycast(transform.position, transform.right,maxLaserLength,wallLayer);

        if (enemyHit)
        {
            RayHitEffect(enemyHit.point);
            ParticlesOnRayEffect(transform.position, enemyHit.point);
            OnEnemyHit();
        }
        else if (wallHit)
        {
            RayHitEffect(wallHit.point);
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, wallHit.point);
            ParticlesOnRayEffect(bouncingRay.transform.position, wallHit.point);
            var bounceVector = Bounce(wallHit);

            bouncingRay.positionCount = 2;
            bouncingRay.SetPosition(0, wallHit.point);
            bouncingRay.SetPosition(1, (Vector2)wallHit.point + bounceVector.normalized * maxLaserLength);


            enemyHit = Physics2D.Raycast(wallHit.point + bounceVector.normalized * 0.2f, bounceVector, maxLaserLength, damagingLayer);
            //second collision with a wall, some reason detected itself so added a small offset
            var oldWallHit = wallHit;
            wallHit = Physics2D.Raycast(wallHit.point+bounceVector.normalized*0.2f, bounceVector, maxLaserLength, wallLayer);
            if (enemyHit)
            {
                RayHitEffect(enemyHit.point);
                ParticlesOnRayEffect(oldWallHit.point, enemyHit.point);
                bouncingRay.SetPosition(1, (Vector2)enemyHit.point);
                enemyHit.collider.gameObject.GetComponent<IDamageable>().TakeDamage(damage * Time.deltaTime);
            }
            else if (wallHit)
            {
                RayHitEffect(enemyHit.point);
                ParticlesOnRayEffect(oldWallHit.point, wallHit.point);
                bouncingRay.SetPosition(1, (Vector2)oldWallHit.point);
            }
            else
            {
                ParticlesOnRayEffect(oldWallHit.point,(Vector2)oldWallHit.point + bounceVector.normalized * maxLaserLength);
            }
        }
        else
        {
            ParticlesOnRayEffect(transform.position, transform.position + transform.right.normalized * maxLaserLength);
            bouncingRay.positionCount = 0;
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, transform.position+transform.right.normalized * maxLaserLength);
        }
        bouncingRay.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    public Vector2 Bounce(in RaycastHit2D hit)
    {
        var dir = Vector2.Reflect(transform.right.normalized, hit.normal);
        return dir;
    }

    void RayHitEffect(Vector3 pos)
    {
        var instance = GlobalReferences.instance.laserSparkPool.GetNext();
        instance.SetActive(true);
        instance.transform.position = pos;
        instance.transform.GetComponent<ParticleSystem>().Play();
    }

    void ParticlesOnRayEffect(Vector3 startPos, Vector3 endPos)
    {
        var instance = GlobalReferences.instance.laserParticlePool.GetNext();
        instance.SetActive(true);

        float angle = Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * 180 / Mathf.PI;
        instance.transform.position = startPos;
        instance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        var distanceBetweenPoints = Vector3.Distance(startPos, endPos);
        instance.gameObject.transform.localScale = new Vector3((distanceBetweenPoints*5), 1, 1);

        var system = instance.GetComponent<ParticleSystem>();
        ParticleSystem.Burst burst = new ParticleSystem.Burst();
        burst.count = 20 * (distanceBetweenPoints / maxLaserLength);
        system.emission.SetBurst(0, burst);
        system.Play();

    }


}
