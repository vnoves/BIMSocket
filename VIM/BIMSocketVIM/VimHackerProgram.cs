using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Vim;
using Vim.DataFormat;
using Vim.DotNetUtilities;
using Vim.Geometry;
using Vim.LinqArray;
using Vim.Math3d;
using BIMSocket_VIM.Model;

namespace BIMSocket_VIM
{
    public static class VimHackerProgram
    {


        public static void Main(string[] args)
        {
            VIMManagement.TestJsonToVim(@"C:\Users\pderendinger\Desktop\BIMSocket.json", false);
            //System.AppDomain currentDomain = System.AppDomain.CurrentDomain;

            //currentDomain.AssemblyResolve += new ResolveEventHandler(currentDomain_AssemblyResolve);

            //if (FireBaseConnection.Connect("models", "FBK_01"))
            //{
            //    FireBaseConnection.ReceiveChangesFromDB();
            //}

            //System.Windows.Forms.Application.Run();

        }

        private static Assembly currentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Vim.DotNetUtilities"))
            {
                string filename = Path.GetDirectoryName(
                  System.Reflection.Assembly
                    .GetExecutingAssembly().Location);

                filename = Path.Combine(filename,
                  "Vim.DotNetUtilities.dll");

                if (File.Exists(filename))
                {
                    return System.Reflection.Assembly
                      .LoadFrom(filename);
                }
            }
            return null;
        }
    }
}
