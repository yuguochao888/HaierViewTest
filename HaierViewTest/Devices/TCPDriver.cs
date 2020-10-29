using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HaierViewTest.Devices
{
    public class SocketClient
    {
        /// <summary>
        /// IP地址
        /// </summary>
        public string  IP { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { set; get; }

        /// <summary>
        /// 连接状态
        /// </summary>
        public bool IsConnect { set; get; }

        private bool ConnectStatus = false;
        private Socket newclient;//= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private byte[] buffer = new byte[1024];


        ~SocketClient()
        {
            Close();
        }
        public bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }


        public void ByteMencpy(byte[] bufferDart, byte[] bufferOrig, int lend)
        {
            for (int i = 0; i < lend; i++)
            {
                bufferDart[i] = bufferOrig[i];

            }
        }
        public bool Connect(string ipString, int port)
        {
            IPEndPoint ie;
            if (IsIP(ipString))
            {
                ie = new IPEndPoint(IPAddress.Parse(ipString), port);
            }
            else
            {
                ie = new IPEndPoint(
                Dns.GetHostEntry(ipString).AddressList[0], port);
            }


            try
            {
                newclient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //newclient.BeginDisconnect(true, new AsyncCallback(DisconnectCallback), newclient);
                newclient.Connect(ie);
                newclient.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), newclient);
            }
            catch (SocketException e)
            {
              //  Log.Trace("DEBUG", "unable to connect to server");
               // Log.Trace("DEBUG", e.ToString());
                return false;
            }
            ConnectStatus = true;


            return true;


        }
        void ReceiveCallback(IAsyncResult result)
        {
            try
            {
                Socket ts = (Socket)result.AsyncState;
                int len = ts.EndReceive(result);
                result.AsyncWaitHandle.Close();
                if (len > 0)
                {
                    //  string receivStr =Convert.ToString   Encoding.ASCII.GetString(buffer);
                    //Encoding.ASCII.GetString(buffer)
                    byte[] bufferTemp = new byte[100];
                    ByteMencpy(bufferTemp, buffer, 99);
                 //  Log.Trace("DEBUG", "收到消息：" + CMyString.ToHexString(bufferTemp));
                    //buffer = new byte[buffer.Length];
                    ts.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), ts);
                }
                else
                {
                  //  Log.Trace("DEBUG", "收到消息长度为0 断开");
                    Close();
                }


            }
            catch (SocketException e)
            {
              //  Log.Trace("ERROR", "接收异常:" + e.ToString());
            }
        }
        public bool Send(string sendStr)
        {
            byte[] bs = Encoding.ASCII.GetBytes(sendStr);
           // Log.Trace("DEBUG", "发送消息：" + Encoding.ASCII.GetString(bs));
            newclient.Send(bs);
            return true;
        }


        public bool Send(byte[] bs)
        {
            //  Log.Trace("DEBUG", "发送消息：" + Encoding.ASCII.GetString(bs));


            int len = -1;
            try
            {
                len = newclient.Send(bs);
            }
            catch
            {

            }
            if (len > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public int Receive(byte[] recvBytes)
        {
            int bytes;
            bytes = newclient.Receive(recvBytes, recvBytes.Length, 0);
            return bytes;
        }
        public bool Close()
        {
            try
            {
                newclient.Shutdown(SocketShutdown.Both);
                newclient.Close();
            }
            catch
            {


            }
            ConnectStatus = false;
            return true;
        }


        public static void test()
        {
            SocketClient client = new SocketClient();
            if (client.Connect("127.0.0.1", 9999))
            {
                byte[] bs = new byte[] { 0x01, 0x02, 0x03 };
                client.Send(bs);
                while (true)
                {
                    Thread.Sleep(2);
                }
            }
            else
            {
                //Log.Trace("DEBUG", "链接服务器失败");
            }
        }
    }
}
