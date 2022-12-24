using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class DialogResponseModel
{

    public string action;

    public string text;

    public string audioUrl;

    public string questionText;

}

public class DialogUIResponseModel
{
    public string action;

    public AudioClip audioClip;

    public string answerText;

    public string questionText;
}

[Serializable]
public class DialogTextRequestModel
{
   public string text;
}

[Serializable]
public class DialogAudioRequestModel
{
   public byte[] audio;
}