namespace ex1.Model
{
    // class for sending the data to FlightGear via the client
    abstract class Pilot
    {
        // interface of the client
        protected IClient cl;

        // destination port of FlightGear
        public int DestPort
        {
            get
            {
                return cl.DestPort;
            }
            set
            {
                cl.DestPort = value;
            }
        }

        // connect the client to server
        public bool startClient()
        {
            return cl.connect();
        }

        // close client connection
        public void endClient()
        {
            cl.close();
        }

        // add a string to the data that will be sent to server
        public abstract void addLine(string s);

        // get string of given timestep and send it via client
        public abstract bool sendCurrentData(int timestep);

        // clear loaded data
        public abstract void reset();
    }
}
