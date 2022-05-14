using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Network Information", menuName = "ScriptableObjects/NetworkInformation")]
public class NetworkInformation : ScriptableObject
{
    public string ip;
    public int port;
    public string myName;
    public string enemyName;
    public string GetAddress()
    {
        return ip + ":" + port;
    }
}
