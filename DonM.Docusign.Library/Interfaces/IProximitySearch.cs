using System.Collections.Generic;

namespace DonM.Docusign.Library.Interfaces
{
    public interface IProximitySearch
    {
        int ExecuteSearch();

        //this should not be in interface but used for testing purposes
        List<Dictionary<int, string>> GetRanges();
    }
}
