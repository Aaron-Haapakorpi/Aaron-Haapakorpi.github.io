using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    // Start is called before the first frame update
    public float disableAfter = 0.2f;
    private float currentTimer = 0;
    private Coroutine disableRoutine;
    private void OnEnable()
    {
        currentTimer = 0;
        if (disableRoutine != null)
        {
            StopCoroutine(DisableCoroutine());
        }
       // disableRoutine = StartCoroutine(DisableCoroutine());
    }

    private void Update()
    {
        currentTimer += Time.deltaTime;
    }
    IEnumerator DisableCoroutine()
    {
        yield return new WaitForSeconds(disableAfter);
        disableRoutine = null;
        gameObject.SetActive(false);
    }
    private void DisableParticle()
    {
        Debug.Log(currentTimer);
        gameObject.SetActive(false);
    }
}
