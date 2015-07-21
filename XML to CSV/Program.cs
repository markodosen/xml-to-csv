using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_to_CSV
{
    class Program
    {
        static void Main(String[] arg)
        {
            //list of attributes that we need to display
            List<String> list = new List<String>();
            list.Add("Payment Amount");
            list.Add("Subtotal");
            list.Add("Shipping");            
            list.Add("Tax");
            list.Add("Address3");
            

            //if user doesn't put file path as argument, ask him to put it.
            Converter c = new Converter(list);
            if (arg.Length < 1)
            {
                Console.WriteLine("Please input file path:");
                c.filePath = Console.ReadLine();
            }
            else { 
                c.filePath = arg[0];
            }
            //check file if it exists
            c.checkFile();

            //create scv file from file that was red
            c.fileCreate();
            
        }
    }
}
