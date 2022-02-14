using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using DG.Tweening;
using System;

/// <summary>
/// currently unused
/// </summary>
public class OptionMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioMixer mixer;

    void ResolutionChange(in TMP_Dropdown dropDown)
    {
        int width = 1920;
        int height = 1080;
        switch (dropDown.value)
        {
            case 0:
                width = 1366;
                height = 768;
                break;
            case 1:
                width = 1920;
                height = 1080;
                break;
            case 2:
                width = 2560;
                height = 1440;
                break;
            default:
                return;
        }
        Screen.SetResolution(width, height, false);
    }

    void AudioChange(in Slider slider, in string param)
    {
        mixer.SetFloat(param, Mathf.Log10(slider.value) * 20);
    }
}
