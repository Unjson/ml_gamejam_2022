using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UDPTester : MonoBehaviour
{
    [SerializeField]
    private NetworkEvent _recieveEvent = null, _sendEvent = null;

    [SerializeField]
    private TMP_InputField _recieveText = null, _sendMessage = null;

    [SerializeField]
    private Button _sendButton = null;


    // Start is called before the first frame update
    void Start()
    {
        _recieveEvent.Add(OnRecieved);
        _sendButton.onClick.AddListener(Send);
    }

    private void Send()
    {
        var msg = new NetworkMessage();
        msg.message = _sendMessage.text;
        _sendEvent.Raise(msg);
    }

    private void OnRecieved(NetworkMessage msg)
    {
        _recieveText.text = $"New message! \n GUID: {msg.guid}\n Name: {msg.senderName}\n Message: {msg.message}";
    }
}
