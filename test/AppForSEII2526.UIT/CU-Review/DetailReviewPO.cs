using OpenQA.Selenium;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.CU_Review
{
    public class DetailReviewPO : PageObject
    {
        private By _reviewTitleBy = By.Id("ReviewTitle");
        private By _countryBy = By.Id("CustomerCountry");

        public DetailReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }

        public bool CheckReviewDetail(string expectedCountry, string expectedTitle)
        {
            try
            {
                WaitForBeingVisible(_reviewTitleBy);

                var titleText = _driver.FindElement(_reviewTitleBy).Text;
                var countryText = _driver.FindElement(_countryBy).Text;

                return
                    titleText.Contains(expectedTitle) &&
                    countryText.Contains(expectedCountry);
            }
            catch
            {
                return false;
            }
        }

        public bool IsDeviceShownInDetail(int deviceId)
        {
            try
            {
                return _driver.FindElement(By.Id($"DeviceData_{deviceId}")).Displayed;
            }
            catch
            {
                return false;
            }
        }

    }
}
