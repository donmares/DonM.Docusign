using System;
using System.Collections.Generic;
using Xunit;
using DonM.Docusign.Library.Services;
using DonM.Docusign.Library.Interfaces;

namespace DonM.Docusign.Test
{
    public class ProximityTests
    {
        [Fact]
        public void GetRangeTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges(6, "the man the plan the canal panama");

            Assert.True(rangeList.Count == 2);
        }


        [Fact]
        public void GetRangeLengthTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges(6, "the man in the canal");

            Assert.True(rangeList.Count == 1);
        }

        [Fact]
        public void GetRangeMinimumLengthTest()
        {
            bool isFailure = false;
            try
            {
                IProximitySearch proximitySearch = new ProximitySearch();
                int matchCount = proximitySearch.ExecuteSearch("the", "canal", 1, " canal ");
            }
            catch (Exception ex)
            {
                isFailure = true;
            }
            Assert.True(isFailure);
        }

        [Fact]
        public void InvalidKeywordFailureTest()
        {
            bool isFailure = false;
            try
            {
                IProximitySearch proximitySearch = new ProximitySearch();
                int matchCount = proximitySearch.ExecuteSearch("the new", "canal", 3, "the new man canal");
            }
            catch (Exception ex)
            {
                isFailure = true;
            }
            Assert.True(isFailure);
        }

        [Fact]
        public void SameKeywordFailureTest()
        {
            bool isFailure = false;
            try
            {
                IProximitySearch proximitySearch = new ProximitySearch();
                int matchCount = proximitySearch.ExecuteSearch("the", "The", 3, "the man ");
            }
            catch (Exception ex)
            {
                isFailure = true;
            }
            Assert.True(isFailure);
        }

        [Fact]
        public void SearchExtraSpaceCapitalizationTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("The", "mAn", 2, "  \n   the      \r\n        Man    \r  ");

            Assert.True(matchCount == 1);
        }

        [Fact]
        public void SearchNotFoundTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 4, "inside a bluff");

            Assert.True(matchCount == 0);
        }

        [Fact]
        public void SearchCaseIncensitiveTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 4, "The canal is beyond the bluff");

            Assert.True(matchCount == 2);
        }

        [Fact]
        public void SearchFirstTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 6, "the man the plan the canal panama");

            Assert.True(matchCount == 3);
        }

        [Fact]
        public void SearchSecondTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 3, "the man the plan the canal panama");

            Assert.True(matchCount == 1);
        }

        [Fact]
        public void SearchThirdTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 6, "the man the plan the canal panama panama canal the plan the man the the man the plan the canal panama");

            Assert.True(matchCount == 11);
        }

        [Fact]
        public void SearchFileTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch();
            int matchCount = proximitySearch.ExecuteSearch("the", "canal", 6, fileReader.ReadAll("test3.txt"));

            Assert.True(matchCount == 15);
        }
    }

}
