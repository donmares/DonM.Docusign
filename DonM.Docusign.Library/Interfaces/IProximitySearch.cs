using System.Collections.Generic;

namespace DonM.Docusign.Library.Interfaces
{
    public interface IProximitySearch
    {
        int ExecuteSearch(string firstKeyword, string secondKeyword, int range, string searchString);

        //this should not be in interface but used for testing purposes
        List<Dictionary<int, string>> GetRanges(int range, string searchString);
    }
}
