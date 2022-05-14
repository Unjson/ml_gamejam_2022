using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UDPTester : MonoBehaviour
{
    [SerializeField]
    private NetworkEvent _recieveEvent = null;

    [SerializeField]
    private TMP_InputField _recieveText = null;


    // Start is called before the first frame update
    void Start()
    {
        _recieveEvent.Add(OnRecieved);
    }

    private void OnRecieved(NetworkMessage msg)
    {
        _recieveText.text = $"New message! \n GUID: {msg.guid}\n Name: {msg.senderName}\n Message: {msg.triggerKey} at {msg.triggerFiletime}";
    }
}
