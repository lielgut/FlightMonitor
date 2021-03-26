﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    interface IFlightData
    {
        // parse and add data
        public void addData(string s);
        // get value of feature at given timestep
        public float getValue(string featureName, int timestep);
    }
}
