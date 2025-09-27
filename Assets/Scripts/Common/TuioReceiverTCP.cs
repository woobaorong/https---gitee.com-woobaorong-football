using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class TuioReceiverTCP : MonoBehaviour
{
    public string serverIP = "127.0.0.1";  // TUIO 服务端IP
    public int port = 3333;               // TUIO TCP端口

    private TcpClient tcpClient;
    private NetworkStream stream;
    private Thread receiveThread;

    void Start()
    {
        Connect();
    }

    void Connect()
    {
        try
        {
            tcpClient = new TcpClient(serverIP, port);
            stream = tcpClient.GetStream();

            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log("Connected to TUIO server " + serverIP + ":" + port);
        }
        catch (Exception e)
        {
            Debug.LogError("Connection error: " + e.Message);
        }
    }

    void ReceiveData()
    {
        byte[] buffer = new byte[4096];
        int bytesRead;

        while (true)
        {
            try
            {
                bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    byte[] data = new byte[bytesRead];
                    Array.Copy(buffer, data, bytesRead);
                    ParseOscMessage(data);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Receive error: " + e.Message);
                break;
            }
        }
    }

    void ParseOscMessage(byte[] data)
    {
        // 这里只是简单打印TUIO消息
        string msg = Encoding.ASCII.GetString(data);
        if (msg.Contains("/tuio/2Dcur"))
        {
            Debug.Log("Received TUIO 2Dcur message: " + msg);
        }
    }

    private void OnApplicationQuit()
    {
        if (receiveThread != null) receiveThread.Abort();
        if (stream != null) stream.Close();
        if (tcpClient != null) tcpClient.Close();
    }
}
