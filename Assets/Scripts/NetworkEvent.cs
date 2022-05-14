using medialesson.Library.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Network Event", menuName = "ScriptableObjects/NetworkEvent")]
public class NetworkEvent : EventRelay<NetworkMessage>
{

}

[System.Serializable]
public class NetworkMessage
{
    public enum MessageType
    {
        Hi,
        SendData,
        Hit,
        Miss,
        Die,
        Bye
    }
    public MessageType messageType;
    public string senderName;
    public Guid guid = Guid.NewGuid();
    public long triggerFiletime;
    public KeyCode triggerKey;
}

