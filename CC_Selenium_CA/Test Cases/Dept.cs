﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;

namespace CC_Selenium_CA.Test_Cases
{
    public class Dept : Constants
    {
        private const string PagePath = "/department";


        public static List<LogItem> Create()
        {
            string testName = "Create Department";
            Log.Clear();

            CheckUrl(Constants.BaseUrl + PagePath);

            if (!TableSearch(By.XPath(".//*[@id='tblDepartment_filter']/label/input"), Constants.NewDept, By.XPath("//*[@id='tblDepartment']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//a[contains(@class,'btn btn-primary dropdown-toggle')]"));
                ClickElement(By.XPath("//a[contains(@href,'" + Constants.BaseUrl + "/department/create')]"));

                WaitForElement(By.Id("DepartmentName"), false);
                WriteInElement(By.Id("DepartmentName"), Constants.NewDept);

                ClickElement(By.Id("btn_submit"));

                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Department Created");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Department already exists.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Department Already Exists.");
            }
            return Log.GetLog();

        }


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string subTest = "Edit Department";
            string detailsTab = "Details";

            CheckUrl(Constants.BaseUrl + PagePath);

            if (TableSearch(By.XPath(".//*[@id='tblDepartment_filter']/label/input"), Constants.NewDept, By.XPath("//*[@id='tblDepartment']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));

                subTest = "Edit Department Name";

                MoveToElement(By.LinkText(detailsTab));
                ScrollToTop();
                ClickElement(By.LinkText(detailsTab));
                WaitForElement(By.Id("DepartmentName"), false);
                WriteInElement(By.Id("DepartmentName"), Constants.NewDeptAfterEdit);
                ClickElement(By.Id("btn_submit"));

                LogAlert(subTest);
                Console.WriteLine("Test: " + subTest + " | Result: PASS " + " | Outcome: Department Updated");
            }
            else
            {
                Log.Entry(Log.Fail, subTest, "Department Name Already Updated.");
                Console.WriteLine("Test: " + subTest + " | Result: FAIL " + " | Outcome: Department Name Already Updated.");
            }
            return Log.GetLog();

        }

        /// <summary>
        /// This will delete the test department automatically generated by Dept.Create().
        /// For this to succeed, it needs to not be associated with any tasks, so make sure to delete any test incident first.
        /// The incident tasks may have to be deleted before the incident for this to work.
        /// </summary>
        /// <returns></returns>
        public static List<LogItem> Delete()
        {

            Log.Clear();
            string testName = "Delete Department";
            CheckUrl(Constants.BaseUrl + PagePath);

            if (TableSearch(By.XPath(".//*[@id='tblDepartment_filter']/label/input"), Constants.NewDeptAfterEdit, By.XPath("//*[@id='tblDepartment']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");

                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Department Deleted");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Department does not exist.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Department Does not Exist.");
            }
            return Log.GetLog();
        }
    }
}