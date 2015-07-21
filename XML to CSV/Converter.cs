using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Text;

namespace XML_to_CSV
{
    class Converter
    {

        //path from where file is loaded
        private String filePath { get; set; }
        //file that will be loaded if path is correct
        private XmlDocument doc = new XmlDocument(); 
        //header of new csv file
        private String header="";
        //list of all attributes that user is searching for
        private LinkedList<String> searchedAttributes; 
        

        /// <summary>
        /// Constructor for converter that require list of attributes that you want to search for
        /// </summary>
        /// <param name="searchedAttributes">contains list of attributes that you want to search for from xml file</param>
        public Converter(LinkedList<String> searchedAttributes)
        {
            this.searchedAttributes = searchedAttributes;
        }

        /// <summary>
        /// Read file, currently just check if file does exist and given file path is valid.
        /// </summary>
        public void readFile()
        {
            Console.WriteLine("Please input file path:");
            filePath = Console.ReadLine();
            try
            {
                doc.Load(filePath);
            }
            catch{
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Extract data from file using filePath and create output string to be printed in new file
        /// </summary>
        /// <returns>returns output string that contains only elements that are searched</returns>
        public String extractOutput()
        {
            StringBuilder output = new StringBuilder(); //string that will be used to save values
            using (XmlTextReader reader = new XmlTextReader(filePath)) { 
            int i = 1;
            while (reader.Read())
            {
                //when we get to new row, add \n so we can separate individual rows
                if (i == 1 && reader.Name.Equals("row")) 
                        {
                            output.Append("\n");
                            i = 0;
                        }
                else if(reader.Name.Equals("row")) i++; 

                //reading from element that has attributes
                if (reader.AttributeCount > 0)
                        {
                            //taking atribute name and continue working if it is not null
                            String atName = reader.GetAttribute("name").ToString(); 
                            if (atName != null) { 
                            //checking if attribute is required for the file and imputing it once for the header.
                                if (searchedAttributes.Contains(atName) && !header.Contains(atName))  
                                {
                                  header = header + (reader.GetAttribute("name") + ','); //Not using string builder because i need contains function
                                
                                }

                            //reading attribute and "cleaning it" for output string.
                             if(searchedAttributes.Contains(atName))
                             {
                                String forOutput = cleanString(reader.ReadElementContentAsString()); //cleaning string so it would fit csv format
                                output.Append(forOutput + ","); 
                             }

                            }
                        }
            }
            }
            return (header + output);
            
        }

        /// <summary>
        /// Create file from previouse file destination with csv extension
        /// </summary>
        public void fileCreate()
        {
            String newFileDestination = filePath.Substring(0,filePath.LastIndexOf('.')) + ".csv";

            using (StreamWriter file = new StreamWriter(newFileDestination))
            {
                file.WriteLine(extractOutput());
            }
        }
       
        /// <summary>
        /// Cleaning string so it would fit format of CSV file
        /// </summary>
        /// <param name="str">str is string that we want to clean</param>
        /// <returns>return is that same string but cleaned</returns>
        public String cleanString(String str)
        {
            str = str.Replace(',','\0');
            str = str.Replace('"', '\0');
            str = str.Replace('\n', '\0');
            str = str.Replace('\\', '\0');
            return str;
        }
    }
}