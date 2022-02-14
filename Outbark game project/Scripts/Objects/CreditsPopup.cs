using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CreditsPopup : MonoBehaviour
{
    public GameObject panel;
    public Button button;
    public Button creditsButton;
    private bool disappearing = false;
    private bool appearing = false;
    void Start()
    {
        gameObject.SetActive(false);
        //PopupAppear();
        creditsButton.onClick.AddListener(delegate { PopupAppear(); });
        button.onClick.AddListener(delegate { PopupDisappear(0f); });
    }

    private void PopupAppear()
    {
        if (appearing) return;
        appearing = true;
        gameObject.SetActive(true);
        panel.transform.localScale = Vector3.zero;
        var tween = panel.transform.DOScale(1, 2);
        // tween.OnComplete(delegate { PopupDisappear(3); });
        tween.OnComplete(() => appearing = false);

    }
    public void PopupDisappear(float waitBeforeDisappear)
    {
        appearing = false;
        if (disappearing) return;
        disappearing = true;
        StartCoroutine(PopupDisappearRoutine(waitBeforeDisappear));
    }

    private IEnumerator PopupDisappearRoutine(float waitBeforeDisappear)
    {
        Debug.Log("disappear");
        yield return new WaitForSeconds(waitBeforeDisappear);
        var tween = panel.transform.DOScale(0, 1);
        tween.OnComplete(() => gameObject.SetActive(false));
        tween.OnComplete(() => disappearing = false);
    }
    private void OnEnable()
    {
        //PopupAppear();
    }
}


