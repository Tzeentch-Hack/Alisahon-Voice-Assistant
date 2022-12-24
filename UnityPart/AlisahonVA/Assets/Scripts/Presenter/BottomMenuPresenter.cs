using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BottomMenuPresenter : MonoBehaviour
{
    [SerializeField]
    private Button multiFunctionalButton;

    AudioClip clientAudio;

    private void SetupMenu()
    {
        multiFunctionalButton.onClick.AddListener(RecordMode);
    }

    void Start()
    {
        SetupMenu();
        //ChatInteractor.Instance.SendText("Привет");
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
