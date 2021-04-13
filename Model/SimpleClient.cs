using System;
using System.Text;
using System.IO;
using System.Net.Sockets;

namespace ex1.Model
{
    // class for implementing the IClient interface
    class SimpleClient : IClient
    {
        // property for destination port
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

        // the client
        private TcpClient cl;
        // server-client stream
        private Stream stream;
        // encoding
        private ASCIIEncoding enc;

        // constructor
        public SimpleClient()
        {
            this.cl = null;
            this.enc = new ASCIIEncoding();
        }

        // connect to server (returns false if failed)
        public bool connect()
        {
            try
            {
                cl = new TcpClient();
                cl.Connect("127.0.0.1", destPort);
                this.stream = cl.GetStream();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // send a string to server
        public bool send(string data)
        {
            try
            {
                string s = data + "\r\n";
                byte[] msg = enc.GetBytes(s);
                stream.Write(msg, 0, msg.Length);
                return true;
            }
            // return false if failed to send message
            catch (Exception)
            {
                return false;
            }
        }

        // terminate connection
        public void close()
        {
            stream.Close();
            cl.Close();
        }
    }
}
