using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TravelArrow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Color startingColor;

    public Transform teleportSpot;

    private void Start()
    {
        image = GetComponent<Image>();
        startingColor = image.color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        image.color = Color.green;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        image.color = startingColor;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        image.color = Color.red;
        StartCoroutine(startTravel());
    }

    IEnumerator startTravel()
    {
        GameManager.instance.SwipeClose();
        PlayerStateMachine.instance.PlayerLock();
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.player.transform.position = teleportSpot.position;
        StartCoroutine(finishTravel());
    }

    IEnumerator finishTravel()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.instance.SwipeOpen();
        PlayerStateMachine.instance.PlayerFreeWill();
    }
}
