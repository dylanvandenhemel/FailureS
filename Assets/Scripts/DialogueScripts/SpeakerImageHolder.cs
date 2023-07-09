using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeakerImageHolder : MonoBehaviour
{
    public GameObject neutral;
    public GameObject happy;
    public GameObject angry;
    public GameObject lewd;


    public void SwitchImageNeutral()
    {
        neutral.SetActive(true);
        happy.SetActive(false);
        angry.SetActive(false);
        lewd.SetActive(false);
    }
    public void SwitchImageHappy()
    {
        neutral.SetActive(false);
        happy.SetActive(true);
        angry.SetActive(false);
        lewd.SetActive(false);
    }
    public void SwitchImageAngry()
    {
        neutral.SetActive(false);
        happy.SetActive(false);
        angry.SetActive(true);
        lewd.SetActive(false);
    }
    public void SwitchImageLewd()
    {
        neutral.SetActive(false);
        happy.SetActive(false);
        angry.SetActive(false);
        lewd.SetActive(true);
    }

    public void MainSpeaker()
    {
        neutral.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        happy.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        angry.GetComponent<Image>().color = new Color(255, 255, 255, 1);
        lewd.GetComponent<Image>().color = new Color(255, 255, 255, 1);
    }
    public void NotSpeaker()
    {
        neutral.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        happy.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        angry.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
        lewd.GetComponent<Image>().color = new Color(255, 255, 255, 0.5f);
    }
}
