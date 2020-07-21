/*  HEADER
 * 
 * ELEMENT CLASS  
 *  A high level class to inherit from.  Provides fields that all model elements must have.
 *  
 *  3/3/15
 * Ana Garcia Puyol
 */

using System;

namespace RCva3c
{

    public class Layer
    {
        private string myID;
        public string Name { get; set; }

        public string ID
        {
            get { return myID; }
            set
            {
                try
                {
                    //test for the empty string
                    if (value == "")
                    {
                        throw new ArgumentException("The input string cannot be empty");
                    }

                    myID = value;
                }

                catch (Exception e) //should catch the null case
                {
                    throw e;
                }
            }
        }

        public Layer() { }
        public Layer(string name)
        {
            Name = name;
        }


    }

   
}
