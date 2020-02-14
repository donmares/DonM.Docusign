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
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 6, "the man the plan the canal panama");
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges();

            Assert.True(rangeList.Count == 2);
        }

        [Fact]
        public void GetRangeSingleTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 2, "the man ");
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges();

            Assert.True(rangeList.Count == 1);
        }

        [Fact]
        public void GetRangeLengthTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 6, "the man in the canal");
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges();

            Assert.True(rangeList.Count == 1);
        }

        [Fact]
        public void GetRangeExtraSpaceTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 2, "   the    \r\n     man      ");
            List<Dictionary<int, string>> rangeList = proximitySearch.GetRanges();

            Assert.True(rangeList.Count == 1);
        }

        [Fact]
        public void GetRangeMinimumLengthTest()
        {
            bool isFailure = false;
            try
            {
                IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 1, " canal ");
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
                IProximitySearch proximitySearch = new ProximitySearch("the new", "canal", 3, "the new man canal");
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
                IProximitySearch proximitySearch = new ProximitySearch("the", "The", 3, "the man ");
            }
            catch (Exception ex)
            {
                isFailure = true;
            }
            Assert.True(isFailure);
        }

        [Fact]
        public void ExecuteNotFoundTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 4, "inside a bluff");
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 0);
        }

        [Fact]
        public void ExecuteCaseIncensitiveTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 4, "The canal is beyond the bluff");
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 2);
        }

        [Fact]
        public void ExecuteFirstTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 6, "the man the plan the canal panama");
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 3);
        }

        [Fact]
        public void ExecuteSecondTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 3, "the man the plan the canal panama");
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 1);
        }

        [Fact]
        public void ExecuteThirdTest()
        {
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 6, "the man the plan the canal panama panama canal the plan the man the the man the plan the canal panama");
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 11);
        }

        [Fact]
        public void ExecuteFileTest()
        {
            IFileReader fileReader = new FileReader();
            IProximitySearch proximitySearch = new ProximitySearch("the", "canal", 6, fileReader.ReadAll("test3.txt"));
            int matchCount = proximitySearch.ExecuteSearch();

            Assert.True(matchCount == 15);
        }
    }

}
