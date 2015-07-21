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
             
            LinkedList<String> list = new LinkedList<string>();
            list.AddFirst("Subtotal");
            list.AddLast("Shipping");
            list.AddLast("Tax");
            list.AddLast("Payment Amount");
            Converter c = new Converter(list);
            c.readFile();
            c.fileCreate();
            
        }
    }
}
