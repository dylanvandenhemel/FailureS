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
    public int choicePath;
    public List<string> choices;
}
