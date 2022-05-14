using medialesson.Library.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Network Event", menuName = "ScriptableObjects/NetworkEvent")]
public class NetworkEvent : EventRelay<NetworkMessage>
{

}

public class NetworkMessage
{
    public enum MessageType
    {
        Hit,
        Miss,
        Die,
        Bye
    }
    public MessageType messageType;
    public int guid;
    public string message;
}

