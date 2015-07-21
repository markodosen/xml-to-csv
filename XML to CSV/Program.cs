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
            LinkedList<String> list = new LinkedList<String>();
            list.AddFirst("Subtotal");
            list.AddLast("Shipping");
            list.AddLast("Tax");
            list.AddLast("Payment Amount");

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
            //readFile to check if it is ok
            c.readFile();

            //create scv file from file that was red
            c.fileCreate();
            
        }
    }
}
