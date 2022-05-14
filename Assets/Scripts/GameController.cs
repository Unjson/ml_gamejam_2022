using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Waiting,
        Ready,
        Listening,
        Conclusion
    }
    [SerializeField]
    private GameState _state;
    public GameState State
    {
        get { return _state; }
        set 
        {
            if (value != _state)
            {
                _state = value;
                OnStateChanged();
            }
        }
    }

    private Coroutine _waitingCR = null;

    [SerializeField]
    private NetworkInformation _networkInformation;
    public string OpponentName => _networkInformation.enemyName; 

    [SerializeField]
    private NetworkEvent _sendEvent = null, _receiveEvent = null;

    [SerializeField]
    private CanvasController _canvasController = null;

    private bool isMaster = false;

    private void Start()
    {
        State = GameState.Waiting;
        _receiveEvent.Add(OnRecieved);
        
        _waitingCR = StartCoroutine(WaitForOpponent());
    }

    private void OnRecieved(NetworkMessage msg)
    {
        switch (msg.messageType)
        {
            case NetworkMessage.MessageType.Hi:
            case NetworkMessage.MessageType.Shake:
                OnHandshakeRecieved(msg);
                break;
        }
    }

    private void OnHandshakeRecieved(NetworkMessage msg)
    {
        if(_waitingCR != null)
        {
            StopCoroutine(_waitingCR);
        }

        _networkInformation.enemyName = msg.senderName;

        var shake = new NetworkMessage();
        msg.messageType = NetworkMessage.MessageType.Shake;
        _sendEvent.Raise(shake);

        DetermineMasterPlayer();

        State = GameState.Ready;
    }

    private void OnStateChanged()
    {
        _canvasController.OnStateChanged(_state);
    }

    private IEnumerator WaitForOpponent()
    {
        var msg = new NetworkMessage();
        msg.messageType = NetworkMessage.MessageType.Hi;
        while (true)
        {
            if(State != GameState.Waiting) { yield break; }

            _sendEvent.Raise(msg);
            yield return new WaitForSeconds(1);
        }
    }

    private void DetermineMasterPlayer()
    {
        var myHash = _networkInformation.myName.GetHashCode();
        var theirHash = _networkInformation.enemyName.GetHashCode();
        isMaster = myHash > theirHash;
    }
}
