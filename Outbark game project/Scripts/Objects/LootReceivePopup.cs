using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class LootReceivePopup : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI itemText;

    public void SetText(string text)
    {
        itemText.text = text;
    }

    private void PopupAppear()
    {
        panel.transform.localScale = Vector3.zero;
        var tween = panel.transform.DOScale(1, 2);
        tween.OnComplete(delegate { PopupDisappear(3); });
        
    }
    private void PopupDisappear(float waitBeforeDisappear)
    {
        StartCoroutine(PopupDisappearRoutine(waitBeforeDisappear));
    }

    private IEnumerator PopupDisappearRoutine(float waitBeforeDisappear)
    {
        yield return new WaitForSeconds(waitBeforeDisappear);
        var tween = panel.transform.DOScale(0, 1);
        tween.OnComplete(()=>gameObject.SetActive(false));
    }
    private void OnEnable()
    {
        PopupAppear();
    }

}
