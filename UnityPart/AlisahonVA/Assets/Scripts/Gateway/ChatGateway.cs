using Models;
using Proyecto26;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ChatGateway
{

    private readonly string basePath = "http://192.168.134.187:5000";
	private RequestHelper currentRequest;

	public static ChatGateway Instance;
	/*
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	public void Initialize()
	{ 
		Instance = this;
	}
	*/

	public ChatGateway()
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
			Uri = basePath + "/getAnswerByAudio",
			FormData = new WWWForm(),
			EnableDebug = true
		};
		currentRequest.FormData.AddBinaryData("audio", audioclip, "first");
		RestClient.Post<DialogResponseModel>(currentRequest)
		.Then(res => {
			var fileUrl = basePath + res.audioUrl;
			RestClient.Get(new RequestHelper
			{
				Uri = fileUrl,
				DownloadHandler = new DownloadHandlerAudioClip(fileUrl, AudioType.MPEG)
			}).Then(audioRes =>
			{
				AudioClip clip = ((DownloadHandlerAudioClip)audioRes.Request.downloadHandler).audioClip;
				ChatInteractor.Instance.GetResponse.Invoke(new DialogUIResponseModel
				{
					action = res.action,
					answerText = res.answerText,
					audioClip = clip,
					questionText = res.questionText
				});
				this.LogMessage("Success", JsonUtility.ToJson(res, true));
			});
			})
		.Catch(err => {
			ChatInteractor.Instance.ServerError.Invoke();
			this.LogMessage("Error", err.Message);
		});
	}

    public void PostText(string text)
    {
		currentRequest = new RequestHelper
		{
			Uri = basePath + "/getAnswerByText",
			Body = new DialogTextRequestModel
			{
			     text = text
			},
			EnableDebug = true
		};
		RestClient.Post<DialogResponseModel>(currentRequest)
		.Then(res => {
			var fileUrl = basePath + res.audioUrl;
			RestClient.Get(new RequestHelper
			{
				Uri = fileUrl,
				DownloadHandler = new DownloadHandlerAudioClip(fileUrl, AudioType.MPEG)
			}).Then(audioRes =>
			{
				AudioClip clip = ((DownloadHandlerAudioClip)audioRes.Request.downloadHandler).audioClip;
				ChatInteractor.Instance.GetResponse.Invoke(new DialogUIResponseModel
				{
					action = res.action,
					answerText = res.answerText,
					audioClip = clip,
					questionText = res.questionText
				});
				this.LogMessage("Success", JsonUtility.ToJson(res, true));
			});
		})
		.Catch(err => {
			ChatInteractor.Instance.ServerError.Invoke();
			this.LogMessage("Error", err.Message); });
	}

}
