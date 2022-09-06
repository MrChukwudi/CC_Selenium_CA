using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using Selenium_CC_CA.Initialisers;
using System;

namespace Selenium_CC_CA.Initialisers
{
    class BrowserSelection
    {
        /// <summary>
        /// Select the browser to use during the test, create the driver and return it.
        /// </summary>
        /// <returns></returns>
        public static string[] SelectBrowser(out IWebDriver webDriver)
        {
            int choice;
            string error = string.Empty;

            string[] message = { "Test launched using ", "" };
            Console.WriteLine("CC - Selenium:");
            Console.Write("\n\t1 - Chrome\n\t2 - Edge\n\t3 - Firefox \n\t4 - Internet Explorer\n\nPlease select the browser you want to use: ");
            if (Constants.IsDebug == true)
            {
                ChromeOptions options = new ChromeOptions();
                options.AddExcludedArgument("excludeSwitches, ['enable-logging']"); // used to remove the blue
                options.AddArguments("--disable-notifications"); // disable notification popups
                ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                webDriver = new ChromeDriver(service, options);
            }
            else if (int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("\n");

                try
                {
                    switch (choice)
                    {
                        case 1:
                        default:
                            ChromeOptions options = new ChromeOptions();
                            options.AddExcludedArgument("excludeSwitches, ['enable-logging']"); // used to remove the blue
                            options.AddArguments("--disable-notifications"); // disable notification popups
                            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                            service.HideCommandPromptWindow = true;
                            webDriver = new ChromeDriver(service, options);
                            //message[0] += "Chrome ";
                            break;
                        case 2:
                            webDriver = new EdgeDriver();
                            //message[0] += "Edge ";
                            break;
                        case 3:
                            webDriver = new FirefoxDriver();
                            //message[0] += "Firefox ";
                            break;
                        case 4:
                            webDriver = new InternetExplorerDriver();
                            //message[0] += "Internet Explorer ";
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error launching the browser: {0}", e.Message);
                    webDriver = null;
                    error = e.Message;
                }
            }
            else
            {
                Console.WriteLine("\n");
                try
                {
                    ChromeOptions options = new ChromeOptions();
                    options.AddExcludedArgument("excludeSwitches, ['enable-logging']"); // used to remove the blue
                    options.AddArguments("--disable-notifications"); // disable notification popups
                    ChromeDriverService service = ChromeDriverService.CreateDefaultService();
                    service.HideCommandPromptWindow = true;
                    webDriver = new ChromeDriver(service, options);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error launching the browser: {0}", e.Message);
                    webDriver = null;
                    error = e.Message;
                }
            }


            if (webDriver != null)
            {
                // select the portal url to use as base URL
                Console.WriteLine("\n\n\t1 - Dev portal ({0})\n\t2 - PP portal ({1})\n\t3 - Live Portal ({2})\n\t4 - G-Cloud Portal ({3})\n\t5 - KSA portal ({4})\n\t6 - MEA Portal ({5})\n\t7 - UAE Portal ({6})",
                    Constants.DevUrl, Constants.PpUrl, Constants.LiveUrl, Constants.GUrl, Constants.KsaUrl, Constants.MeaUrl, Constants.UaeUrl);
                Console.Write("\nPlease select the portal you want to test: ");

                if (Constants.IsDebug == true)
                {
                    Console.WriteLine("6");
                    Constants.BaseUrl = Constants.MeaUrl;
                }
                else
                {
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        switch (choice)
                        {
                            case 1:
                            default:
                                Constants.BaseUrl = Constants.DevUrl;
                                break;
                            case 2:
                                Constants.BaseUrl = Constants.PpUrl;
                                break;
                            case 3:
                                Constants.BaseUrl = Constants.LiveUrl;
                                break;
                            case 4:
                                Constants.BaseUrl = Constants.GUrl; // need to remove G portal as it has been deleted 
                                break;
                            case 5:
                                Constants.BaseUrl = Constants.KsaUrl;
                                break;
                            case 6:
                                Constants.BaseUrl = Constants.MeaUrl;
                                break;
                            case 7:
                                Constants.BaseUrl = Constants.UaeUrl;
                                break;
                        }
                    }
                    else
                    {
                        Constants.BaseUrl = Constants.LiveUrl;
                    }
                    Console.WriteLine("\n");
                }

                ICapabilities capabilities = ((ChromeDriver)webDriver).Capabilities;
                //string strBrowserVersion = capabilities.Version.ToString();
                //if (strBrowserVersion == "")
                string strBrowserVersion = capabilities.GetCapability("browserVersion")?.ToString() ?? "";

                //string osVersion = System.Environment.OSVersion.VersionString;

                string subKey = @"SOFTWARE\Wow6432Node\Microsoft\Windows NT\CurrentVersion";
                //Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.LocalMachine;
                //Microsoft.Win32.RegistryKey skey = key.OpenSubKey(subKey);
                //string osName = skey.GetValue("ProductName").ToString();
                string osBuild = Environment.OSVersion.Version.Build.ToString();

                //return name != null ? name.ToString() : "Unknown";
                message[0] += webDriver.GetType().Name.ToString().Replace("Driver", string.Empty) + " " + strBrowserVersion /* + strBrowserVersion*/;
                //message[1] = "Running on " + osName + " (" + osVersion + ") Build " + osBuild;

                // set implicit wait to 10s.
                webDriver.Manage().Timeouts().ImplicitWait = Constants.ImplicitWait;
            }
            else
            {
                return new string[] { Constants.ErrorMsg[0], error };
            }

            logHeader();

            return message;
        }

        /// <summary>
        /// Add header to the log file
        /// </summary>
        private static void logHeader()
        {
            Functions.LogFile.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx" ); // + "\r\n"
            Functions.LogFile.Add("Automated Test Started On " + DateTime.Now.ToLocalTime().ToString() );
            Functions.LogFile.Add("URL : " + Constants.BaseUrl);
            //Functions.LogFile.Add("Computer : " + Environment.MachineName);
            Functions.LogFile.Add("xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx \r\n");
        }
    }
}
