using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWindowPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject serverMessagePrefab;
    [SerializeField]
    private GameObject clientMessagePrefab;

    [SerializeField]
    private GameObject messageGrid;

    private GameObject currentServerMessage;

    private GameObject currentClientMessage;

    private void Start()
    {
        ChatInteractor.Instance.GetResponse.AddListener(RecieveResponse);
        ChatInteractor.Instance.ServerError.AddListener(DestroyLastMessages);
    }

    private void ClearAllMessages()
    {

    }

    private void DestroyLastMessages()
    {
        Destroy(currentClientMessage);
        Destroy(currentServerMessage);
    }

    public void CreateMessages(string text)
    {
        currentClientMessage = Instantiate(clientMessagePrefab, messageGrid.transform);
        if (text != "")
        {
            currentClientMessage.GetComponent<ClientMessage>().SetText(text);
        }
        currentServerMessage = Instantiate(serverMessagePrefab, messageGrid.transform);
    }

    private void RecieveResponse(DialogUIResponseModel dialogUIResponseModel)
    {
        SetUpCLientMessage(dialogUIResponseModel.questionText);
        SetUpServerMessage(dialogUIResponseModel.answerText);
    }

    private void SetUpCLientMessage(string text)
    {
        currentClientMessage.GetComponent<ClientMessage>().SetText(text);
    }

    private void SetUpServerMessage(string text)
    {
        currentServerMessage.GetComponent<ServerMessage>().SetText(text);
    }
}
