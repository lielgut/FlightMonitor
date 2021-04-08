using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ex1.Model
{
    class SimpleClient : IClient
    {
        private int destPort;
        int IClient.DestPort
        {
            get
            {
                return destPort;
            }
            set
            {
                this.destPort = value;
            }
        }
        private TcpClient cl;
        private Stream stream;
        private ASCIIEncoding enc;

        public SimpleClient()
        {
            this.cl = new TcpClient();
            this.enc = new ASCIIEncoding();
        }

        public bool connect()
        {
            try
            {
                cl.Connect("127.0.0.1", destPort);
                this.stream = cl.GetStream();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public bool send(string data)
        {
            try
            {
                string s = data + "\r\n";
                byte[] msg = enc.GetBytes(s);
                stream.Write(msg, 0, msg.Length);
                return true;
            }
            // catch exception if server closes
            catch
            {
                return false;
            }
        }

        public void close()
        {
            stream.Close();
            cl.Close();
        }
    }
}
