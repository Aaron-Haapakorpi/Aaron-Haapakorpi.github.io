using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkTest : MonoBehaviour
{
    public ObjectPool pool;
    public ParticleSystem spark;
    public ParticleSystem laserSparks;
    public ParticleSystem laserParticlesOnRay;
    public ParticleSystem bossLaserSparks;
    public ParticleSystem bossLaserParticlesOnRay;

    private void Start()
    {
        pool = GetComponent<ObjectPool>();
        pool.Initialize(5, spark.gameObject);
        GlobalReferences.instance.sparkPool = pool;
        if (laserSparks != null)
        {
            ObjectPool pool2 = gameObject.AddComponent<ObjectPool>();
            pool2.Initialize(3, laserSparks.gameObject);
            GlobalReferences.instance.laserSparkPool = pool2;

            ObjectPool pool3 = gameObject.AddComponent<ObjectPool>();
            pool3.Initialize(3, laserParticlesOnRay.gameObject);
            GlobalReferences.instance.laserParticlePool = pool3;
        }
        if (bossLaserSparks != null)
        {
            ObjectPool pool4 = gameObject.AddComponent<ObjectPool>();
            pool4.Initialize(3, bossLaserSparks.gameObject);
            GlobalReferences.instance.bossLaserSparkPool = pool4;

            ObjectPool pool5 = gameObject.AddComponent<ObjectPool>();
            pool5.Initialize(3, bossLaserParticlesOnRay.gameObject);
            GlobalReferences.instance.bossLaserParticlePool = pool5;
        }
    }
}
