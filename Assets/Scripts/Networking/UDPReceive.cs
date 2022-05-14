using UnityEngine;
using System.Collections;
 
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{
    NetworkService service;
    UdpClient clientData;
    public int port = 8008;
    public int receiveBufferSize = 120000;

    [SerializeField]
    private bool showDebug = false;
    IPEndPoint ipEndPointData;
    private object obj = null;
    private System.AsyncCallback AC;
    byte[] receivedBytes;

    private void Start()
    {
        service = GetComponentInParent<NetworkService>();
    }

    public void Init()
    {
        ipEndPointData = new IPEndPoint(IPAddress.Any, port);
        clientData = new UdpClient();
        clientData.Client.ReceiveBufferSize = receiveBufferSize;
        clientData.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, optionValue: true);
        clientData.ExclusiveAddressUse = false;
        clientData.EnableBroadcast = true;
        clientData.Client.Bind(ipEndPointData);
        clientData.DontFragment = true;
        if (showDebug) Debug.Log("BufSize: " + clientData.Client.ReceiveBufferSize);
        AC = new System.AsyncCallback(ReceivedUDPPacket);
        clientData.BeginReceive(AC, obj);
        Debug.Log("UDP - Start Receiving..");
    }

    void ReceivedUDPPacket(System.IAsyncResult result)
    {
        receivedBytes = clientData.EndReceive(result, ref ipEndPointData);
        ParsePacket();
        clientData.BeginReceive(AC, obj);
    } 

    void ParsePacket()
    {
        string s = Encoding.UTF8.GetString(receivedBytes);
        service.OnNewPacketRecieved(s);
    }

    void OnDestroy()
    {
        if (clientData != null)
        {
            clientData.Close();
        }
    }
}