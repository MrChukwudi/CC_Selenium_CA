using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{

    public class Scheduler : Constants
    {
        public static readonly string PagePath = "/scheduler";


        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Create scheduled ping";
            Console.WriteLine(testName);

            Ping.SetUpPing();

            ClickElement(By.Id("btn_schedule"));
            WaitForElement(By.Id("schduler_setting"), true);
            WriteInElement(By.Id("jobName"), Constants.ScheduledPingJobName);
            ClickElement(By.Id("btn_submit"));

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Edit Scheduled item";
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.ScheduledPingJobName, By.XPath("//*[@id='tblSchedule']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));
                WaitForElement(By.Id("schduler_setting"), true);
                //ClickElement(By.XPath("//a[contains(@class, 'btn btn-sm btn-small btn-warning')]"));

                WriteInElement(By.Id("jobName"), Constants.NewScheduledPingAfterEdit);

                ClickElement(By.Id("btn_submit"));

            }
            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testName = "Delete scheduled item";
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewScheduledPingAfterEdit, By.XPath("//*[@id='tblSchedule']/tbody/tr[1]/td[1]")))
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
