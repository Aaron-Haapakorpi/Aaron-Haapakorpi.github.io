using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//currently unused
public class UIBase : MonoBehaviour
{
   
    protected List<Selectable> selectableList = new List<Selectable>();
    private int currentListIndex = 0;
    protected void ControllerUp()
    {
        if (currentListIndex < selectableList.Count - 1) currentListIndex++;
        else currentListIndex = 0;
        selectableList[currentListIndex].Select();
    }

    protected void ControllerDown()
    {
        if (currentListIndex > 0) currentListIndex--;
        else currentListIndex = selectableList.Count - 1;
        selectableList[currentListIndex].Select();
    }

    protected void ControllerPress()
    {
    }
}
