using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BIMSocket.Utils
{
    class LocalFiles
    {
        private static string assemblyPath { get; set; }

        public LocalFiles() {
            assemblyPath = Directory.GetCurrentDirectory();


}
        public static string getJson3DPath()
        {
            var s = Assembly.GetExecutingAssembly().Location;
            string path = Directory.GetParent(s) + "\\BIMSocket.json";
            return path;
        }


        public static string getCredentialsPath()
        {
            var s = Assembly.GetExecutingAssembly().Location;
            string path = Directory.GetParent(s) + "\\credentials.json";
            return path;
        }
    }
}
