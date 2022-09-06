using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    class Global_Config : Constants
    {
        private static string PagePath = "/company/configuration";

        /// <summary>
        /// This function sets up the global settings to allow for the test to run smoothly
        /// </summary>
        public static List<LogItem> Setup()
        {
            Log.Clear();
            string testName = "Setup test";
            Console.WriteLine(testName);
            CheckUrl(Constants.BaseUrl + PagePath);   
            //Log.Entry(testName);
            WaitForElement(By.Id("settings_tabs"), true);

            // disable pings on incident closure
            testName = ("Disable ping on incident closure");
            ClickElement(By.LinkText("Incident"));
            WaitForElement(By.Id("incident_params"), true);
            ClickElement(By.XPath("//label[contains(.,'No')]"));
            ClickElement(By.Id("btn_inc_submit"));
            LogAlert(testName);
            ShortWait();

            // add password when performing task action
            testName= "Ask for Password when performing a Task";
            ScrollToTop();
            ClickElement(By.LinkText("System"));
            WaitForElement(By.Id("tab_system"), true);
            ClickElement(By.XPath("//*[@id='883b02f9ab88987aec19dfb9148d4abb']/label[1]"));
            MoveToElement(By.XPath("//*[@id='system_params']/div[19]/div"));
            ClickElement(By.XPath("(//button[@id='btn_system_submit'])")); //*[@id="a62a4af2f7d72cabdb622d7df17f4d54"]
            LogAlert(testName);
            ShortWait();

            // disable channels by priority
            ScrollToTop();
            testName = "Disable priority settings";
            ClickElement(By.LinkText("Channel by Priority"));
            WaitForElement(By.Id("frmcommspriority"), true);
            //Constants.driver.FindElement(By.XPath("//label[contains(text(),'No')]")).Submit(); //Apparently No is not displayed on the page... so using this rather than the usual function.
            ClickElement(By.XPath("//form[@id='frmcommspriority']/div/div[2]/div/label[contains(text(),'No')]"));
            ClickElement(By.Id("btn_priority_submit"));
            LogAlert(testName);
            ShortWait();

            // disable channels by severity
            ScrollToTop();
            testName = "Disable security settings";
            ClickElement(By.LinkText("Channel by Severity"));
            WaitForElement(By.Id("frmcommsseverity"), true);
            ClickElement(By.XPath("//form[@id='frmcommsseverity']/div/div[2]/div/label[contains(text(),'No')]"));
            ClickElement(By.Id("btn_severity_submit"));
            LogAlert(testName);
            ShortWait();

            return Log.GetLog();

        }
    }
}
