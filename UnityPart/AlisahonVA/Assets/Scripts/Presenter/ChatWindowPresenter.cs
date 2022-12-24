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

    private void Start()
    {
        ChatInteractor.Instance.GetResponse.AddListener(RecieveResponse);
    }

    private void ClearAllMessages()
    {

    }

    private void RecieveResponse(DialogUIResponseModel dialogUIResponseModel)
    {
        CreateCLientMessage(dialogUIResponseModel.questionText);
        CreateServerMessage(dialogUIResponseModel.answerText);
    }

    private void CreateCLientMessage(string text)
    {
        GameObject newClientMessageObject = Instantiate(clientMessagePrefab, messageGrid.transform);
        ClientMessage newClientMessage = newClientMessageObject.GetComponent<ClientMessage>();
        newClientMessage.SetText(text);
    }

    private void CreateServerMessage(string text)
    {
        GameObject newClientMessageObject = Instantiate(serverMessagePrefab, messageGrid.transform);
        ServerMessage newClientMessage = newClientMessageObject.GetComponent<ServerMessage>();
        newClientMessage.SetText(text);
    }
}
