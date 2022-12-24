using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class AnswerStructure 
{
    string text;

    string audioUrl;
}

[Serializable]
public class DialogResponseModel
{

    string action;

    AnswerStructure answer;

    string questionText;

}

[Serializable]
public class DialogTextRequestModel
{
    string text;
}

[Serializable]
public class DialogAudioRequestModel
{
    byte[] audio;
}