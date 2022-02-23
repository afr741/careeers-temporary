using UnityEngine;
using System.Collections;

public class EventManager : MonoBehaviour
{
    public delegate void QuestionIndexChanged();
    public static event QuestionIndexChanged OnQuestionChanged;
    public delegate void TagHasChanged();
    public static event TagHasChanged OnTagChanged;



    public static void InvokeOnQuestionChanged()
    {
        if (OnQuestionChanged != null) OnQuestionChanged();
    }
    public static void InvokeOnTagChanged()
    {
        if (OnTagChanged != null) OnTagChanged();
    }
}