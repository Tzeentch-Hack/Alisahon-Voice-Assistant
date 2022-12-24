using Models;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGateway
{

    private readonly string basePath = "";
	private RequestHelper currentRequest;

	public static ChatGateway Instance;
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	public void Initialize()
	{ 
		Instance = this;
	}

	private void LogMessage(string title, string message)
	{
#if UNITY_EDITOR
		EditorUtility.DisplayDialog(title, message, "Ok");
#else
		Debug.Log(message);
#endif
	}

	public void PostAudio(byte[] audioclip)
    {
		currentRequest = new RequestHelper
		{
			Uri = basePath + "/posts",
			Body = new DialogAudioRequestModel
			{
				audio = audioclip
			},
			EnableDebug = true
		};
		RestClient.Post<DialogResponseModel>(currentRequest)
		.Then(res => {
			DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(res.audioUrl, AudioType.MPEG);
			AudioClip clip = downloadHandler.audioClip;
			ChatInteractor.Instance.GetResponse.Invoke(new DialogUIResponseModel {
				action = res.action,
				answerText= res.text,
				audioClip = clip,
				questionText = res.questionText
			}); 
			this.LogMessage("Success", JsonUtility.ToJson(res, true));
		})
		.Catch(err => this.LogMessage("Error", err.Message));
	}

    public void PostText(string text)
    {
		currentRequest = new RequestHelper
		{
			Uri = basePath + "/posts",
			Body = new DialogTextRequestModel
			{
			     text = text
			},
			EnableDebug = true
		};
		RestClient.Post<DialogResponseModel>(currentRequest)
		.Then(res => {
			DownloadHandlerAudioClip downloadHandler = new DownloadHandlerAudioClip(res.audioUrl, AudioType.MPEG);
			AudioClip clip = downloadHandler.audioClip;
			ChatInteractor.Instance.GetResponse.Invoke(new DialogUIResponseModel
			{
				action = res.action,
				answerText = res.text,
				audioClip = clip,
				questionText = res.questionText
			});
			this.LogMessage("Success", JsonUtility.ToJson(res, true));
		})
		.Catch(err => this.LogMessage("Error", err.Message));
	}

}