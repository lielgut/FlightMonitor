﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    interface IClient
    {
        public int DestPort { get; set; }
        public void connect();
        public void send(string data);
        public void close();
    }
}