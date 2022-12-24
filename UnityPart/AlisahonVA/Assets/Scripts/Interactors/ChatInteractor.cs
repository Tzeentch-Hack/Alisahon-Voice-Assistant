using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChatInteractor
{
    public static ChatInteractor Instance;
    public UnityEvent<DialogUIResponseModel> GetResponse;


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public void Initialize()
    {
        Instance = this;
        GetResponse.AddListener(SetData);
    }

    public void SendAudio(byte[] audioClip)
    {
        ChatGateway.Instance.PostAudio(audioClip);
    }

    public void SendText(string text)
    {
        ChatGateway.Instance.PostText(text);
    }

    public void SetData(DialogUIResponseModel uIResponseModel)
    {
       
    }

}
