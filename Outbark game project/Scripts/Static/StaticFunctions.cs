using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public static class StaticFunctions
{
    public static Color DarkenColor(Color color, float multiplier)
    {
        float h, s, v; //v = darkness value, s = saturation, h = hue aka color
        Color.RGBToHSV(color, out h, out s, out v);
        v *= multiplier;
        Debug.Log(v);
        return Color.HSVToRGB(h, s, v);
    }

    public static Tween FadeImageTween(Image img, bool fadeOut, float duration, Ease easeType = Ease.Linear)
    {
        Color tempCol = img.color;
        float alpha = fadeOut ? 0 : 1;
        tempCol.a = alpha;
        return DOTween.To(() => img.color, x => img.color = x, tempCol, duration).SetEase(easeType);
    }

    public static Tween FadeTextAlpha(TextMeshPro img,float duration, Ease easeType = Ease.Linear)
    {
        Color tempCol = img.color;
        tempCol.a = 0;
        return DOTween.To(() => img.color, x => img.color = x, tempCol, duration).SetEase(easeType);
    }

    public static void CopyList<T>(ref List<T> from, ref List<T> to)
    {
        to.Clear();
        foreach(var item in from)
        {
            to.Add(item);
        }
    }

    public static Vector2 PositionOnUnitCircle(float angle)
    {
        return new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
    }

    public static float AngleBetweenTwoPoints(Vector3 startPos, Vector3 endPos)
    {
        return Mathf.Atan2(endPos.y - startPos.y, endPos.x - startPos.x) * 180 / Mathf.PI;
    }


}
