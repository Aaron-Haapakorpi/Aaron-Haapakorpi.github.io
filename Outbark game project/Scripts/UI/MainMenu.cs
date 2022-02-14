using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using DG.Tweening;

public class MainMenu : UIBase
{
    public Button startGameButton;
    public Button optionsButton;
    public Button exitButton;
    public PlayerInput actions;
    public TMP_Dropdown resolutionDropDown;
    public Button returnButton;

    public GameObject mainLayout;
    public GameObject optionLayout;
    public Slider soundSlider;
    public Slider musicSlider;
    public AudioMixer mixer;

    public AudioType ButtonClickSound;

    private Vector3 panelEnterPosition;
    private Vector3 panelEndPosition;
    private Vector3 panelEnterLeftPosition;

    Tween mainScreenTween;
    Tween optionScreenTween;
    private void Awake()
    {
        startGameButton.onClick.AddListener(delegate { SceneLoader.Instance.LoadSceneAsync(1, 2); });
        startGameButton.onClick.AddListener(delegate { AudioManager.instance.PlaySound(AudioType.SFXStartGame); });
        exitButton.onClick.AddListener(Application.Quit);
        exitButton.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });
        optionsButton.onClick.AddListener(delegate { SwapScreen(mainLayout, optionLayout); });
        optionsButton.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });

        resolutionDropDown.onValueChanged.AddListener( delegate { ResolutionChange(resolutionDropDown); });
        returnButton.onClick.AddListener(delegate { SwapScreen(optionLayout, mainLayout); });
        returnButton.onClick.AddListener(delegate { AudioManager.instance.PlaySound(ButtonClickSound); });
        soundSlider.onValueChanged.AddListener(delegate { AudioChange(soundSlider,"SoundVol"); });
        musicSlider.onValueChanged.AddListener(delegate { AudioChange(musicSlider, "MusicVol"); });

        panelEnterPosition = optionLayout.transform.localPosition;
        panelEndPosition = mainLayout.transform.localPosition;
        panelEnterLeftPosition.Set(panelEnterPosition.x*-1, panelEndPosition.y, panelEndPosition.z);

    }
    
    private void Start()
    {
        LoadMenuPrefs();
    }
    void SwapScreen(GameObject from, GameObject to)
    {
        if (mainScreenTween.IsActive())
        {
            mainScreenTween.Kill(true);
        }
        if (optionScreenTween.IsActive())
        {
            optionScreenTween.Kill(true);
        }
        to.gameObject.SetActive(true);
        if (to ==optionLayout)
        {
            optionScreenTween = to.transform.DOLocalMove(panelEndPosition, 0.5f, false);    
            optionScreenTween.SetEase(Ease.Linear);
            mainScreenTween= from.transform.DOLocalMove(panelEnterLeftPosition, 0.5f, false);
            mainScreenTween.SetEase(Ease.Linear);
            mainScreenTween.OnComplete(() =>
            {
                from.gameObject.SetActive(false);
            });
        }
        else
        {
            mainScreenTween = to.transform.DOLocalMove(panelEndPosition, 0.5f, false);
            mainScreenTween.SetEase(Ease.Linear);
            optionScreenTween = from.transform.DOLocalMove(panelEnterPosition, 0.5f, false);
            optionScreenTween.SetEase(Ease.Linear);
            optionScreenTween.OnComplete(() =>
            {
                from.gameObject.SetActive(false);
            });
        }
        EventSystem.current.SetSelectedGameObject(to.gameObject.transform.GetChild(0).gameObject);
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
        var soundValue = PlayerPrefs.GetFloat("SoundVol",1);
        var musicValue = PlayerPrefs.GetFloat("MusicVol",1);
        soundSlider.value = soundValue;
        musicSlider.value = musicValue;

        mixer.SetFloat("SoundVol", Mathf.Log10(soundValue) * 20);
        mixer.SetFloat("MusicVol", Mathf.Log10(musicValue) * 20);
    }

}
