﻿using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Cw2
{
    class Program

    {
        public static void ErrorLogging(Exception ex)
        {
            string logpath = @"C:\Users\s17064\Desktop\log.txt";
            if (!File.Exists(logpath))
            {
                File.Create(logpath).Dispose();
            }
            StreamWriter sw = File.AppendText(logpath);
            sw.WriteLine("logging errors");
            sw.WriteLine("Start " + DateTime.Now);
            sw.WriteLine("Error: " + ex.Message);
            sw.WriteLine("End " + DateTime.Now);
            sw.Close();
        }
        static void Main(string[] args)
        {
            try
            {
                string csvpath = Console.ReadLine(); // P:\FTP(Public)\maksous\APBD\2_csvtoxml\dane.csv
                string xmlpath = Console.ReadLine(); // C:\Users\s17064\Desktop\
                string format = Console.ReadLine(); // xml
                if (File.Exists(csvpath) && Directory.Exists(xmlpath)) {
                    string[] source = File.ReadAllLines(csvpath);
                    XElement xml = new XElement("uczelnia",
                        new XElement("studenci",
                        from str in source
                        let fields = str.Split(',')
                        select new XElement("student",

                                new XAttribute("indexNumber", "s" + fields[4]),
                                new XElement("fname", fields[0]),
                                new XElement("lName", fields[1]),
                                new XElement("birthday", fields[5]),
                                new XElement("email", fields[6]),
                                new XElement("mothersname", fields[7]),
                                new XElement("fathersname", fields[8]),
                                new XElement("studies",
                                    new XElement("name", fields[2]),
                                    new XElement("mode", fields[3])

                            )
                        )
                    )
                        );

                    xml.Save(String.Concat(xmlpath + "result.xml"));
                }
                else{
                    if (!File.Exists(csvpath))
                    {
                        throw new Exception("File does not exist");
                    } if (!Directory.Exists(xmlpath))
                    {
                        throw new Exception("Directory does not exist");
                    }
                }
                }  
                catch (Exception ex)
            {
                ErrorLogging(ex);
            }

        }
    }
}
