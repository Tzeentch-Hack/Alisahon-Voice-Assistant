using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    private ChatInteractor chatInteractor;
    private ChatGateway chatGateway;
    void Awake()
    {
        chatInteractor = new ChatInteractor();
        chatGateway = new ChatGateway();
    }
}
