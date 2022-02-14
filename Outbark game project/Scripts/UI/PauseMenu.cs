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


public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    Button toGame;
    [SerializeField]
    Button toMain;
    [SerializeField]
    Button toOptions;
    public GameObject pauseLayout;
    public GameObject optionLayout;
    [SerializeField]
    Button toPause;

    private Vector3 panelEnterRightPosition;
    private Vector3 panelEndPosition;
    private Vector3 panelEnterLeftPosition;

    Tween oldTween;
    Tween newTween;

    public Action onPause;
    public Action onContinue;

    public TMP_Dropdown resolutionDropDown;
    public AudioType ButtonClickSound;
    public Slider soundSlider;
    public Slider musicSlider;
    public AudioMixer mixer;

    private void Awake()
    {
        toGame.onClick.AddListener(TogglePauseMenu);
        toMain.onClick.AddListener(delegate { SceneLoader.Instance.LoadSceneWithfade(0);});
        toGame.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });
        toMain.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });
        toOptions.onClick.AddListener(delegate { ChangeScreen(pauseLayout, optionLayout,true); });
        toPause.onClick.AddListener(delegate { ChangeScreen(optionLayout, pauseLayout,false); });

        toOptions.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });
        toPause.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });

        panelEnterRightPosition = optionLayout.transform.localPosition;
        panelEndPosition = pauseLayout.transform.localPosition;
        panelEnterLeftPosition.Set(panelEnterRightPosition.x * -1, panelEndPosition.y, panelEndPosition.z);


        soundSlider.onValueChanged.AddListener(delegate { AudioChange(soundSlider, "SoundVol"); });
        musicSlider.onValueChanged.AddListener(delegate { AudioChange(musicSlider, "MusicVol"); });
        resolutionDropDown.onValueChanged.AddListener(delegate { ResolutionChange(resolutionDropDown); });

    }

    private void Start()
    {
        GlobalReferences.instance.pauseMenu = this;
        gameObject.SetActive(false);
    }

    public void TogglePauseMenu()
    {
        if (Time.timeScale == 0)
        {
            pauseLayout.gameObject.SetActive(false);
            optionLayout.gameObject.SetActive(false);
            gameObject.SetActive(false);
            Time.timeScale = 1;
            AudioManager.instance.PlaySound(AudioType.SFXPauseMenuClose);
            if (onContinue != null) onContinue.Invoke();
        }
        else
        {
            //Debug.Log("pausing");
            AudioManager.instance.StopSoundNoOverLap();
            pauseLayout.gameObject.SetActive(true);
            gameObject.SetActive(true);
            ResetMenuPositions();
            Time.timeScale = 0;
            AudioManager.instance.PlaySound(AudioType.SFXPauseMenuOpen);
            if (onPause != null) onPause.Invoke();
            EventSystem.current.SetSelectedGameObject(pauseLayout.gameObject.transform.GetChild(0).gameObject);
        }
    }
    private void ResetMenuPositions()
    {
        pauseLayout.transform.localPosition= panelEndPosition;
        optionLayout.transform.localPosition = panelEnterRightPosition;
    }
    void ChangeScreen(GameObject newActive, GameObject oldActive, bool tweenLeft)
    {
        if (oldTween.IsActive())
        {
            oldTween.Kill(true);
        }
        if (newTween.IsActive())
        {
           newTween.Kill(true);
        }
        oldActive.gameObject.SetActive(true);
        oldTween = oldActive.transform.DOLocalMove(panelEndPosition, 0.5f, false);
        oldTween.SetEase(Ease.Linear);
        oldTween.SetUpdate(true);
        oldTween.OnComplete(() =>
        {
            newActive.gameObject.SetActive(false);
        });
        if (tweenLeft)
        {
            newTween = newActive.transform.DOLocalMove(panelEnterLeftPosition, 0.5f, false);
        }
        else
        {
            newTween = newActive.transform.DOLocalMove(panelEnterRightPosition, 0.5f, false);
        }
        newTween.SetEase(Ease.Linear);
        newTween.SetUpdate(true);
        EventSystem.current.SetSelectedGameObject(oldActive.gameObject.transform.GetChild(0).gameObject);
    }


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
        float value = Mathf.Log10(slider.value) * 20;
        mixer.SetFloat(param, value);
        PlayerPrefs.SetFloat(param, slider.value);
    }

    void LoadMenuPrefs()
    {
        var soundValue = PlayerPrefs.GetFloat("SoundVol", 1);
        var musicValue = PlayerPrefs.GetFloat("MusicVol", 1);
        soundSlider.value = soundValue;
        musicSlider.value = musicValue;

        mixer.SetFloat("SoundVol", Mathf.Log10(soundValue) * 20);
        mixer.SetFloat("MusicVol", Mathf.Log10(musicValue) * 20);
    }
}