using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    PlayerControls pActions;
    public DialogueInfo dialogueVal;
    [HideInInspector] public StringBuilder sb = new StringBuilder();

    //canvas and parts
    public GameObject dialogueCanvas;
    public Transform characterImageGroup;
    public Image background;

    //text
    public TMP_Text nameText;
    public TMP_Text sentencesTXT;

    //speaker value
    [HideInInspector] public int iName;
    private int jSent;

    [Range(1, 10)]
    public float typeSpeed;
    private float typeStart;
    bool bIsTalking;
    public Transform portraitCenter;
    public Transform portraitLeft;
    public Transform portraitRight;

    private GameObject speaker1Images;
    private GameObject speaker2Images;

    [Header("Choice Objects")]
    public GameObject choiceButtonPrefab;
    public Transform choiceButtonGroup;
    private int currentChoicePath;

    //skip toggle
    private IEnumerator readLineCourutine;
    bool bSkipActive;

    public static Action EndDialogueDelegate = delegate { };

    public void OnEnable()
    {
        instance = this;

        pActions = new PlayerControls();
        pActions.Enable();

        typeSpeed = 11 - typeSpeed;
        typeSpeed /= 100;
        typeStart = typeSpeed;

        readLineCourutine = ReadLine();
    }

    private void OnDisable()
    {
        pActions.Disable();
        pActions.PlayerActions.Mouse1.started -= NextLineButton;
        pActions.PlayerActions.SkipDialogue.started -= SkipDialogueOn;
        pActions.PlayerActions.SkipDialogue.canceled -= SkipDialogueOff;
    }

    private void NextLineButton(InputAction.CallbackContext c)
    {
        NextLine();
    }

    private void NextLine()
    {
        if (bIsTalking)
        {
            SkipLine(dialogueVal);
        }
        else
        {
            DisplayNextSentance();
        }
    }

    public void StartDialogue(DialogueInfo dialogue)
    {
        GameManager.instance.DisableDayCycleCanvas();

        dialogueVal = dialogue;
        //if dialogue paths are availiable
        if (dialogueVal.conversation[iName].bHasChoice)
        {
            ChoiceButtonGen();
            bSkipActive = false;

            pActions.PlayerActions.Mouse1.started -= NextLineButton;
            pActions.PlayerActions.SkipDialogue.started -= SkipDialogueOn;
            pActions.PlayerActions.SkipDialogue.canceled -= SkipDialogueOff;
        }
        else
        {
            pActions.PlayerActions.Mouse1.started += NextLineButton;
            pActions.PlayerActions.SkipDialogue.started += SkipDialogueOn;
            pActions.PlayerActions.SkipDialogue.canceled += SkipDialogueOff;

        }

        ManageSpeakerImages();
        ChangeBackground();

        //fills sb with the name and replaces it with the saved player name
        if (dialogue.conversation[iName].charName == "")
        {
            nameText.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            nameText.transform.parent.gameObject.SetActive(true);
        }
        nameText.text = "";
        sentencesTXT.text = "";
        sb.Clear();
        sb.Append(dialogue.conversation[iName].charName);

        dialogueCanvas.SetActive(true);

        nameText.text = "";
        sentencesTXT.text = "";

        //changes visual text in speech to what is in the sb
        nameText.text = sb.ToString();
        DisplayNextSentance();
    }

    private void DisplayNextSentance()
    {
        if (dialogueVal.conversation[iName].choicePath == 0 || dialogueVal.conversation[iName].choicePath == currentChoicePath)
        {
            if (jSent < dialogueVal.conversation[iName].sentences.Length)
            {
                if (bIsTalking)
                {
                    SkipLine(dialogueVal);
                }
                else
                {
                    typeSpeed = typeStart;
                    StartCoroutine(ReadLine());
                }
            }
            else if (jSent == dialogueVal.conversation[iName].sentences.Length)
            {
                if (iName == dialogueVal.conversation.Count - 1)
                {
                    EndDialogue();
                    return;
                }

                iName++;
                jSent = 0;
                sb.Clear();

                StartDialogue(dialogueVal);
            }
            else
            {
                //catch when jSent is too large
                jSent = 0;
            }
        }
        else
        {
            //not in path go to next available path
            if (iName == dialogueVal.conversation.Count - 1)
            {
                EndDialogue();
                return;
            }
            iName++;
            jSent = 0;
            sb.Clear();

            StartDialogue(dialogueVal);
        }
        
    }

    IEnumerator ReadLine()
    {
        bIsTalking = true;
        sentencesTXT.text = "";
        sb.Clear();

        sb.Append(dialogueVal.conversation[iName].sentences[jSent]);
        foreach (char letter in sb.ToString().ToCharArray())
        {
            if (!bIsTalking)
            {
                yield break;
            }
            sentencesTXT.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        bIsTalking = false;
        jSent++;
    }

    private void SkipLine(DialogueInfo dialogue)
    {
        StopCoroutine(readLineCourutine);
        //StopAllCoroutines();
        sentencesTXT.text = "";
        sb.Clear();
        sb.Append(dialogue.conversation[iName].sentences[jSent]);

        sentencesTXT.text = sb.ToString();
        bIsTalking = false;
        jSent++;
    }
    private void SkipDialogueOn(InputAction.CallbackContext c)
    {
        //Debug.Log("on");
        bSkipActive = true;
        StartCoroutine(skip());
    }
    IEnumerator skip()
    {
        while (bSkipActive)
        {
            yield return new WaitForSeconds(0.1f);
            NextLine();
        }
    }
    private void SkipDialogueOff(InputAction.CallbackContext c)
    {
        //Debug.Log("off");
        bSkipActive = false;
    }

    private void ManageSpeakerImages()
    {
        //I know this is not optimal but It works and I don't have performance issues
        if(speaker1Images != null)
        {
            Destroy(speaker1Images);
        }
        if(dialogueVal.conversation[iName].speakerVal1 != null)
        {
            speaker1Images = Instantiate(dialogueVal.conversation[iName].speakerVal1.speakerImages, characterImageGroup);
            switch (dialogueVal.conversation[iName].speakerPosition1)
            {
                case Dialogue.SpeakerPosition.Center:
                    speaker1Images.transform.position = portraitCenter.position;
                    break;

                case Dialogue.SpeakerPosition.Left:
                    speaker1Images.transform.position = portraitLeft.position;
                    break;

                case Dialogue.SpeakerPosition.Right:
                    speaker1Images.transform.position = portraitRight.position;
                    break;

            }

            switch (dialogueVal.conversation[iName].emotionState1)
            {
                case Dialogue.EmotionState.neutral:
                    speaker1Images.GetComponent<SpeakerImageHolder>().SwitchImageNeutral();
                    break;

                case Dialogue.EmotionState.happy:
                    speaker1Images.GetComponent<SpeakerImageHolder>().SwitchImageHappy();
                    break;

                case Dialogue.EmotionState.angry:
                    speaker1Images.GetComponent<SpeakerImageHolder>().SwitchImageAngry();
                    break;

                case Dialogue.EmotionState.lewd:
                    speaker1Images.GetComponent<SpeakerImageHolder>().SwitchImageLewd();
                    break;

            }

            //fades char a little if not the main speaker
            if(dialogueVal.conversation[iName].currentSpeaker == 1)
            {
                speaker1Images.GetComponent<SpeakerImageHolder>().MainSpeaker();
            }
            else
            {
                speaker1Images.GetComponent<SpeakerImageHolder>().NotSpeaker();
            }
        }

        //I know this is not optimal but It works and I don't have performance issues
        if (speaker2Images != null)
        {
            Destroy(speaker2Images);
        }
        if (dialogueVal.conversation[iName].speakerVal2 != null)
        {
            speaker2Images = Instantiate(dialogueVal.conversation[iName].speakerVal2.speakerImages, characterImageGroup);
            switch (dialogueVal.conversation[iName].speakerPosition2)
            {
                case Dialogue.SpeakerPosition.Center:
                    speaker2Images.transform.position = portraitCenter.position;
                    break;

                case Dialogue.SpeakerPosition.Left:
                    speaker2Images.transform.position = portraitLeft.position;
                    break;

                case Dialogue.SpeakerPosition.Right:
                    speaker2Images.transform.position = portraitRight.position;
                    break;

            }

            switch (dialogueVal.conversation[iName].emotionState2)
            {
                case Dialogue.EmotionState.neutral:
                    speaker2Images.GetComponent<SpeakerImageHolder>().SwitchImageNeutral();
                    break;

                case Dialogue.EmotionState.happy:
                    speaker2Images.GetComponent<SpeakerImageHolder>().SwitchImageHappy();
                    break;

                case Dialogue.EmotionState.angry:
                    speaker2Images.GetComponent<SpeakerImageHolder>().SwitchImageAngry();
                    break;

                case Dialogue.EmotionState.lewd:
                    speaker2Images.GetComponent<SpeakerImageHolder>().SwitchImageLewd();
                    break;

            }

            //fades char a little if not the main speaker
            if (dialogueVal.conversation[iName].currentSpeaker == 2)
            {
                speaker2Images.GetComponent<SpeakerImageHolder>().MainSpeaker();
            }
            else
            {
                speaker2Images.GetComponent<SpeakerImageHolder>().NotSpeaker();
            }
        }
    }
    private void ChangeBackground()
    {
        if(dialogueVal.conversation[iName].background != null)
        {
            //sets background to plane white to display image properly
            background.color = new Color(255, 255, 255, 1);
            background.sprite = dialogueVal.conversation[iName].background;
        }
        else
        {
            //sets background to a faded dark shade to hide scene a little
            background.sprite = null;
            background.color = new Color(0, 0, 0, 0.3f);
        }
    }
    private void ChoiceButtonGen()
    {
        for(int i = 0; i < dialogueVal.conversation[iName].choices.Count; i++)
        {
            GameObject button = Instantiate(choiceButtonPrefab, choiceButtonGroup);
            //sets choice text name to button text
            button.GetComponent<ButtonValueHolder>().InstantiateButton(dialogueVal, dialogueVal.conversation[iName].choices[i], i + 1);
        }
    }

    //genorated buttons wioll be assained a choice path value
    public void ChoiceButtonSelected(int choicePathVal)
    {
        Debug.Log(choicePathVal);
        currentChoicePath = choicePathVal;

        //destroy gen buttons after use
        for(int i = 0; i < choiceButtonGroup.childCount; i++)
        {
            Destroy(choiceButtonGroup.GetChild(i).gameObject);
        }

        //continue dialogue
        StopCoroutine(readLineCourutine);
        iName++;
        jSent = 0;
        sb.Clear();
        bSkipActive = false;
        bIsTalking = false;

        StartDialogue(dialogueVal);

        pActions.PlayerActions.Mouse1.started += NextLineButton;
        pActions.PlayerActions.SkipDialogue.started += SkipDialogueOn;
        pActions.PlayerActions.SkipDialogue.canceled += SkipDialogueOff;
    }
    private void EndDialogue()
    {
        GameManager.instance.EnableDayCycleCanvas();

        dialogueCanvas.SetActive(false);
        pActions.PlayerActions.Mouse1.started -= NextLineButton;
        pActions.PlayerActions.SkipDialogue.started -= SkipDialogueOn;
        pActions.PlayerActions.SkipDialogue.canceled -= SkipDialogueOff;
        StopAllCoroutines();

        bSkipActive = false;
        iName = 0;
        jSent = 0;

        //tells any active script dialogue to end
        EndDialogueDelegate();
    }

}
