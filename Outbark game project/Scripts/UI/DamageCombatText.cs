using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


[RequireComponent(typeof(TMPro.TextMeshPro))]
public class DamageCombatText : MonoBehaviour
{
    private TextMeshPro textComponent;
    public float moveAmount = 5;
    public float duration = 3;
    public float shakeStrength = 0.5f;

    private void Awake()
    {
        textComponent = GetComponent<TextMeshPro>();
    }
    private void OnEnable()
    {
        //transform.localPosition = Vector3.zero;
        Reset();
        transform.DOShakePosition(duration,shakeStrength);
        transform.DOLocalMoveY(transform.localPosition.y +moveAmount, duration);
        StaticFunctions.FadeTextAlpha(textComponent,duration);
        Invoke("SetDisable", duration);
    }

    private void SetDisable()
    {
        gameObject.SetActive(false);
    }

    public void SetText(string text)
    {
        textComponent.text = text;
    }

    private void Reset()
    {
        var tempCol = textComponent.color;
        tempCol.a =1;
        textComponent.color = tempCol;
    }

}
