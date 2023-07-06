using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    [Header("Day Cycle")]
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

    public void DayStart()
    {
        currentCycle = 0;
        UpdateColorTemp();
    }

    //after an action is done move to next time of day
    public void NextDayCycle()
    {
        if(currentCycle < 3)
        {
            currentCycle++;
            UpdateColorTemp();
        }
        else
        {
            DayStart();
        }
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


}
