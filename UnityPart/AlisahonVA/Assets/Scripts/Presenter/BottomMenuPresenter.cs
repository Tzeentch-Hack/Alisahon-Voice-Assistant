using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenuPresenter : MonoBehaviour
{
    [SerializeField]
    private Button multiFunctionalButton;

    [SerializeField]
    private GameObject loader;

    [SerializeField]
    private ChatWindowPresenter chatWindowPresenter;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private AudioSource audioSource;


    [SerializeField]
    private Sprite micImage;

    [SerializeField]
    private Sprite sendImage;

    string currentText;
    AudioClip clientAudio;

    private void SetupMenu()
    {
        ChatInteractor.Instance.GetResponse.AddListener(PlayAudio);
        ChatInteractor.Instance.ServerError.AddListener(AfterError);
        multiFunctionalButton.onClick.AddListener(RecordMode);
        multiFunctionalButton.image.sprite = micImage;
        inputField.onValueChanged.AddListener(CheckInput);
    }

    private void AfterError()
    {
        multiFunctionalButton.gameObject.SetActive(true);
        multiFunctionalButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        loader.SetActive(false);
    }

    void Start()
    {
        SetupMenu();
    }

    private void PlayAudio(DialogUIResponseModel dialogUIResponse)
    {
        multiFunctionalButton.gameObject.SetActive(true);
        multiFunctionalButton.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        inputField.text = "";
        loader.SetActive(false);
        audioSource.clip = dialogUIResponse.audioClip;
        audioSource.Play();
        if (dialogUIResponse.action == "chat") return;
        Action(dialogUIResponse.action);
    }

    public void Action(string action)
    {
        bool fail = false;
        string bundleId = action;
        if (action == "internet")
        {
            bundleId = "com.android.chrome";
        }
        else if(action == "alarm")
        {
            bundleId = "com.hb.dialer.free";
        };
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        Debug.Log(packageManager);
        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
        }
        catch (System.Exception e)
        {
            fail = true;
        }

        if (fail || launchIntent == null)
        {
          
            Application.OpenURL($" https://www.google.com/search?q={bundleId}");
            
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        if(launchIntent != null)
        launchIntent.Dispose();
    }

    private void CheckInput(string text)
    {
        multiFunctionalButton.onClick.RemoveAllListeners();
        currentText = text;
        if(currentText.Length>0)
        {
            multiFunctionalButton.onClick.AddListener(SendText);
            multiFunctionalButton.image.sprite = sendImage;
        }
        else
        {
            multiFunctionalButton.image.sprite = micImage;
            multiFunctionalButton.onClick.AddListener(RecordMode);
        }
    }

    private void SendText()
    {
        ChatInteractor.Instance.SendText(currentText);
        multiFunctionalButton.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        chatWindowPresenter.CreateMessages(currentText);
        loader.SetActive(true);
    }

    private void RecordMode()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("Stop recording");
          
            byte[] bytes = WavToMp3.ConvertWavToMp3(clientAudio, 48);
            ChatInteractor.Instance.SendAudio(bytes);
            chatWindowPresenter.CreateMessages("");
            multiFunctionalButton.gameObject.SetActive(false);
            loader.SetActive(true);
        }
        else
        {
            Debug.Log("Start recording");
            clientAudio = Microphone.Start(null, false,3, 32000);
            inputField.gameObject.SetActive(false);
            multiFunctionalButton.gameObject.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private AudioClip TrimSilence(AudioClip oldAudioclip)
    {
        AudioClip result = SavWav.TrimSilence(oldAudioclip, 0.5f);
        return result;
    }
}
