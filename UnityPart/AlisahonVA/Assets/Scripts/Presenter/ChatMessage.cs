using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI messageText;

    [SerializeField]
    private GameObject loader;

    public void Awake()
    {
        messageText.gameObject.SetActive(false);
        loader.SetActive(true);
    }

    public void SetText(string text)
    {
        messageText.gameObject.SetActive(true);
        loader.SetActive(false);
        messageText.text = text;
    }
}
