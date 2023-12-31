using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu]
public class DialogueInfo : ScriptableObject
{
    public List<Dialogue> conversation;
}

[System.Serializable]
public struct Dialogue
{
    public string charName;
    public Sprite background;
    public SpeakerInfo speakerVal1;
    public SpeakerInfo speakerVal2;
    [Range(1, 2)] public int currentSpeaker;

    public enum SpeakerPosition {Left, Center, Right};
    public SpeakerPosition speakerPosition1;
    public SpeakerPosition speakerPosition2;

    public enum EmotionState { neutral, happy, angry, lewd};
    public EmotionState emotionState1;
    public EmotionState emotionState2;

    [TextArea(5, 10)] public string[] sentences;

    [Header("Choice Information")]
    public bool bHasChoice;
    [Tooltip("0: Main Path(Only skipped if sliders are maxed), 1 - 2: Alternate Dialogue Paths, -3 - -1 Scored Minigames, 4: Partner Meter Maxed, 5: Player Meter Maxed")]public int choicePath;
    public List<string> choices;

    [Header("For Minigame CutAway")]
    [Tooltip("Check if after this dialogue finishes to start a minigame")] public bool bStartMinigame;
    [Tooltip("1 - 3")]public int minigameDifficulty;
    [Tooltip("Check if after this dialogue finishes, return to original dialogue path")] public bool bEndMinigameDialogue;
}
