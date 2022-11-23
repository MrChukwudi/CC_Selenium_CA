using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    class Ping_Auto : Constants
    {
        private static readonly string NewPingPath = "/ping/newping";
        private static readonly string ViewPingPath = "/ping/view";

        public static void Create()
        {
            SetUpPing();

            ClickElement(By.Id("btn_submit"));
            //WaitForElement(By.ClassName("sa-button-container"), true);

            LogAlert("Ping - create");
        }

        public static void Acknowledge()
        {
            bool success = false;
            string testName = "Ping - acknowledgement";

            // go to the ping page
            CheckUrl(Constants.BaseUrl + ViewPingPath);
            WaitForElement(By.Id("tblPing"), true);

            // open the message panel
            ClickElement(By.Id("chat-menu-toggle"));
            WaitForElement(By.Id("pingBdg"), true);

            // select ping tab
            if (ClickElement(By.Id("pingacktab")))
            {
                ShortWait();
                IWebElement pingFound = GetMatchingPing();

                // if a matching pin is found, acknowledge it.
                if (pingFound != null)
                {
                    try
                    {
                        pingFound.Click();
                    }
                    catch (Exception)
                    {
                        // log error
                        Log.Entry(Log.Fail, testName, "Ping Not Found");
                    }
                    // if an acknowledge button show, click it.
                    if (WaitForElement(By.XPath("//button[contains(@class, 'btn btn-block mt-2 btn-primary rspndBtn display-none')]"), false))
                    {
                        ClickElement(By.XPath("//button[contains(@class, 'btn btn-block mt-2 btn-primary rspndBtn display-none')]"));
                        success = true;
                    }
                    else if (WaitForElement(By.XPath("//button[contains(@class,'btn btn-block btn-primary rspndBtn display-none')]"), false))
                    {
                        ClickElement(By.XPath("//button[contains(@class,'btn btn-block btn-primary rspndBtn display-none')]"));
                        success = true;
                    }
                    else
                    {
                        // this page is difficult to deal with, make sure you're still there.
                        ClickElement(By.Id("pingBdg"));
                        pingFound = GetMatchingPing();
                        // sometimes when getting here the ping has collapsed and the radio buttons are not visible.
                        // In that case, click on the ping to expand and show the radio buttons.
                        if (!WaitForElement(By.XPath("//button[contains(@class, 'btn btn-block mt-2 btn-primary rspndBtn display-none')]"), false))
                        {
                            pingFound.Click();
                        }
                        // if multiple options, select one.
                        if (WaitForElement(By.XPath("//button[contains(@class, 'btn btn-block mt-2 btn-primary rspndBtn display-none')]"), false))
                        {
                            ClickElement(By.XPath("//button[contains(@class, 'btn btn-block mt-2 btn-primary rspndBtn display-none')]"));
                            success = true;
                        }
                    }
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "Ping Not Found");
                }
            }

            // wait before continuing as it will otherwise cause a crash
            ShortWait();
            ClickElement(By.XPath("//span[contains(@class,'cursorp fa-2x float-right text-white')]"));
            // return the message corresponding to the outcome.
            if (success)
            {
                Log.Entry(Log.Pass, testName, "acknowledgement successful");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "acknowledgement failed");
            }
        }


        private static IWebElement GetMatchingPing()
        {
            // get all pings
            var pings = Constants.driver.FindElements(By.ClassName("user-name"));
            IWebElement pingFound = null;

            // look for the first matching ping
            foreach (IWebElement ping in pings)
            {
                if (ping.Text == Constants.PingMessage && pingFound == null)
                {
                    pingFound = ping;
                }
            }

            return pingFound;
        }

        public static void SetUpPing()
        {
            CheckUrl(Constants.BaseUrl + NewPingPath);

            if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
            {
                ClickElement(By.XPath("//input[contains(@placeholder, 'Select Location')]"));
                WriteInElement(By.XPath("//input[contains(@placeholder, 'Select Location')]"), Constants.PingLocation);
                MoveToElement(By.CssSelector("input.select2-search__field"));
                ClickElement(By.XPath("//span[contains(@class,'select2-selection select2-selection--multiple')]/ul"));
                WriteInElement(By.XPath("//span[contains(@class,'select2-selection select2-selection--multiple')]/ul/li/input"), Constants.PingLocation);
            }
            else
            {
                WriteInElement(By.CssSelector("input.select2-search__field"), Constants.PingLocation);
            }
            ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

            ClickElement(By.Id("Message"));
            WriteInElement(By.Id("Message"), Constants.PingMessage);

            if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
            {
                ClickElement(By.XPath("//input[contains(@placeholder, 'Users')]"));
            }
            WriteInElement(By.XPath("//input[contains(@placeholder, 'User(s)')]"), Constants.UserNumber1);
            ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

            ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
            if (Constants.driver.GetType().Name.ToLower().Contains("chrome") && IsElementDisplayed(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"), false))
            {
                // On chrome, the first click does not always work so click twice.
                ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
            }
            ClickElement(By.XPath("//li[contains(@title,'Phone')]/span[contains(@class,'select2-selection__choice__remove')]"));
        }
    }
}
