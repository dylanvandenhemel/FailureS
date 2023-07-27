using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OsuGameTarget : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{    
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        gameObject.SetActive(false);
        MiniGameManager.instance.osuTargetsClicked++;
    }
    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
