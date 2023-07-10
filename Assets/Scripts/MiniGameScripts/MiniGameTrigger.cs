using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameTrigger : MonoBehaviour
{
    public GameObject gameButton;


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
            gameButton.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameButton.SetActive(false);
        }
    }

    public void GameButton()
    {
        gameButton.SetActive(false);

        PlayerStateMachine.instance.PlayerLock();
    }
    private void TalkEnd()
    {
        PlayerStateMachine.instance.PlayerFreeWill();

    }
}
