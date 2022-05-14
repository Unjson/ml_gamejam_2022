using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPSend : MonoBehaviour
{
    // prefs
    public string ip;
    public int port; 

    // "connection" things
    IPEndPoint remoteEndPoint;
    UdpClient client;

    // init
    public void Init()
    {
        // ----------------------------
        // Senden
        // ----------------------------
        remoteEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        client = new UdpClient();
    }

    // sendData
    public void SendString(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            client.Send(data, data.Length, remoteEndPoint);
        }
        catch (Exception err)
        {
            Debug.Log(err.ToString());
        }
    }
}