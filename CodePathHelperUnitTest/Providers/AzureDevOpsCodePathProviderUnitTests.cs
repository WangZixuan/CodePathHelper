namespace ShareCodePathUnitTest.Providers
{
    using CodePathHelper.Providers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AzureDevOpsCodePathProviderUnitTests
    {
        [TestMethod]
        public void TestExtractInfoFromUrl()
        {
            bool isSuccess = AzureDevOpsCodePathProvider.ExtractInfoFromUrl(
                "https://Tenant.visualstudio.com/Organization/_git/Repository?path=/sources/dev/data/source.cs&version=GBusers/TestAzureDevOps&line=22&lineEnd=25&lineStartColumn=1&lineEndColumn=45&lineStyle=plain&_a=contents",
                out string repoUrl, out string filePath, out string branchName, out int lineNumber);

            Assert.IsTrue(isSuccess);
            Assert.IsTrue(repoUrl == "https://Tenant.visualstudio.com/Organization/_git/Repository");
            Assert.IsTrue(filePath == "/sources/dev/data/source.cs");
            Assert.IsTrue(branchName == "users/TestAzureDevOps");
            Assert.IsTrue(lineNumber == 22);
        }

        [TestMethod]
        public void TestFormatRepoUrl()
        {
            string newUrl = AzureDevOpsCodePathProvider.FormatRepoUrl("https://user@dev.azure.com/user/Repository/_git/Repository");
            Assert.IsTrue(newUrl == "https://user.visualstudio.com/_git/Repository");

            string newUrl2 = AzureDevOpsCodePathProvider.FormatRepoUrl("https://tenant.visualstudio.com/Organization/_git/Repository");
            Assert.IsTrue(newUrl2 == "https://tenant.visualstudio.com/Organization/_git/Repository");
        }
    }
}
