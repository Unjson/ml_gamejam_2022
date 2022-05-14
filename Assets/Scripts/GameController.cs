using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Searching,
        Waiting,
        Ready,
        Shoot,
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
    public Coroutine _duelCR = null;

    [SerializeField]
    private NetworkInformation _networkInformation;
    public string MyName => _networkInformation.myName;
    public string OpponentName => _networkInformation.enemyName;

    [SerializeField]
    private NetworkEvent _sendEvent = null, _receiveEvent = null;

    [SerializeField]
    private CanvasController _canvasController = null;

    [SerializeField]
    private List<KeyCode> _validKeyCodes = new List<KeyCode>();

    private bool isMaster = false;

    private KeyCode _triggerKey = KeyCode.None;
    private long _triggerTime = long.MaxValue;

    private void Start()
    {
        State = GameState.Searching;
        _receiveEvent.Add(OnRecieved);
        
        _waitingCR = StartCoroutine(WaitForOpponent());
    }

    private void OnRecieved(NetworkMessage msg)
    {
        switch (msg.messageType)
        {
            case NetworkMessage.MessageType.Hi:
                OnHandshakeRecieved(msg);
                break;
            case NetworkMessage.MessageType.SendData:
                OnDuelDataRecieved(msg);
                break;
        }
    }

    private void OnDuelDataRecieved(NetworkMessage msg)
    {
        if(_duelCR != null)
        {
            StopCoroutine(_duelCR);
        }

        State = GameState.Ready;
        _triggerKey = msg.triggerKey;
        _triggerTime = msg.triggerFiletime;
        _duelCR = StartCoroutine(PrepareDuel());
    }

    private void OnHandshakeRecieved(NetworkMessage msg)
    {
        if(_waitingCR != null)
        {
            StopCoroutine(_waitingCR);
        }
        if (State != GameState.Searching) return;

        State = GameState.Waiting;

        _networkInformation.enemyName = msg.senderName;

        var shake = new NetworkMessage();
        msg.messageType = NetworkMessage.MessageType.Hi;
        _sendEvent.Raise(shake);

        DetermineMasterPlayer();

        if (isMaster)
        {
            ChooseNewDuel();
        }
    }

    private void ChooseNewDuel()
    {
        var msg = new NetworkMessage();

        var inSeconds = UnityEngine.Random.Range(3f, 10f);
        var randPos = UnityEngine.Random.Range(0, _validKeyCodes.Count);
        msg.triggerFiletime = DateTime.Now.AddSeconds(inSeconds).ToFileTime();
        msg.triggerKey = _validKeyCodes[randPos];

        _sendEvent.Raise(msg);
        _receiveEvent.Raise(msg);
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
            if(State != GameState.Searching) break; 
            _sendEvent.Raise(msg);

            yield return new WaitForSeconds(1);
        }
    }

    private void DetermineMasterPlayer()
    {
        isMaster = MyName.GetHashCode() >= OpponentName.GetHashCode();
    }

    private IEnumerator PrepareDuel()
    {
        while(DateTime.Now.ToFileTime() < _triggerTime)
        {
            yield return new WaitForEndOfFrame();
        }
        State = GameState.Shoot;
    }
}
