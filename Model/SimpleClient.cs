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
        int IClient.DestPort {
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

        public SimpleClient(int portnum)
        {
            this.destPort = portnum;
            this.cl = new TcpClient();
            this.enc = new ASCIIEncoding();
        }

        public void connect()
        {
            cl.Connect("127.0.0.1", destPort);
            this.stream = cl.GetStream();
        }

        public void send(string data)
        {
            string s = data + "\r\n";
            byte[] msg = enc.GetBytes(s);
            stream.Write(msg, 0, msg.Length);
        }

        public void close()
        {
            stream.Close();
            cl.Close();            
        }
    }
}
