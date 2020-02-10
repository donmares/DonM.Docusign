using System;
using DonM.Docusign.Library.Interfaces;
using DonM.Docusign.Library.Services;

namespace DonM.Docusign.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length != 4)
                    throw new Exception("Error - 4 parameters expected <firstKeyword> <secondKeyword> <range> <filename>");

                int range = 0;
                if (!int.TryParse(args[2], out range))
                    throw new Exception("Error - third parameter for <range> is expected to be an integer");

                IFileReader fileReader = new FileReader();
                string fileContents = fileReader.ReadAll(args[3]);
                
                IProximitySearch proximitySearch = new ProximitySearch(args[0], args[1], range, fileContents);

                int matchCount = proximitySearch.ExecuteSearch();

                System.Console.WriteLine($"Success - returned {matchCount} matches");
            }
            catch(Exception ex)
            {
                System.Console.WriteLine(ex.Message);
            }
        }
    }
}
