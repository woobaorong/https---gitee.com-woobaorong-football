

using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class TUIOClient : MonoBehaviour
{
    public int port = 3333; // TUIO 默认端口
    public static UdpClient udpClient;
    private Thread receiveThread;

    // 保存当前触摸点
    private Dictionary<int, Vector2> cursors = new Dictionary<int, Vector2>();

    void Start()
    {
        if (udpClient == null)
        {
            udpClient = new UdpClient(port);
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();
            Debug.Log("TUIO Receiver started on port " + port);
        }
    }

    void ReceiveData()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

        while (true)
        {
            try
            {
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                ParseOscMessage(data);
            }
            catch (System.Exception e)
            {
                Debug.Log("Error receiving data: " + e.Message);
            }
        }
    }

    void ParseOscMessage(byte[] data)
    {

        var frame = TuioParser.Parse(data);

        // 获取alive触点
        foreach (int id in frame.AliveIDs)
        {
            Debug.Log("Alive ID: " + id);
        }

        // 获取set触点
        foreach (var cursor in frame.Cursors)
        {
            Debug.Log($"Set ID:{cursor.SessionID} x:{cursor.X} y:{cursor.Y} vx:{cursor.XVel} vy:{cursor.YVel} MotionAccel:{cursor.MotionAccel}");
        }

        // 当前帧序号
        Debug.Log("Frame: " + frame.FrameSeq);


        // 用简单的ASCII解析OSC地址
        // string msg = Encoding.ASCII.GetString(data);
        // if (msg.Contains("/tuio/2Dcur"))
        // {
        //     Debug.Log("Received TUIO 2Dcur message: " + msg);
        // }
    }

    void Update()
    {
        // 这里可以根据cursors字典来控制Unity里的物体
        foreach (var cursor in cursors)
        {
            Debug.Log("Cursor ID: " + cursor.Key + " Position: " + cursor.Value);
        }
    }

    private void OnApplicationQuit()
    {
        if (receiveThread != null)
            receiveThread.Abort();
        udpClient.Close();
    }
}
