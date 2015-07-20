using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XML_to_CSV
{
    class Program
    {
        static void Main(string[] args)
        {
            Converter c = new Converter();
            c.readFile();
            c.fileCreate();
        }
    }
}
