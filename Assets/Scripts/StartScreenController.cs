using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField _ipInput = null;

    [SerializeField]
    private TMP_InputField _nameInput = null;

    [SerializeField]
    private NetworkInformation _networkInformation = null;


    public void BuildConfig()
    {
        _networkInformation.ip = _ipInput.text;
        _networkInformation.myName = _nameInput.text;
        if (NetworkService.Instance.TrySetNetworkInformation(_networkInformation))
        {
            SceneManager.LoadScene("Main");
        }
    }
}
