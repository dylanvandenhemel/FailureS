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
    //public Image background;

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

    //skip toggle
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
        //portrait.enabled = false;
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
        dialogueVal = dialogue;
        pActions.PlayerActions.Mouse1.started += NextLineButton;
        pActions.PlayerActions.SkipDialogue.started += SkipDialogueOn;
        pActions.PlayerActions.SkipDialogue.canceled += SkipDialogueOff;

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
        // sb.Replace("/name", pName.playerName);

        dialogueCanvas.SetActive(true);

        nameText.text = "";
        sentencesTXT.text = "";

        //changes visual text in speech to what is in the sb
        nameText.text = sb.ToString();
        DisplayNextSentance();
    }

    private void DisplayNextSentance()
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
    }

    IEnumerator ReadLine()
    {
        bIsTalking = true;
        sentencesTXT.text = "";
        sb.Clear();

        sb.Append(dialogueVal.conversation[iName].sentences[jSent]);
        //  sb.Replace("/name", pName.playerName);
        foreach (char letter in sb.ToString().ToCharArray())
        {
            sentencesTXT.text += letter;
            yield return new WaitForSeconds(typeSpeed);
        }
        bIsTalking = false;
        jSent++;
    }

    private void SkipLine(DialogueInfo dialogue)
    {
        StopAllCoroutines();
        sb.Clear();
        sb.Append(dialogue.conversation[iName].sentences[jSent]);
        //   sb.Replace("/name", pName.playerName);

        sentencesTXT.text = sb.ToString();
        bIsTalking = false;
        jSent++;
    }
    private void SkipDialogueOn(InputAction.CallbackContext c)
    {
        Debug.Log("on");
        bSkipActive = true;
        StartCoroutine(skip());
    }
    IEnumerator skip()
    {
        while (bSkipActive)
        {
            yield return new WaitForSeconds(0.2f);
            NextLine();
            Debug.Log("deee");
        }
    }
    private void SkipDialogueOff(InputAction.CallbackContext c)
    {
        Debug.Log("off");
        bSkipActive = false;
    }

    private void EndDialogue()
    {
        bSkipActive = false;
        dialogueCanvas.SetActive(false);
        pActions.PlayerActions.Mouse1.started -= NextLineButton;
        pActions.PlayerActions.SkipDialogue.started -= SkipDialogueOn;
        pActions.PlayerActions.SkipDialogue.canceled -= SkipDialogueOff;

        iName = 0;
        jSent = 0;

        //tells any active script dialogue to end
        EndDialogueDelegate();
    }

}
