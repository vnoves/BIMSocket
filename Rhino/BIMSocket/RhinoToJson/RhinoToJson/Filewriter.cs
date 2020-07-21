using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.IO;

namespace RhinoToJson
{
    class Filewriter
    {
        public void SaveFileTo(List<String> lines)
        {
            var filter = "Text Files(*.json)|*.json|All(*.*)|*";
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = filter
            };
            dialog.FileName = "scene";
            if (dialog.ShowDialog() == true)
            {
                using (StreamWriter writer = new StreamWriter(dialog.FileName))
                {
                    if (lines.Count != 0) //Error Handling
                    {
                        foreach (String s in lines)
                        {
                            writer.WriteLine(s);
                        }
                    }
                }
            }
        }
    }
}
