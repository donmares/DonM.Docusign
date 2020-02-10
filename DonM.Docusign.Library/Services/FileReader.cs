using System;
using System.IO;
using System.Linq;
using System.Text;
using DonM.Docusign.Library.Interfaces;

namespace DonM.Docusign.Library.Services
{
    public class FileReader : IFileReader
    {
        /// <summary>
        /// simple method to read all lines from file into a string. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ReadAll(string fileName)
        {
            string allLines = "";

            if (!File.Exists(fileName))
                throw new Exception("Error - file name not found");

            using (StreamReader sr = new StreamReader(fileName))
            {
                allLines = sr.ReadToEnd();
            }

            return allLines;
        }
    }
}
