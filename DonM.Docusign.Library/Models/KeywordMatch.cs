using System;

namespace DonM.Docusign.Library.Models
{
    /// <summary>
    /// this class stores the positions of matches
    /// </summary>
    public class KeywordMatch : IEquatable<KeywordMatch>
    {
        public int FirstPosition { get; set; }
        public int SecondPosition { get; set; }

        public bool Equals(KeywordMatch keywordMatch)
        {
            return FirstPosition == keywordMatch.FirstPosition && SecondPosition == keywordMatch.SecondPosition;
        }
    }
}
