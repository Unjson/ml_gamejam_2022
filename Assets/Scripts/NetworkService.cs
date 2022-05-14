using medialesson.Library.IoC;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkService : Singleton<NetworkService>
{
    [SerializeField]
    private NetworkEvent _sendEvent = null, _recieveEvent = null;

    [SerializeField]
    private UDPSend _sender = null;

    [SerializeField]
    private UDPReceive _reciever = null;

    [SerializeField]
    private int _resendFrames = 10;

    private NetworkInformation info = null;
    private NetworkMessage _lastRecievedMessage = new NetworkMessage();


    private void Start()
    {
        DontDestroyOnLoad(this);
        _sendEvent.Add(OnMessageSendRequested);
    }

    private void OnMessageSendRequested(NetworkMessage msg)
    {
        if (string.IsNullOrWhiteSpace(msg.senderName))
        {
            msg.senderName = info.myName;
        }

        var json = JsonConvert.SerializeObject(msg);
        StartCoroutine(SendOverTimeFrame(json, _resendFrames));
    }
    private IEnumerator SendOverTimeFrame(string json, int frames)
    {
        for (int i = 0; i < frames; i++)
        {
            _sender.SendString(json);
            yield return null;
        }
    }

    public bool TrySetNetworkInformation(NetworkInformation networkInformation)
    {
        info = networkInformation; 

        _sender.ip = info.ip;
        _sender.port = info.port;
        _reciever.port = info.port;

        try
        {
            _sender.Init();
            _reciever.Init();
        }
        catch (Exception ex)
        {
            Debug.LogWarning("Could not Initialize network:" + ex.Message);
            return false;
        }
        return true;
    }

    public void OnNewPacketRecieved(string text)
    {
        var msg = JsonConvert.DeserializeObject<NetworkMessage>(text);
        if(msg != null)
        {
            if(msg.guid != _lastRecievedMessage.guid)
            {
                _lastRecievedMessage = msg;
                _recieveEvent.Raise(msg);
            }
        }
    }
}
