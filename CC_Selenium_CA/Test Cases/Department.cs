using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace Selenium_CC_CA.Initialisers
{

    public class Department : Constants
    {
        private const string PagePath = "/group";


        public static List<LogItem> Create()
        {
            string testName = "Create Group";
            Log.Clear();

            CheckUrl(BaseUrl + PagePath);

            if (!TableSearch(By.XPath(".//*[@id='tblGroup_filter']/label/input"), Constants.NewDepartment, By.XPath("//*[@id='tblGroup']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//a[contains(@class,'btn btn-primary dropdown-toggle')]"));
                ClickElement(By.XPath("//a[contains(@href,'" + Constants.BaseUrl + "/group/create')]"));

                WaitForElement(By.Id("GroupName"), false);
                WriteInElement(By.Id("GroupName"), Constants.NewDepartment);

                ClickElement(By.Id("btn_submit"));

                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Group Created");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Group already exists");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Group already Exists");
            }
            return Log.GetLog();
        }


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string subTest = "add first member";
            string membersTab = "Users";
            string detailsTab = "Details";
            //string membersLink = "#group_members";
            //string detailsLink = "#group_details";

            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class,'form-control input-inline')]"), Constants.NewDepartment, By.XPath("//*[@id='tblGroup']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//*[@id='tblGroup']/tbody/tr[1]/td[2]"));

                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));

                WaitForElement(By.Id("GroupName"), true);
                ClickElement(By.LinkText(membersTab));
                //if (driver.GetType().Name.ToString().ToLower().Contains("chrome"))
                //{
                //    driver.Navigate().GoToUrl(driver.Url + membersLink);
                //}
                //else
                //{
                //    ClickElement(By.LinkText(membersTab));
                //}

                AddUserToDepartment(UserNumber1, subTest);
                subTest = "Add second member";
                AddUserToDepartment(UserNumber2, subTest);

                subTest = "Edit Group details";
                //driver.Navigate().GoToUrl(driver.Url.Replace(membersLink, detailsLink));
                MoveToElement(By.LinkText(detailsTab));
                ScrollToTop();
                ClickElement(By.LinkText(detailsTab));
                WaitForElement(By.Id("GroupName"), false);
                WriteInElement(By.Id("GroupName"), Constants.NewDepartmentAfterEdit);
                ClickElement(By.Id("btn_submit"));
                LogAlert(subTest);
            }

            return Log.GetLog();
        }

        /// <summary>
        /// Assign a user to the current department. call from the member tab.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="subTest"></param>
        private static void AddUserToDepartment(string user, string subTest)
        {
            if (!TableSearch(By.XPath("//input[contains(@aria-controls,'GroupsMembers')]"), user, By.XPath("//*[@id='GroupsMembers']/tbody/tr[1]/td[2]")))
            {
                if (TableSearch(By.XPath("//input[contains(@aria-controls,'NonMemberUsers')]"), user, By.XPath("//*[@id='NonMemberUsers']/tbody/tr[1]/td[2]")))
                {
                    ClickElement(By.XPath("//*[@id='NonMemberUsers']/tbody/tr[1]/td[4]/div/label"));
                }

                if (TableSearch(By.XPath("//input[contains(@aria-controls,'GroupsMembers')]"), user, By.XPath("//*[@id='GroupsMembers']/tbody/tr[1]/td[2]")))
                {
                    Log.Entry(Log.Pass, subTest, "User added to the Group");
                    Console.WriteLine("Test: " + subTest + " | Result: PASS " + " | Outcome: User Added to Group");
                }
                else
                {
                    Log.Entry(Log.Fail, subTest, "User not added to the Group");
                    Console.WriteLine("Test: " + subTest + " | Result: FAIL " + " | Outcome: User not added to the Group");
                }
                driver.Navigate().Refresh();
            }
            else
            {
                Log.Entry(Log.Fail, subTest, "User already in the Group.");
                Console.WriteLine("Test: " + subTest + " | Result: FAIL " + " | Outcome: User Already in the Group");
            }
        }

        /// <summary>
        /// This will delete the test department automatically generated by Department.Create().
        /// For this to succeed, it needs to not be associated with any tasks, so make sure to delete any test incident first.
        /// The incident tasks may have to be deleted before the incident for this to work.
        /// </summary>
        /// <returns></returns>
        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testname = "Delete Group";
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath(".//*[@id='tblGroup_filter']/label/input"), Constants.NewDepartmentAfterEdit, By.XPath("//*[@id='tblGroup']/tbody/tr[1]/td[3]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");

                LogAlert(testname);
                Console.WriteLine("Test: " + testname + " | Result: PASS " + " | Outcome: Group Deleted");
            }
            else
            {
                Log.Entry(Log.Fail, testname, "Group does not exist.");
                Console.WriteLine("Test: " + testname + " | Result: FAIL " + " | Outcome: Group does not exist.");
            }

            return Log.GetLog();
        }
    }
}
