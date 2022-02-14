using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTrackOnAwake : MonoBehaviour
{
    [SerializeField]
    AudioType type;
    public bool fadeIn;
    public float delayTime = 0.5f;
    private void Start()
    {
        if (fadeIn) StartCoroutine(DelayFadeIn());

        else StartCoroutine(DelayPlay());
    }

    public IEnumerator DelayFadeIn()
    {
        yield return new WaitForSeconds(delayTime);
        AudioManager.instance.PlayTrackWithFadeIn(type, 4, false);
    }

    public IEnumerator DelayPlay()
    {
        yield return new WaitForSeconds(delayTime);
        AudioManager.instance.PlayTrack(type);
    }
}
