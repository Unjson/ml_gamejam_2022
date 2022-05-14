using medialesson.Library.IoC;
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

    private NetworkInformation info = null;

    public bool TrySetNetworkInformation(NetworkInformation networkInformation)
    {
        info = networkInformation; 

        _sender.ip = info.ip;
        _reciever.ip = info.ip;
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
}
