using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace Selenium_CC_CA.Initialisers
{

    public class Location : Constants
    {

        private const string PagePath = "/location";
        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Create Location";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + PagePath);

            if (!TableSearch(By.XPath(".//*[@id='tblLocation_filter']/label/input"), Constants.NewLocation, By.XPath("//*[@id='tblLocation']/tbody/tr[1]/td[contains(@class, 'sorting_1')]/span")))
            {
                ClickElement(By.XPath("//a[contains(@class,'btn btn-primary dropdown-toggle')]"));
                ClickElement(By.XPath("//a[contains(@href,'" + BaseUrl + "/location/create')]"));

                WriteInElement(By.Id("LocationName"), Constants.NewLocation);
                WriteInElement(By.Id("Description"), "Description");

                WriteInElement(By.Id("LocationAddress"), "14 Roger St, London WC1N 2LN, UK");

                ClickElement(By.Id("address_map"));
                ShortWait();
                WaitForElement(By.Id("autocomplete"), true);
                while (IsElementDisplayed(By.Id("autocomplete"),false))
                {
                    WriteInElement(By.Id("autocomplete"), "14 Roger St, London");
                    ClickElement(By.ClassName("pac-item"));
                    ShortWait();
                    ClickElement(By.Id("select_address"));

                    ShortWait();
                }

                ClickElement(By.Id("btn_submit"));
                //WaitForElement(By.ClassName("sa-button-container"), true);

                // log test result
                LogAlert(testName);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Location Created");

            }
            else
            {
                Log.Entry(Log.Fail, testName, "Location already exists.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Location Already exists.");
            }

            // return log
            return Log.GetLog();
        }

        public static List<LogItem> Edit()
        {
            Log.Clear();
            
            string membersTab = "Users"; //Members
            string detailsTab = "Details";
            //string membersLink = "#group_members";
            //string detailsLink = "#group_details";

            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), NewLocation, By.XPath("//*[@id='tblLocation']/tbody/tr[1]/td[3]")))
            {
                //ClickElement(By.XPath("//*[@id='tblLocation']/tbody/tr[1]/td[2]"));
                //ClickElement(By.XPath("//a[contains(@class,'btn btn-sm btn-small btn-warning')]"));
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));

                //
                // 1. Add users to the group
                //

                string subtest = "Add members to Location";

                WaitForElement(By.LinkText(membersTab), true);
                ClickElement(By.LinkText(membersTab));

                WaitForElement(By.Id("GroupsMembers"), true);
                if (!TableSearch(By.XPath("//input[contains(@aria-controls,'GroupsMembers')]"), UserNumber1, By.XPath("//*[@id='GroupsMembers']/tbody/tr[1]/td[2]")))
                {
                    if (TableSearch(By.XPath("//input[contains(@aria-controls,'NonMemberUsers')]"), UserNumber1, By.XPath("//*[@id='NonMemberUsers']/tbody/tr[1]/td[2]")))
                    {
                        ClickElement(By.XPath(".//td[4]/div/label"));
                    }
                }
                else
                {
                    Log.Entry(Log.Fail, subtest, "user already present in location");
                }

                // check the user has been added and doing so allow time for the completion of the selection
                if (TableSearch(By.XPath("//input[contains(@aria-controls,'GroupsMembers')]"), UserNumber1, By.XPath("//*[@id='GroupsMembers']/tbody/tr[1]/td[2]")))
                {
                    Log.Entry(Log.Pass, subtest, "user now present in location");
                    Console.WriteLine("Test: " + subtest + " | Result: PASS " + " | Outcome: User now present in the Location.");
                }
                else
                {
                    Log.Entry(Log.Fail, subtest, "user not added");
                    Console.WriteLine("Test: " + subtest + " | Result: FAIL " + " | Outcome: User not added to Location.");
                }


                //
                // 2. Edit the Location Description
                //

                subtest = "Edit description";

                ClickElement(By.LinkText(detailsTab));

                WriteInElement(By.XPath("//textarea[contains(@id, 'Description')]"), SimpleText);

                //
                // 3. save the changes
                //

                //ClickElement(By.XPath(".//*[@id='btn_submit']"));
                ClickElement(By.Id("btn_submit"));
                // log test result
                LogAlert(subtest);
                Console.WriteLine("Test: " + subtest + " | Result: PASS " + " | Outcome: Location Discription Updated.");
            }
            else
            {
                Log.Entry(Log.Fail, "Edit Location", "Location to edit not found");
                Console.WriteLine("Test: Edit Location" + " | Result: FAIL " + " | Outcome: Location to Edit not Found.");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            CheckUrl(BaseUrl + PagePath);
            string subTest = "Delete Location";

            // Wait for filtering to be complete
            if (TableSearch(By.XPath("//*[@id='tblLocation_filter']/label/input"), NewLocation, By.XPath("//*[@id='tblLocation']/tbody/tr[1]/td[3]")))
            {

                ClickElement(By.XPath("//i[contains(@class,'far fa-plus-circle fa-2x cursorp')]"));

                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                WaitForElement(By.LinkText("Delete"), true);
                ClickElement(By.LinkText("Delete"));

                ShortWait();
                ClosePopUp("Delete Confirmation");
                //ShortWait();

                LogAlert(subTest);
            }
            else
            {
                Log.Entry(Log.Fail, "Delete Location", "Location to delete not found.");
                Console.WriteLine("Test: Delete Location" + " | Result: FAIL " + " | Outcome: Location to delete not Found.");
            }
            //FindAlert();
            return Log.GetLog();
        }

    }


}


