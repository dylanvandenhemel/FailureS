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


    //all games info
    private int miniGameID = 1;
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
        pActions.PlayerActions.Mouse1.started -= MiniGameClick;
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
        miniGameID = Random.Range(1, 3);
        //miniGameID = 2;

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

        }

    }

    //ring increases to size of match ring, player must press button when scale is close enough to match scale
        private void RingMatch()
        {
            bRingGameActive = true;
            ringMatchGame.SetActive(true);
            pActions.PlayerActions.Mouse1.started += MiniGameClick;
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

    //bar slider game, spot slides on bar back and forth until press
    private void BarSlide()
    {
        bBarGameActive = true;
        barGame.SetActive(true);
        pActions.PlayerActions.Mouse1.started += MiniGameClick;
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
        Debug.Log("succ");
        totalWonGames++;
        //player, partner sliders  respectivly
        AdjustSliders(5, 15);

        CancelGame();
    }
    private void FailedClick()
    {
        Debug.Log("fail");

        AdjustSliders(20, 5);

        CancelGame();
    }

    public void AdjustSliders(float playerAmount, float partnerAmount)
    {
        playerSlider.value += playerAmount;
        partnerSlider.value += partnerAmount;
    }

    private void CancelGame()
    {
        pActions.PlayerActions.Mouse1.started -= MiniGameClick;
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




    }

    IEnumerator EndGameRound(int wonGames)
    {
        yield return new WaitForSeconds(1);

        miniGameGroup.SetActive(false);
        Debug.Log("Games Won: " + wonGames);

    }

    public void EndMiniGameMode()
    {
        GameManager.instance.EnableDayCycleCanvas();
        miniGameGroup.SetActive(false);
    }

}