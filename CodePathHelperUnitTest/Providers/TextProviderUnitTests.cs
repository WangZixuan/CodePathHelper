
namespace ShareCodePathUnitTest.Providers
{
    using CodePathHelper.Providers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TextProviderUnitTests
    {
        [TestMethod]
        public void TestExtractUrl()
        {
            bool isSuccess = TextProvider.ExtractUrl("today let's watch https://www.cctv.com/all?channel=cctv5&param=1080p and http://www.youtube.com together", out string url);

            Assert.IsTrue(isSuccess);
            Assert.IsTrue(url == "https://www.cctv.com/all?channel=cctv5&param=1080p");
        }
    }
}
