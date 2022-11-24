using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;


namespace Selenium_CC_CA.Initialisers
{
    class DriverOptions
    {
        /// Browser Settings 

        internal static ChromeDriver GetChromeDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddExcludedArgument("excludeSwitches, ['enable-logging']");
            options.AddArguments("--disable-notifications"); // disable notification popups
            options.AddArguments("--headless", "--window - size = 1920, 1200"); // tries to launch chrome in headless mode // prev before -- 
            options.AddArguments("disable - gpu");
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            return new ChromeDriver(service, options);
        }

    }
}

