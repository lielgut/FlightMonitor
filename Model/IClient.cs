namespace ex1.Model
{
    // interface for implementing simple client-server communication
    interface IClient
    {
        // property for destination port
        public int DestPort { get; set; }
        // connect to server (returns false if failed)
        public bool connect();
        // send a string to server (returns false if failed)
        public bool send(string data);
        // terminate connection
        public void close();
    }
}
