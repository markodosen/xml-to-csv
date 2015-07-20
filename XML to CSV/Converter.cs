using System;
using System.IO;
using System.Xml;

namespace XML_to_CSV
{
    class Converter
    {
        private String filePath { get; set; }
        private XmlDocument doc = new XmlDocument();
        private String attributes = "";
        public Converter(){

        }

        //read file from file path that is inserted through console. Also saves file name.
        public void readFile()
        {
            Console.WriteLine("Please input file path:");
            filePath = Console.ReadLine();
            try
            {
                doc.Load(filePath);
            }
            catch (System.Exception) {
                Environment.Exit(0);
            }
        }

        //extracting data from xml
        public String extractOutput()
        {
            String output =""; //string that will be used to save values
            XmlTextReader reader = new XmlTextReader(filePath);
            int i = 1;
            while (reader.Read())
            {
               // switch (reader.NodeType)
              //  {
                  //  case XmlNodeType.Element: // The node is an element.

                if (i == 1 && reader.Name.Equals("row")) //when we get to new row, add \n so we can separate individual rows
                        {
                            output = output + '\n';
                            i = 0;
                        }
                        else {if(reader.Name.Equals("row")) i++; }
                        if (reader.AttributeCount > 0)
                        {
                            //adding attributes to the string. Attributes will be added only if they are not already put in
                            if ((reader.GetAttribute(0).ToString().Equals("Subtotal")
                        || reader.GetAttribute(0).ToString().Equals("Shipping") || reader.GetAttribute(0).ToString().Equals("Tax")
                        || reader.GetAttribute(0).ToString().Equals("Payment Amount"))  && !attributes.Contains(reader.GetAttribute(0)))  
                            {
                                attributes = attributes + (reader.GetAttribute(0) + ',');
                                
                            }

                            //reading numbers from element that have selected atribute.
                            if(reader.GetAttribute(0).ToString().Equals("Subtotal")
                        || reader.GetAttribute(0).ToString().Equals("Shipping") || reader.GetAttribute(0).ToString().Equals("Tax")
                        || reader.GetAttribute(0).ToString().Equals("Payment Amount"))
                            {
                                output = output + reader.ReadElementContentAsDouble() + ',';
                            }
                        }
                
                                               
                      //  break;
                    //case XmlNodeType.Text: //Display the text in each element. might be needed for later
                        //reading the value
                     //   break;
                    //case XmlNodeType.EndElement: //Display the end of the element.
                   //     break;
                //}

            }
            reader.Close();
            return (attributes + output);
            
        }
        //create csv file at the same location as xml, but keep both files.
        public void fileCreate()
        {
            String newFileDestination = filePath.Replace(".xml", ".csv");

            using (StreamWriter file = new StreamWriter(newFileDestination))
            {
                file.WriteLine(extractOutput());
            }
        }

    }
}
