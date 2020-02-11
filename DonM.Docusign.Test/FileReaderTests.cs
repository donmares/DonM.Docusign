using System;
using System.Collections.Generic;
using Xunit;
using DonM.Docusign.Library.Services;
using DonM.Docusign.Library.Interfaces;

namespace DonM.Docusign.Test
{
    public class FileReaderTests
    {
        [Fact]
        public void InvalidFileTest()
        {
            bool isFailure = false;
            try
            {
                IFileReader fileReader = new FileReader();
                string fileContents = fileReader.ReadAll("DoesNotExist.txt");
            }
            catch (Exception ex)
            {
                isFailure = true;
            }
            Assert.True(isFailure);
        }
    }
}
