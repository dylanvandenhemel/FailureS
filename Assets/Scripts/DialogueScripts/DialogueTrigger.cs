using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject talkButton;
    public List<DialogueInfo> dialogueList;

    private void OnEnable()
    {
        DialogueManager.EndDialogueDelegate += TalkEnd;
    }

    private void OnDisable()
    {
        DialogueManager.EndDialogueDelegate -= TalkEnd;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            talkButton.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            talkButton.SetActive(false);
        }
    }

    public void TalkButton()
    {
        talkButton.SetActive(false);
        DialogueManager.instance.StartDialogue(dialogueList[0]);

        PlayerStateMachine.instance.PlayerLock();
    }
    private void TalkEnd()
    {
        PlayerStateMachine.instance.PlayerFreeWill();

    }
}
