using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    class Company_Setup : Constants
    {
        private static readonly string PagePath = "/company";
        private static string CompanyName = "";
        private static string Address = "";
        private static string City = "";

        public static List<LogItem> Modifiy()
        {

            Log.Clear();
            string testName = "Company setup - edit";
            Console.WriteLine(testName);

            CheckUrl(BaseUrl + PagePath);
            WaitForElement(By.Id("CompanyName"), true);

            CompanyName = Constants.driver.FindElement(By.Id("CompanyName")).GetAttribute("value");
            Address = Constants.driver.FindElement(By.Id("Address1")).GetAttribute("value");
            City = Constants.driver.FindElement(By.Id("CompanyCity")).GetAttribute("value");

            WriteInElement(By.Id("CompanyName"), "Transputec London Bridge");
            WriteInElement(By.Id("Address1"), "Transputec London Bridge");
            WriteInElement(By.Id("CompanyCity"), "London Bridge");

            ClickElement(By.Id("btn_company_submit"));

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Modifiy2()
        {
            Log.Clear();
            string testName = "Company setup - reverse edit";
            Console.WriteLine(testName);

            CheckUrl(BaseUrl + PagePath);
            WaitForElement(By.Id("CompanyName"),true);

            WriteInElement(By.Id("CompanyName"), CompanyName);
            WriteInElement(By.Id("Address1"), Address);
            WriteInElement(By.Id("CompanyCity"), City);

            ClickElement(By.Id("btn_company_submit"));

            LogAlert(testName);

            return Log.GetLog();
        }
    }
}
