using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomMenuPresenter : MonoBehaviour
{
    void Start()
    {
        ChatInteractor.Instance.SendText("Привет");
    }

}
