using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XML_to_CSV
{
    class Converter
    {
        private String fileName { get; set; }
        private String filePath { get; set; }
        private XmlDocument doc = new XmlDocument();
        private String attributes = "";

        //read file from file path that is inserted through console. Also saves file name.
        public void readFile()
        {
            Console.WriteLine("Please input file path:");
            filePath = Console.ReadLine();
            fileName = filePath.Substring(filePath.LastIndexOf('\\')+1);
            try
            {
                doc.Load(filePath);
            }
            catch (System.IO.FileNotFoundException) { }
            
        }

        //Display all nodes. Used just to test if file was read as it should.
        public void displayXMLfile()
        {
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                string text = node.InnerText;
                Console.WriteLine(text);
            }
        }

        //extracting data from xml
        public String extractOutput()
        {
            String output =""; //string that will be used to save values
            XmlTextReader reader = new XmlTextReader(filePath);
            while (reader.Read())
            {
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element: // The node is an element.
                        if (reader.Name.Equals("row")) //when we get to new row, add \n so we can separate individual rows
                        {
                            output = output + '\n';
                        }
                        if (reader.AttributeCount > 0)
                        {
                            
                            if (!attributes.Contains(reader.GetAttribute(0)))  //adding attributes to the string. Attributes will be added only if they are not already put in
                            {
                                attributes = attributes + (reader.GetAttribute(0) + ',');  
                            }
                        }
                                               
                        break;
                    case XmlNodeType.Text: //Display the text in each element.
                        output = output + reader.Value + ','; //reading the value
                        break;
                    case XmlNodeType.EndElement: //Display the end of the element.
                        break;
                }

            }
            reader.Close();
            return (attributes + output);
            
        }
        //create csv file at the same location as xml, but keep both files.
        public void fileCreate()
        {
            String newFileDestination = filePath.Replace(".xml", ".csv");
            StreamWriter file = new StreamWriter(newFileDestination);
            file.WriteLine(extractOutput());
            file.Close();
        }

    }
}
