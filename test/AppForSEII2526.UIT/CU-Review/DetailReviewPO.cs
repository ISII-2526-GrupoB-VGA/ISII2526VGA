using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.CU_Review
{
    public class DetailReviewPO : PageObject
    {
        public DetailReviewPO(IWebDriver driver, ITestOutputHelper output)
            : base(driver, output)
        {
        }
        public bool CheckReviewDetail(string country, string reviewTitle)
        {
            WaitForBeingVisible(By.Id("ReviewTitle"));

            bool result = true;

            result &= _driver.FindElement(By.Id("CustomerCountry"))
                             .Text.Contains(country);

            result &= _driver.FindElement(By.Id("ReviewTitle"))
                             .Text.Contains(reviewTitle);

            return result;
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