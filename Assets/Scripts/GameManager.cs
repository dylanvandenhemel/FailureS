using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    [Header("Fade Animation")]
    public Animator fadeAnim;
    public Animator swipeAnim;
    public GameObject bedCanvas;

    [Header("Day Cycle")]
    public GameObject dayCycleCanvas;
    public TMP_Text dayNumberTXT;
    private int dayNumber;
    public Image cycleImage;
    private int currentCycle;
    private Color morning = Color.white;
    private Color noon = Color.yellow;
    private Color afternoon = Color.red;
    private Color night = Color.blue;


    private void OnEnable()
    {
        instance = this;
    }

    private void Start()
    {
        FadeIn();
        dayNumber++;
        dayNumberTXT.text = "Day: " + dayNumber;
    }

    public void DayStart()
    {
        bedCanvas.SetActive(false);
        StartCoroutine(nextDay());
    }

    //after an action is done move to next time of day
    public void NextDayCycle()
    {
        bedCanvas.SetActive(false);
        if (currentCycle < 3)
        {
            StartCoroutine(nextCycle());
        }
        else
        {
            DayStart();
        }
    }

    IEnumerator nextCycle()
    {
        FadeOut();
        PlayerStateMachine.instance.PlayerLock();
        yield return new WaitForSeconds(1);
        currentCycle++;
        UpdateColorTemp();

        FadeIn();
        PlayerStateMachine.instance.PlayerFreeWill();
    }

    IEnumerator nextDay()
    {
        FadeOut();
        PlayerStateMachine.instance.PlayerLock();
        yield return new WaitForSeconds(2);
        currentCycle = 0;
        UpdateColorTemp();

        dayNumber++;
        dayNumberTXT.text = "Day: " + dayNumber;

        FadeIn();
        PlayerStateMachine.instance.PlayerFreeWill();
    }

    public void FadeIn()
    {
        fadeAnim.SetTrigger("fadeIn");
    }
    public void FadeOut()
    {
        fadeAnim.SetTrigger("fadeOut");
    }
    public void SwipeOpen()
    {
        swipeAnim.SetTrigger("swipeOpen");
    }
    public void SwipeClose()
    {
        swipeAnim.SetTrigger("swipeClose");
    }

    private void UpdateColorTemp()
    {
        if(currentCycle == 0)
        {
            cycleImage.color = morning;
        }
        else if (currentCycle == 1)
        {
            cycleImage.color = noon;
        }
        else if (currentCycle == 2)
        {
            cycleImage.color = afternoon;
        }
        else
        {
            cycleImage.color = night;
        }
    }

    public void EnableDayCycleCanvas()
    {
        dayCycleCanvas.SetActive(true);
    }
    public void DisableDayCycleCanvas()
    {
        dayCycleCanvas.SetActive(false);
    }
}
