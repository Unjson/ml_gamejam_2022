using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup _waitingForOpponent = null;

    [SerializeField]
    private GameObject _debugCanvas = null;
    

    internal void OnStateChanged(GameController.GameState state)
    {
        switch (state)
        {
            case GameController.GameState.Searching:
                _waitingForOpponent.alpha = 1f;
                _debugCanvas.SetActive(false);
                break;
            default:
                _waitingForOpponent.DOFade(0f, 0.4f).OnComplete(() => _waitingForOpponent.gameObject.SetActive(false));
                _debugCanvas.SetActive(true);
                break;
                
        }
    }
}
