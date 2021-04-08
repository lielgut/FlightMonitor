using System;
using System.Collections.Generic;
using System.Text;

namespace ex1.Model
{
    class PathInfo
    {
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
    }
}