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
    public SpeakerInfo speakerVal1;
    public SpeakerInfo speakerVal2;
    [Range(1, 2)]public int numberofSpeakers;


    //does not apply if only one speaker
    public enum SpeakerPosition {Left, Center, Right};
    public SpeakerPosition speakerPosition1;
    public SpeakerPosition speakerPosition2;

    public enum EmotionState { neutral, happy, flirty, angry };
    public EmotionState emotionState1;
    public EmotionState emotionState2;

    [TextArea(5, 10)]
    public string[] sentences;
}
