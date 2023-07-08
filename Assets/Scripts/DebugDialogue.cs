using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDialogue : MonoBehaviour
{
    public DialogueInfo testDialogueToSend;
    public void DebugButtonDialogue()
    {
        DialogueManager.instance.StartDialogue(testDialogueToSend);
    }
}
