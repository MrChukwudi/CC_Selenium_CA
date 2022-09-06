using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{

    public class Trigger : Constants
    {
        private static readonly string PagePath = "/trigger";

        public static List<LogItem> Create() 
        {
            Log.Clear();
            string testName = "Trigger - Create triggered ping";
            Console.WriteLine(testName);
            Ping.SetUpPing();

            ClickElement(By.Id("btn_trigger"));

            WaitForElement(By.Id("SourceEmail"), true);
            WriteInElement(By.Id("SourceEmail"), "test@test.com");
            WriteInElement(By.Id("FailureEmail"), "test@test.com");
            WriteInElement(By.Id("jobName"), Constants.TriggeredPingJobName);

            if (!ClickElement(By.Id("btn_submit")))
            {
                ClickElement(By.Id("schduler_new"));
                ClickElement(By.Id("btn_submit"));
            }

            LogAlert(testName);

            return Log.GetLog();
        }


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Trigger - Edit trigger item";

            CheckUrl(Constants.BaseUrl + PagePath);

            WaitForElement(By.Id("tblTrigger"), true);
            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.TriggeredPingJobName, By.XPath("//*[@id='tblTrigger']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));
                //ClickElement(By.XPath("//a[contains(@class, 'btn btn-sm btn-small btn-warning')]"));

                WaitForElement(By.Id("jobName"), true);
                WriteInElement(By.Id("jobName"), Constants.NewTriggeredPingAfterEdit);

                ClickElement(By.Id("btn_submit"));

            }

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testName = "Trigger - Delete trigger item";
            CheckUrl(BaseUrl + PagePath);

            WaitForElement(By.Id("tblTrigger"), true);
            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewTriggeredPingAfterEdit, By.XPath("//*[@id='tblTrigger']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                //ClickElement(By.XPath("//a[contains(@class,'btn btn-sm btn-small btn-danger')]"));
                ShortWait();
                ClosePopUp("Delete Confirmation");
                //ShortWait();

            }
            LogAlert(testName);

            return Log.GetLog();
        }
    }
}
