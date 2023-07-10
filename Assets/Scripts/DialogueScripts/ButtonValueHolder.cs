using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonValueHolder : MonoBehaviour
{
    public TMP_Text buttonTXT;
    public int choicePathValue;


    public void InstantiateButton(DialogueInfo dialogue, string buttonName, int choiceValue)
    {
        choicePathValue = choiceValue;
        buttonTXT.text = buttonName;
        GetComponent<Button>().onClick.AddListener(delegate { DialogueManager.instance.ChoiceButtonSelected(choicePathValue); });
    }
}
