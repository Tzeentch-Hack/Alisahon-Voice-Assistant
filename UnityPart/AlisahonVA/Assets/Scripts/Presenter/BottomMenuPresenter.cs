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
    private TMP_InputField inputField;

    string currentText;
    AudioClip clientAudio;

    private void SetupMenu()
    {
        multiFunctionalButton.onClick.AddListener(RecordMode);
        inputField.onValueChanged.AddListener(CheckInput);
    }

    void Start()
    {
        SetupMenu();
    }

    private void CheckInput(string text)
    {
        multiFunctionalButton.onClick.RemoveAllListeners();
        currentText = text;
        if(currentText.Length>0)
        {
          multiFunctionalButton.onClick.AddListener(SendText);
        }
        else
        {
            multiFunctionalButton.onClick.AddListener(RecordMode);
        }
    }

    private void SendText()
    {
        ChatInteractor.Instance.SendText(currentText);
    }

    private void RecordMode()
    {
        if (Microphone.IsRecording(null))
        {
            Microphone.End(null);
            Debug.Log("Stop recording");
            //byte[] bytes = Convert(audio);
            byte[] bytes = WavToMp3.ConvertWavToMp3(clientAudio, 128);
            ChatInteractor.Instance.SendAudio(bytes);
        }
        else
        {
            Debug.Log("Start recording");
            clientAudio = Microphone.Start(null, false, 5, 32000);
        }
    }
}
