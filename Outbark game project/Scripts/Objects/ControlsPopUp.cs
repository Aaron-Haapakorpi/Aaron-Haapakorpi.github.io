using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ControlsPopUp : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject panel;
    public Button button;
    private bool disappearing = false;
    void Start()
    {
        PopupAppear();
        button.onClick.AddListener(delegate { PopupDisappear(0f); });
    }

    private void PopupAppear()
    {
        panel.transform.localScale = Vector3.zero;
        var tween = panel.transform.DOScale(1, 2);
       // tween.OnComplete(delegate { PopupDisappear(3); });

    }
    public void PopupDisappear(float waitBeforeDisappear)
    {
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
    }
    private void OnEnable()
    {
        PopupAppear();
    }
}
