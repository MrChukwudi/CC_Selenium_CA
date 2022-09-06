using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace Selenium_CC_CA.Initialisers
{

    public class Group : Constants
    {
        private static readonly string PagePath = "/security";


        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Create Menu Access";
            CheckUrl(BaseUrl + PagePath);

            if (!TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewGroup, By.XPath("//*[@id='tblSecurity']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//a[contains(text(),'Add Menu Access')]"));
                WaitForElement(By.Id("GroupName"), false);

                ClickElement(By.Id("select2-UserRole-container"));
                WriteInElement(By.ClassName("select2-search__field"), "Admin");
                ClickElement(By.XPath("//ul[@id='select2-UserRole-results']/li")); //.XPath("//ul[@id='select2-UserRole-results']/li[Contains(@class, 'select2-results__option select2-results__option--highlighted')]")

                WriteInElement(By.Id("GroupName"), Constants.NewGroup);
                WriteInElement(By.Id("Description"), "Automated Test Description");

                ClickElement(By.Id("btn_submit"));

                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Menu Access Created");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Menu Access profile already exists.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Error creating menu access");
            }

            return Log.GetLog();
        }


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Edit Menu Access";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewGroup, By.XPath("//*[@id='tblSecurity']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//button[contains(@class, 'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));
                WaitForElement(By.Id("Description"), false);

                WriteInElement(By.Id("Description"), "New automated test description");

                ClickElement(By.XPath("//label[contains(text(),'Dashboard')]"));
                ClickElement(By.XPath("//label[contains(text(),'Ping')]"));
                ClickElement(By.XPath("//label[contains(text(),'Setup Locations')]"));

                // give time for the ticks to happen!
                ShortWait();

                ClickElement(By.Id("btn_submit"));

                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Menu Access Successfully Updated");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Menu Access Group does not exist.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Menu Access not Updated");
            }

            return Log.GetLog();
        }


        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testName = "Delete Menu Access";
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewGroup, By.XPath("//*[@id='tblSecurity']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//button[contains(@class, 'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");

                WaitForElement(By.Id("toast-container"), true);
                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Menu Access Deleted");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Menu Access Group does not exist.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Error deleting Menu Access");
            }


            return Log.GetLog();
        }
    }
}
