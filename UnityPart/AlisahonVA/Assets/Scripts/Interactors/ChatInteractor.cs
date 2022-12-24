using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChatInteractor
{
    public static ChatInteractor Instance;
    public UnityEvent<DialogUIResponseModel> GetResponse;

    /*
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.)]
    public void Initialize()
    {
        Instance = this;
        GetResponse.AddListener(SetData);
    }
    */
    public ChatInteractor()
    {
        if (GetResponse == null)
            GetResponse = new UnityEvent<DialogUIResponseModel>();
        Instance = this;
    }

    public void SendAudio(byte[] audioClip)
    {
        ChatGateway.Instance.PostAudio(audioClip);
    }

    public void SendText(string text)
    {
        ChatGateway.Instance.PostText(text);
    }

}
