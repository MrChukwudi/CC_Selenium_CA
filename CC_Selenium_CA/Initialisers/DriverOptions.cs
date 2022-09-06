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
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            return new ChromeDriver(service, options);
        }

        internal static InternetExplorerDriver GetIEDriver()
        {
            InternetExplorerDriverService service = InternetExplorerDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            return new InternetExplorerDriver(service);
        }

        internal static FirefoxDriver GetFireFoxDriver()
        {
            FirefoxOptions options = new FirefoxOptions();
            options.SetPreference("geo.enabled", false);
            options.SetPreference("geo.provider.use_corelocation", false);
            options.SetPreference("geo.prompt.testing", false);
            options.SetPreference("geo.prompt.testing.allow", false);
            options.SetPreference("dom.webnotifications.enabled", false); // TODO Test this.
            //options.AddArgument("-headless");
            FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            return new FirefoxDriver(service, options);
        }
    }
}

