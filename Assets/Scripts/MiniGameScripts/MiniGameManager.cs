using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager instance;
    PlayerControls pActions;

    [Header("Basic Game UI")]
    public GameObject miniGameGroup;
    public Slider playerSlider;
    public Slider partnerSlider;

    [Header("Ring Game")]
    public GameObject ringMatchGame;
    public RectTransform ringToMatchObject;
    public RectTransform ringToScaleObject;
    private bool bRingGameActive;

    [Header("Bar Game")]
    public GameObject barGame;
    public RectTransform barSlider;
    private Vector3 barSliderStartPos;
    public RectTransform barTarget;
    public RectTransform barLeftSide;
    public RectTransform barRightSide;
    private bool bBarGameActive;

    [Header("OSU Game")]
    public GameObject osuGame;
    public Slider osuGameSlider;
    public List<GameObject> osuTargets;
    [HideInInspector] public int osuTargetsClicked;
    private bool bOSUGameActive;


    //all games info
    private int miniGameID = 1;
    private int miniGameIdOld;
    private int difficultyScale;
    private bool bInSpace;
    private int totalWonGames;
    private int roundOfGames = 3;

    private void OnEnable()
    {
        instance = this;

        pActions = new PlayerControls();
        pActions.Enable();
    }
    private void OnDisable()
    {
        pActions.Disable();
        pActions.PlayerActions.Confirm.started -= MiniGameClick;
    }

    public void StartMiniGame(int difficulty)
    {
        GameManager.instance.DisableDayCycleCanvas();

        miniGameGroup.SetActive(true);
        difficultyScale = difficulty;
        StartCoroutine(miniGameStart());
    }
    IEnumerator miniGameStart()
    {
        miniGameID = Random.Range(1, 4);
        
        while(miniGameID == miniGameIdOld)
        {
            miniGameID = Random.Range(1, 4);
        }
        miniGameIdOld = miniGameID;

        //bar slide starter
        barSliderStartPos = barLeftSide.anchoredPosition;

        yield return new WaitForSeconds(1);
        //ring match
        if(miniGameID == 1)
        {
            RingMatch();
        }
        //Bar Slide
        else if(miniGameID == 2)
        {
            BarSlide();
        }
        else if(miniGameID == 3)
        {
            OSUGame();
        }
        //maybe more........

    }

    //ring increases to size of match ring, player must press button when scale is close enough to match scale
    private void RingMatch()
    {
        bRingGameActive = true;
        ringMatchGame.SetActive(true);
        pActions.PlayerActions.Confirm.started += MiniGameClick;
        StartCoroutine(ringMatchTiming());
    }
    IEnumerator ringMatchTiming()
    {
        while (ringToScaleObject.localScale.x < ringToMatchObject.localScale.x + 0.4f && bRingGameActive)
        {
            if(ringToScaleObject.localScale.x < ringToMatchObject.localScale.x + 0.1f && ringToScaleObject.localScale.x > ringToMatchObject.localScale.x - 0.1f)
            {
                bInSpace = true;
            }
            else
            {
                bInSpace = false;
            }
            yield return new WaitForFixedUpdate();
            ringToScaleObject.localScale = new Vector2(ringToScaleObject.localScale.x + (0.01f * difficultyScale), ringToScaleObject.localScale.y + (0.01f) * difficultyScale);
        }

        if (bRingGameActive)
        {
            FailedClick();
        }
    }
    //


    //bar slider game, spot slides on bar back and forth until press
    private void BarSlide()
    {
        bBarGameActive = true;
        barGame.SetActive(true);
        pActions.PlayerActions.Confirm.started += MiniGameClick;
        StartCoroutine(barSlideTiming());
    }
    IEnumerator barSlideTiming()
    {
        RectTransform currentTarget = barRightSide;
        barSlider.anchoredPosition = barSliderStartPos;
        while (bBarGameActive)
        {
            Vector3 direction = (currentTarget.position - barSlider.position).normalized;
            float distanceToTarget = Vector3.Distance(barSlider.position, currentTarget.position);
            float stoppingDistance = 50;

            // Check if the UI element has reached the current target
            if (distanceToTarget > stoppingDistance)
            {
                barSlider.position += direction * 400 * difficultyScale * Time.fixedDeltaTime;
            }
            else
            {
                // Switch the target based on the current target
                if (currentTarget == barRightSide)
                {
                    currentTarget = barLeftSide;
                }
                else
                {
                    currentTarget = barRightSide;
                }
            }

            //range of click time
            if(Vector3.Distance(barSlider.position, barTarget.position) <= 30)
            {
                bInSpace = true;
            }
            else
            {
                bInSpace = false;
            }


            yield return new WaitForFixedUpdate();
        }
    }
    //

    //Osu game, three targets will appear and all need to bhe clicked on before time runs out
    private void OSUGame()
    {
        bOSUGameActive = true;
        osuGame.SetActive(true);
        osuGameSlider.value = 100;

        StartCoroutine(osuTimer());
    }
    IEnumerator osuTimer()
    {
        //prevents duplicates
        int targetIDFirst = 0;
        int targetIDSecond = 0;
        int targetIDThird = 0;

        while (targetIDFirst == targetIDSecond || targetIDFirst == targetIDThird || targetIDSecond == targetIDThird)
        {
            targetIDFirst = Random.Range(0, osuTargets.Count);
            targetIDSecond = Random.Range(0, osuTargets.Count);
            targetIDThird = Random.Range(0, osuTargets.Count);

        }
        //Debug.Log(targetIDFirst + " " + targetIDSecond + " " + targetIDThird + " targetCount: " + osuTargets.Count);
        osuTargets[targetIDFirst].gameObject.SetActive(true);
        osuTargets[targetIDSecond].gameObject.SetActive(true);
        osuTargets[targetIDThird].gameObject.SetActive(true);


        while (bOSUGameActive)
        {
            osuGameSlider.value -= 0.5f * difficultyScale;
            if(osuGameSlider.value <= 0)
            {
                FailedClick();
            }

            yield return new WaitForFixedUpdate();

            //win condition
            if(osuTargetsClicked >= 3)
            {
                SuccessClick();
            }

        }
    }

    //
    private void MiniGameClick(InputAction.CallbackContext c)
    {
        if (bInSpace)
        {
            SuccessClick();
        }
        else
        {
            FailedClick();
        }
    }
    private void SuccessClick()
    {
        //Debug.Log("succ");
        totalWonGames++;
        //player, partner sliders  respectivly
        AdjustSliders(2.5f, 15);

        CancelGame();
    }
    public void FailedClick()
    {
        //Debug.Log("fail");

        AdjustSliders(15, 5);

        CancelGame();
    }

    public void AdjustSliders(float playerAmount, float partnerAmount)
    {
        playerSlider.value += playerAmount;
        partnerSlider.value += partnerAmount;
    }

    private void CancelGame()
    {
        pActions.PlayerActions.Confirm.started -= MiniGameClick;
        bInSpace = false;
        //continue if max games not reached
        roundOfGames--;
        if(roundOfGames > 0)
        {
            StartMiniGame(difficultyScale);
        }
        else
        {
            StartCoroutine(EndGameRound(totalWonGames));
            //end of roun
            totalWonGames = 0;
            roundOfGames = 3;

        }

        //ring game cancel
        bRingGameActive = false;
        ringToScaleObject.localScale = new Vector3(0.1f, 0.1f);
        ringMatchGame.SetActive(false);

        //bar game cancel
        bBarGameActive = false;
        barSlider.anchoredPosition = barSliderStartPos;
        barGame.SetActive(false);

        //osu game cancel
        bOSUGameActive = false;
        for(int i = 0; i < osuTargets.Count; i++)
        {
            osuTargets[i].gameObject.SetActive(false);
        }
        osuTargetsClicked = 0;
        osuGame.SetActive(false);


    }

    IEnumerator EndGameRound(int wonGames)
    {
        if(wonGames == 0)
        {
            wonGames = -1;
        }
        Debug.Log("Games Won: " + wonGames);
        yield return new WaitForSeconds(1);

        miniGameGroup.SetActive(false);
        DialogueManager.instance.dialogueCanvas.SetActive(true);
        DialogueManager.instance.currentChoicePath = wonGames;
        DialogueManager.instance.iName++;
        DialogueManager.instance.jSent = 0;
        DialogueManager.instance.StartDialogue(DialogueManager.instance.dialogueVal);
    }
}
