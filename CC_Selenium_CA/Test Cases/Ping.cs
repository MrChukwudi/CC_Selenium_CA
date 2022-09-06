using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    public class Ping : Constants
    {
        private static readonly string NewPingPath = "/ping/newping";
        private static readonly string ViewPingPath = "/ping/view";

        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Ping - create";
            SetUpPing();

            ClickElement(By.Id("btn_submit"));

            LogAlert(testName);
            return Log.GetLog();

        }

        public static List<LogItem> Acknowledge()
        {
            Log.Clear();
            bool success = false;
            string testName = "Ping - acknowledgement";
            Console.WriteLine(testName);

            // go to the ping page
            CheckUrl(BaseUrl + ViewPingPath);
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
                        Log.Entry(Log.Fail, testName, "ping not found");
                    }
                    // if an acknowledge button show, click it.
                    if (WaitForElement(By.XPath("//div[contains(@class, 'radio radio-primary')]"), false))
                    {
                        success = AcknowledgeOptions(success);
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
                        if (!WaitForElement(By.XPath("//div[contains(@class, 'radio radio-primary')]"), false))
                        {
                            pingFound.Click();
                        }
                        // if multiple options, select one.
                        if (WaitForElement(By.XPath("//div[contains(@class, 'radio radio-primary')]"), false))
                        {
                            success = AcknowledgeOptions(success);
                        }
                    }
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "ping not found");
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
            return Log.GetLog();
        }

        private static bool AcknowledgeOptions(bool success)
        {
            // find all the options
            IWebElement radioButtonDiv = Constants.driver.FindElement(By.XPath("//div[contains(@class, 'radio radio-primary')]"));
            var radioButtons = radioButtonDiv.FindElements(By.TagName("label"));

            int errorCount = 0;
            // pick the first one that matches and click.
            foreach (IWebElement radioButton in radioButtons)
            {
                if (radioButton.Text.ToLower().Contains("agree")
                    || radioButton.Text.ToLower().Contains("approve")
                    || radioButton.Text.ToLower().Contains("no"))
                {
                    try
                    {
                        radioButton.Click();
                        break;
                    }
                    catch (Exception)
                    {
                        errorCount++;
                        // TODO log error
                    }
                }
            }
            // check if the acknowledgement was successful
            if (errorCount < radioButtons.Count)
            {
                success = true;
            }

            return success;
        }

        private static IWebElement GetMatchingPing()
        {
            // get all pings
            var pings = driver.FindElements(By.ClassName("user-name"));
            IWebElement pingFound = null;

            // look for the first matching ping
            foreach (IWebElement ping in pings)
            {
                if (ping.Text == NewPing && pingFound == null)
                {
                    pingFound = ping;
                }
            }

            return pingFound;
        }

        public static void SetUpPing()
        {
            CheckUrl(BaseUrl + NewPingPath);

            if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
            {
                ClickElement(By.XPath("//input[contains(@placeholder, 'Select Location')]"));
                WriteInElement(By.XPath("//input[contains(@placeholder, 'Select Location')]"), Constants.NewLocation);
                MoveToElement(By.CssSelector("input.select2-search__field"));
                ClickElement(By.XPath("//span[contains(@class,'select2-selection select2-selection--multiple')]/ul"));
                WriteInElement(By.XPath("//span[contains(@class,'select2-selection select2-selection--multiple')]/ul/li/input"), Constants.NewLocation);
            }
            else
            {
                //WriteInElement(By.XPath("//input[contains(@placeholder, 'Location')]"), NewLocation);
                WriteInElement(By.CssSelector("input.select2-search__field"), Constants.NewLocation);
            }
            ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

            ClickElement(By.Id("Message"));
            WriteInElement(By.Id("Message"), Constants.NewPing);

            // change the priority so hopefully all the comms methods get selected by defaults
            // this avoid waisting time when deselecting the comms methods further down.
            //ClickElement(By.Id("select2-Priority-container"));
            //ClickElement(By.XPath("//li[contains(text(), 'High')]"));
            ClickElement(By.CssSelector(".slider-tick:nth-child(3)"));


            if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
            {
                ClickElement(By.XPath("//input[contains(@placeholder, 'Users')]"));
            }
            //WriteInElement(By.CssSelector(".row:nth-child(3) .select2-search__field"), Constants.UserNumber1);
            WriteInElement(By.XPath("//input[contains(@placeholder, 'User(s)')]"), Constants.UserNumber1);
            ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

            int x = Constants.Loop;
            ClickElement(By.Id("selectSingleAsset"));
            do
            {
                //ShortWait();
                WaitForElement(By.Id("tblAssetSingle_wrapper"), true);
                ClickElement(By.XPath("//a[contains(text(),'Select')]"));
                x--;
            }
            while (!IsElementDisplayed(By.Id("playAudioMessage"), false) && x > 0);

            if (x == 0 && !IsElementDisplayed(By.Id("playAudioMessage"), false))
            {
                ClickElement(By.Id("btn_close"));
            }


            ClickElement(By.Id("btnAckOptions"));

            ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[1]/td[1]/div/label"));
            ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[2]/td[1]/div/label"));
            ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[3]/td[1]/div/label"));
            //ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[4]/td[1]/div/label"));

            ClickElement(By.Id("btn_select"));

            ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
            if (Constants.driver.GetType().Name.ToLower().Contains("chrome") && IsElementDisplayed(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"), false))
            {
                // On chrome, the first click does not always work so click twice.
                ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
            }
            ClickElement(By.XPath("//li[contains(@title,'Phone')]/span[contains(@class,'select2-selection__choice__remove')]"));
            //ClickElement(By.XPath("//li[contains(@title,'Email')]/span[contains(@class,'select2-selection__choice__remove')]")); //Email can no longer be removed
            //ClickElement(By.Id("Message"));
        }
    }
}
