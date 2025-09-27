using UnityEngine;
using System;
using System.Collections.Generic;
using System.Text;

public class TuioParser
{
    public class TuioCursor
    {
        public int SessionID;
        public float X;
        public float Y;
        public float XVel;
        public float YVel;
        public float MotionAccel;
    }

    public class TuioFrame
    {
        public List<int> AliveIDs = new List<int>();
        public List<TuioCursor> Cursors = new List<TuioCursor>();
        public int FrameSeq;
    }

    /// <summary>
    /// 解析一个TUIO消息（byte[]），返回解析结果
    /// </summary>
    public static TuioFrame Parse(byte[] data)
    {
        TuioFrame frame = new TuioFrame();

        int offset = 0;
        string bundle = ReadOscString(data, ref offset);
        if (bundle != "#bundle")
        {
            Debug.LogWarning("Not an OSC bundle");
            return frame;
        }

        // 跳过bundle时间戳8字节
        offset += 8;

        while (offset < data.Length)
        {
            // 每条message前有长度(int32)
            int msgLength = ReadInt(data, ref offset);
            int msgStart = offset;
            int msgEnd = offset + msgLength;

            string address = ReadOscString(data, ref offset); // /tuio/2Dcur
            string types = ReadOscString(data, ref offset);   // ,sifffffii

            // 处理参数
            if (address == "/tuio/2Dcur")
            {
                char[] t = types.ToCharArray();
                int argIndex = 1; // 从1开始跳过','

                // 第一个参数是字符串：set / alive / fseq
                string cmd = ReadOscString(data, ref offset);

                if (cmd == "alive")
                {
                    // 后面全是int
                    while (offset < msgEnd)
                    {
                        int id = ReadInt(data, ref offset);
                        frame.AliveIDs.Add(id);
                    }
                }
                else if (cmd == "set")
                {
                    TuioCursor c = new TuioCursor();
                    c.SessionID = ReadInt(data, ref offset);  // session id
                    c.X = ReadFloat(data, ref offset);
                    c.Y = ReadFloat(data, ref offset);
                    c.XVel = ReadFloat(data, ref offset);
                    c.YVel = ReadFloat(data, ref offset);
                    c.MotionAccel = ReadFloat(data, ref offset);
                    frame.Cursors.Add(c);
                }
                else if (cmd == "fseq")
                {
                    frame.FrameSeq = ReadInt(data, ref offset);
                }
            }

            offset = msgEnd; // 跳到下一条消息
        }

        return frame;
    }

    // 读取一个OSC字符串（以0结尾并4字节对齐）
    private static string ReadOscString(byte[] data, ref int offset)
    {
        int start = offset;
        while (offset < data.Length && data[offset] != 0) offset++;
        string s = Encoding.ASCII.GetString(data, start, offset - start);
        // 跳过\0并对齐到4字节
        while ((offset % 4) != 0) offset++;
        return s;
    }

    private static int ReadInt(byte[] data, ref int offset)
    {
        int value = (data[offset] << 24) | (data[offset + 1] << 16) | (data[offset + 2] << 8) | data[offset + 3];
        offset += 4;
        return value;
    }

    private static float ReadFloat(byte[] data, ref int offset)
    {
        if (BitConverter.IsLittleEndian)
        {
            byte[] tmp = new byte[4];
            tmp[0] = data[offset + 3];
            tmp[1] = data[offset + 2];
            tmp[2] = data[offset + 1];
            tmp[3] = data[offset];
            offset += 4;
            return BitConverter.ToSingle(tmp, 0);
        }
        else
        {
            float f = BitConverter.ToSingle(data, offset);
            offset += 4;
            return f;
        }
    }
}
