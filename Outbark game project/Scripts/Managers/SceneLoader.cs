using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using DG.Tweening;

public class SceneLoader: MonoBehaviour
{
    // Start is called before the first frame update
    private static SceneLoader instance;
    public static SceneLoader Instance { get { return instance; } }
    public Image UIFadeImg;
    public event Action OnLoadEndEvent;
    private bool sceneIsLoading = false;
    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        DontDestroyOnLoad(this);
        //SceneManager.activeSceneChanged += LevelLoaded; 
    }
    public void LoadSceneAsync(int scene, float timeToWait)
    {
        if (sceneIsLoading) return;
        sceneIsLoading = true;
        //StartCoroutine(LoadSceneAsyncCoroutine(scene, timeToWait));
        LoadSceneAsyncTween(scene, timeToWait);
    }

    public void LoadSceneAsync(int scene, float timeToWait, Action doAtEnd)
    {
        if (sceneIsLoading) return;
        sceneIsLoading = true;
        //StartCoroutine(LoadSceneAsyncCoroutine(scene, timeToWait));
        LoadSceneAsyncTween(scene, timeToWait,doAtEnd);
    }
    public void LoadSceneWithfade(int scene)
    {
        DOTween.KillAll();
        SceneManager.LoadScene(scene);
        ChangeImageAlpha(UIFadeImg, 0);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Loads scene asynchronously with fade
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="timeToWait"></param>
    /// <returns></returns>
    IEnumerator LoadSceneAsyncCoroutine(int scene, float timeToWait)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        StartCoroutine(FadeImage(UIFadeImg,1, timeToWait));
        yield return new WaitForSeconds(timeToWait);
        async.allowSceneActivation = true;
        sceneIsLoading = false;
    }

    void LoadSceneAsyncTween(int scene, float timeToWait)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        var test = FadeImageTween(UIFadeImg, false, timeToWait,Ease.OutQuad);
        test.OnComplete(() =>
        {
            DOTween.KillAll();
            async.allowSceneActivation = true;
            if (OnLoadEndEvent != null) OnLoadEndEvent.Invoke();
            var t = FadeImageTween(UIFadeImg, true, timeToWait);
            sceneIsLoading = false;
        });
    }

    void LoadSceneAsyncTween(int scene, float timeToWait, Action actionEnd)
    {
        var async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        var test = FadeImageTween(UIFadeImg, false, timeToWait, Ease.OutQuad);
        test.OnComplete(() =>
        {
            DOTween.KillAll();
            async.allowSceneActivation = true;
            if (OnLoadEndEvent != null) OnLoadEndEvent.Invoke();
            var t = FadeImageTween(UIFadeImg, true, timeToWait);
            sceneIsLoading = false;
            actionEnd.Invoke();
        });
    }
    /// <summary>
    /// when level is loaded fades the screen back if the screen is black
    /// </summary>
    /// <param name="current"></param>
    /// <param name="next"></param>
    private void LevelLoaded(Scene current, Scene next)
    {
        if (UIFadeImg.color.a == 1) StartCoroutine(FadeImage(UIFadeImg, 0, 1));
    }

    public IEnumerator FadeImage(Image img,float alpha,float duration)//should probably do with a tweening library for easier implementation and different easing
    {
        var timer = 0f;
        var tempCol= img.color;
        float start = tempCol.a;
        while(timer < duration)
        {
            timer += Time.deltaTime;
            tempCol.a = Mathf.Lerp(start, alpha, timer/duration);
            img.color = tempCol;
            yield return null;
        }
        tempCol.a = alpha;
        img.color = tempCol;
    }

    public Tween FadeImageTween(Image img, bool fadeOut, float duration, Ease easeType = Ease.InQuad)
    {
        Color tempCol = img.color;
        float alpha = fadeOut ? 0 : 1;
        tempCol.a = alpha;
        return DOTween.To(() => img.color, x => img.color = x, tempCol, duration).SetEase(easeType);
    }

    public void ChangeImageAlpha(Image img, float alpha)
    {
        var temp = img.color;
        temp.a = alpha;
        img.color = temp;
    }
}
