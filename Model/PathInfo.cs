using System;

namespace ex1.Model
{
    // class for saving paths of files loaded by the program
    class PathInfo
    {

        // path to normal flight CSV
        private String normalCSVPath;
        public String NormalCSVPath
        {
            get
            {
                return normalCSVPath;
            }
            set
            {
                normalCSVPath = value;
            }
        }

        // path to new flight CSV
        private String newCSVPath;
        public String NewCSVPath
        {
            get
            {
                return newCSVPath;
            }
            set
            {
                newCSVPath = value;
            }
        }

        // path to dll plugin
        private String dllPath;
        public String DLLPath
        {
            get
            {
                return dllPath;
            }
            set
            {
                dllPath = value;
            }
        }

        // FlightGear installation path
        private String fgPath;
        public String FGPath
        {
            get
            {
                return fgPath;
            }
            set
            {
                fgPath = value;
            }
        }

        // XML file path (names of features)
        private String xmlPath;
        public String XMLPath
        {
            get
            {
                return xmlPath;
            }
            set
            {
                xmlPath = value;
            }
        }
    }
}