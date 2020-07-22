using System;
using System.Dynamic;
using Rhino.Geometry;
using Newtonsoft.Json;

namespace RCva3c
{
    public class va3c_Views
    {
        /// <summary>
        /// Generates element for the view
        /// </summary>
        /// <param name="eye">Position of the viewer</param>
        /// <param name="target">Position of the target</param>
        /// <param name="name">Name of this camera</param>
        /// <returns></returns>
        public Element GenerateViewElement(Point3d eye, Point3d target, string name = "")
        {
            //get user inputs
            //user should be able to create a scene contianing only lines, or only meshes, or both.  All geo and material inputs will be optional, and we'll run some defense.
            try
            {
                //create json from lists of json:
                string outJSON = pointJSON(eye, target, name);
                outJSON = outJSON.Replace("OOO", "object");

                Element e = new Element(outJSON, va3cElementType.Camera);
                return e;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string pointJSON(Point3d E, Point3d T, string name)
        {
            //create a dynamic object to populate
            dynamic jason = new ExpandoObject();

            //top level properties
            jason.uuid = Guid.NewGuid();
            jason.name = name;

            jason.eye = new ExpandoObject();
            //populate data object properties
            jason.eye.X = Math.Round(E.X * -1, 5);
            jason.eye.Y = Math.Round(E.Z, 5);
            jason.eye.Z = Math.Round(E.Y, 5);

            jason.target = new ExpandoObject();
            //populate data object properties
            jason.target.X = Math.Round(T.X * -1, 5);
            jason.target.Y = Math.Round(T.Z, 5);
            jason.target.Z = Math.Round(T.Y, 5);

            //return
            return JsonConvert.SerializeObject(jason);
        }
    }
}