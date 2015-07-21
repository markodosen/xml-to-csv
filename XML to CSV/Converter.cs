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
        public String filePath { get; set; }

        //file that will be loaded if path is correct
        private XmlDocument doc = new XmlDocument(); 

        //header of new csv file
        private String header="";

        //list of all attributes that user is searching for
        private List<String> searchedAttributes;

        //dictionary that will contain required attributes and value of elements
        Dictionary<String, String> values = new Dictionary<string, string>();

        //string that will be used to save values
        StringBuilder output = new StringBuilder(); 
        

        /// <summary>
        /// Constructor for converter that require list of attributes that you want to search for
        /// </summary>
        /// <param name="searchedAttributes">contains list of attributes that you want to search for from xml file</param>
        public Converter(List<String> searchedAttributes)
        {
            this.searchedAttributes = searchedAttributes;
        }

        /// <summary>
        ///Check if file from filePath exists
        /// </summary>
        public void checkFile()
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("XML File not found, press any button to exit!");
                Console.ReadKey();
                Environment.Exit(0);
            }          
        }

        /// <summary>
        /// Extract data from xml file and save them in dictionary. Save only data that is passed by list of searched attributes 
        /// and displays them in same order that they were sent.
        /// </summary>
        /// <returns>returns string that consists of header(attirbutes) and all data</returns>
        public String newOutput()
        {
            //set keys to match attributes that will be used for header
            foreach (String s in searchedAttributes)
            {
                values.Add(s, "");
            }

            //set header for document
            setHeader();

            //read xml file
            using (XmlTextReader reader = new XmlTextReader(filePath))
            {
                while (reader.Read())
                {
                    //when end of row is reached fill output stream with values and reset values for next row.
                    if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("row"))
                    {
                        getAllValues();
                        output.Append("\n");
                        cleanValues(values);
                    }
                    {
                        if (reader.AttributeCount > 0)
                        {
                            //read attribute name and if that attribute is searched for csv file, add it to dictionary
                            String atName = reader.GetAttribute("name").ToString();
                            if (atName != null)
                            {
                                if (values.ContainsKey(atName))
                                {
                                    values[atName] = cleanString(reader.ReadElementContentAsString());
                                }
                            }
                        }
                    }
                }
            }
            //return complete string that contains all data ready to be put in csv
            return (header + "\n" + output);
        }

        /// <summary>
        /// Create file from previouse file destination with csv extension
        /// </summary>
        public void fileCreate()
        {
            String newFileDestination = filePath.Replace(Path.GetFileNameWithoutExtension(filePath), "[" +DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + "]" + Path.GetFileNameWithoutExtension(filePath));
            newFileDestination = Path.ChangeExtension(newFileDestination, ".csv");
            using (StreamWriter file = new StreamWriter(newFileDestination))
            {
                file.WriteLine(newOutput());
            }
        }
       
        /// <summary>
        /// Cleaning string so it would fit format of CSV file
        /// </summary>
        /// <param name="str">str is string that we want to clean</param>
        /// <returns>return is that same string but cleaned</returns>
        public String cleanString(String str)
        {
            if (str.Contains(",") || str.Contains("\""))
            {
                str = "\"" + str + "\"";                    
            }
            str = str.Replace('"', '\0');
            str = str.Replace("\n", "\\n");
            return str;
        }

        /// <summary>
        /// reset all values of dictionary, but keep keys
        /// </summary>
        /// <param name="d"> d is dictionary that we want to clean values</param>
        public void cleanValues(Dictionary<String,String> d){
            List<String> keys = new List<string>(d.Keys);
            foreach (var key in keys)
            {
                d[key] = "";
            }
        }

        /// <summary>
        /// create header of csv file from dictionary keys
        /// </summary>
        public void setHeader()
        {
            foreach (var s in values.Keys)
            {
                header = header + s + ",";
            }
        }

        /// <summary>
        /// takes all values from dictionary and add them to output string.
        /// </summary>
        public void getAllValues()
        {
            foreach (String s in values.Values)
            {
                output.Append(s + ",");
            }
        }
    }

    
}