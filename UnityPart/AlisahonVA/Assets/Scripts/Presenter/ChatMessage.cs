using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI messageText;

    public void SetText(string text)
    {
        messageText.text = text;
    }
}
