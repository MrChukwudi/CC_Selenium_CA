using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{

    public class Report : Constants
    {
        private const string PingReportPath = "/reports/pingreport";
        private const string IncidentReportPath = "/reports/incidentreport";
        private const string UserReportPath = "/reports/userreport";
        private const string PerformReportPath = "/reports/performancereport";
        private const string DeviceReportPath = "/reports/devicereport";
        private const string DataExportPath = "/reports/dataexport";
        private const string DataAuditPath = "/reports/dataaudit";

        public static List<LogItem> Ping_Report()
        {
            Log.Clear();
            string subtest = "Group ping report drill down";
            Console.WriteLine(subtest);
            bool success = false;
            // go to the ping report page
            CheckUrl(BaseUrl + PingReportPath);

            //
            // 1. group ping report
            //

            // select custom period
            ClickElement(By.XPath("//label[contains(text(),'Custom Period')]"));

            SelectOldDate(By.Id("grp_from_date"));

            // get all the groups available in the dropdown
            var groups = GetGroupListfromDropDown();

            // iterate through the groups
            for (int i = 0; i < groups.Count; i++)
            {
                // show the list if needed
                try
                {
                    if (!groups[i].Enabled)
                    {
                        groups = GetGroupListfromDropDown();
                    }
                }
                catch (Exception)
                {
                    groups = GetGroupListfromDropDown();
                }

                try
                {
                    // select the group
                    groups[i].Click();
                    // get the data
                    ClickElement(By.Id("grpbtnSubmit"));
                    ShortWait();

                    // if alert, try again
                    if (FindAlert()[0] == "Alert")
                    {
                        ClosePopUp("Alert");
                    }
                    else
                    {
                        success = DrillDownPieChart(By.Id("group_ping_pie"), By.CssSelector("g.highcharts-series > path"), By.Id("btnGrpPieDrillClose"));

                        if (success)
                        {
                            ShortWait();
                            MoveToElement(By.Id("btnGrpPieDrillClose"));
                            ClickElement(By.Id("btnGrpPieDrillClose"));
                            WaitForElement(By.Id("select2-group_filter-container"), true);
                            break;
                        }
                    }
                }
                catch
                {
                    //do nothing.
                }

            }

            if (success)
            {
                // log success
                Log.Entry(Log.Pass, subtest, "drill down successful.");
                Console.WriteLine("Test: " + subtest + " | Result: PASS " + " | Outcome: drill down successful.");
            }
            else
            {
                // log failure
                Log.Entry(Log.Fail, subtest, "drill down not successful");
                Console.WriteLine("Test: " + subtest + " | Result: FAIL " + " | Outcome: drill down not successful.");
            }

            //
            // 2. user ping report
            //

            subtest = "User ping report drill down";
            Console.WriteLine(subtest);
            // reset things for the user ping report test
            success = false;

            // scroll down
            MoveToElement(By.XPath("//div[@id='user_report_params']/div[2]/div/div/label[4]"));
            ClickElement(By.XPath("//div[@id='user_report_params']/div[2]/div/div/label[4]"));

            SelectOldDate(By.Id("usr_from_date"));

            // get the data
            ClickElement(By.Id("usrbtnSubmit"));
            ShortWait(); // this might be unnecessary.
            ScrollToBottom();
            success = DrillDownPieChart(By.Id("user_ping_pie"), By.CssSelector("#user_ping_pie > [id|=highcharts] > svg > .highcharts-series-group > .highcharts-series > path"), By.Id("btnGrpPieDrillClose"));

            if (success)
            {
                // log success
                Log.Entry(Log.Pass, subtest, "drill down successful.");
                Console.WriteLine("Test: " + subtest + " | Result: PASS " + " | Outcome: drill down successful.");
            }
            else
            {
                // log failure
                Log.Entry(Log.Fail, subtest, "drill down unsuccessful.");
                Console.WriteLine("Test: " + subtest + " | Result: FAIL " + " | Outcome: drill down not successful.");
            }

            return Log.GetLog();
        }

        /// <summary>
        /// select an old date in the date time picker
        /// this function is to avaid replicated code.
        /// </summary>
        /// <param name="dateTimePicker"></param>
        private static void SelectOldDate(By dateTimePicker)
        {
            // select a date a while back as "from date"
            ClickElement(dateTimePicker);
            ClickElement(By.CssSelector(".datepicker-days .datepicker-switch")); //".datepicker-days > table.table-condensed > thead > tr:nth-of-type(2) > th.datepicker-switch")); // div.datepicker:nth-child(35) > div:nth-child(1) > table:nth-child(1) > thead:nth-child(1) > tr:nth-child(2) > th:nth-child(2)
            if (IsElementDisplayed(By.CssSelector(".datepicker-days .datepicker-switch"), false))
            {
                ClickElement(By.CssSelector(".datepicker-days .datepicker-switch"));
            }
            ClickElement(By.CssSelector(".datepicker-months .datepicker-switch")); //.datepicker-months > table.table-condensed > thead > tr:nth-of-type(2) > th.datepicker-switch"));

            ClickElement(By.CssSelector("div.datepicker-years > table.table-condensed > thead > tr:nth-child(2) > th.prev"));
            ClickElement(By.CssSelector("table.table-condensed > tbody > tr > td > .year:nth-of-type(2)"));
            ClickElement(By.CssSelector("table.table-condensed > tbody > tr > td > .month:nth-of-type(1)"));
            ClickElement(By.CssSelector("table.table-condensed > tbody > tr:nth-of-type(2) > td.day:nth-of-type(1)"));
        }

        /// <summary>
        /// drill down the pie chart. avoid replicated code.
        /// </summary>
        /// <param name="pieSelector"></param>
        /// <param name="sliceSelector"></param>
        /// <param name="waitSelector"></param>
        /// <returns></returns>
        private static bool DrillDownPieChart(By pieSelector, By sliceSelector, By waitSelector)
        {
            bool success = false;
            // if no alert, keep going
            if (FindAlert()[0] != "Alert")
            {
                MoveToElement(pieSelector);
                // get the pie chart slices 
                var slices = Constants.driver.FindElements(sliceSelector);
                if (slices.Count > 0)
                {
                    foreach (IWebElement slice in slices)
                    {
                        try
                        {
                            slice.Click();
                            if (WaitForElement(waitSelector, true))
                            {
                                success = true;
                                break;
                            }
                        }
                        catch (Exception)
                        {
                            // do nothing
                        }
                    }
                }

            }

            return success;
        }

        /// <summary>
        /// get the groups from the dropdown list. 
        /// </summary>
        /// <returns></returns>
        private static System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> GetGroupListfromDropDown()
        {
            System.Collections.ObjectModel.ReadOnlyCollection<IWebElement> groups;
            ScrollToTop();
            ClickElement(By.Id("select2-group_filter-container"));
            groups = Constants.driver.FindElements(By.XPath("//*[@id='select2-group_filter-results']/li[contains(@class,'select2-results__option')]"));
            return groups;
        }

        public static List<LogItem> Incident_Report()
        {
            Log.Clear();
            string testName = "Incident report";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + IncidentReportPath);
            ClickElement(By.XPath("//label[contains(text(),'This Week')]"));
            //ClickElement(By.Id("grp_from_date"));
            //ClickElement(By.Id("grpbtnSubmit"));
            ShortWait();

            if (IsElementDisplayed(By.XPath("//div[contains(@class, 'sweet-alert showSweetAlert visible')]"), false))
                LogAlert(testName); 
            else
            {
                WriteInElement(By.XPath("//input[@class = 'form-control input-inline']"), Constants.NewIncident);
                ShortWait();

                // sort found incidents starting from the newest
                ClickElement(By.CssSelector(".sorting:nth-child(5)"));
                WaitForElement(By.CssSelector("th.sorting_asc"), true);
                ClickElement(By.CssSelector("th.sorting_asc"));

                ClickElement(By.XPath("//a[contains(@class, 'drillIncident btn-link' )]")); //Clicks on the Incident Name
                WaitForElement(By.Id("tblIncident"), true);

                var progressBars = Constants.driver.FindElements(By.XPath("//td/div[contains(@class,'progress')]/div[contains(@class,'progress-bar acklist progress-bar-success cursorp')]"));
                if (progressBars.Count > 0)
                {
                    foreach (IWebElement progressBar in progressBars)
                    {
                        try
                        {
                            progressBar.Click();
                            if (WaitForElement(By.Id("tbl_drill_message"), false))
                            {
                                if (TableSearch(By.XPath("//*[@id='tbl_drill_message_filter']/label/input"), Constants.UserNumber1, By.XPath("//*[@id='tbl_drill_message']/tbody/tr[1]/td[1]")))
                                {
                                    Log.Entry(Log.Pass, testName, "test successful");
                                }
                                else
                                {
                                    if (IsElementPresent(By.Id("btn_close_ack_list"), false))
                                    {
                                        ClickElement(By.Id("btn_close_ack_list"));
                                        WaitForElement(By.Id("tblIncident"), true);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                else
                {
                    // if you get here, the test was not successful;
                    Log.Entry(Log.Fail, testName, Constants.ErrorMsg[0]);
                    Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Incident Report FAILED.");
                }
            }
            return Log.GetLog();
        }

        public static List<LogItem> User_Report()
        {
            Log.Clear();
            bool success = false;
            string testName = "User report";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + UserReportPath);
            ClickElement(By.XPath("//label[contains(text(),'This Month')]"));
            ShortWait();

            if (FindAlert()[0] == "Alert")
            {
                ClosePopUp("Alert");
                ClickElement(By.XPath("//label[contains(text(),'Last Month')]"));
                ShortWait();
            }
            if (IsElementDisplayed(By.XPath("//div[contains(@class, 'sweet-alert showSweetAlert visible')]"), false))
                LogAlert(testName);

            var messageCounts = Constants.driver.FindElements(By.CssSelector("a[id*=ActId]"));
            foreach (IWebElement messageCount in messageCounts)
            {
                try
                {
                    if (!messageCount.Displayed)
                    {
                        MoveToElement(messageCount);
                    }
                    messageCount.Click();
                    if (WaitForElement(By.Id("closeMsgDrill"), false))
                    {
                        if (IsElementPresent(By.XPath(string.Format("//*[@id='userdrillmessage']/tbody/tr/td[2][contains(text(),'{0}')]", Constants.UserNumber1)), false)) // Table.XPath("//*[@id='userdrillmessage']/tbody/tr/td[2]", Constants.UserNumber1))
                        {
                            success = true;
                            break;
                        }
                        else
                        {
                            ClickElement(By.Id("closeMsgDrill"));
                        }
                    }
                }
                catch
                {
                    // do nothing
                }
            }

            if (success)
            {
                Log.Entry(Log.Pass, testName, "Success");
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: User Report Successful.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "FAILED");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: User Report Failed.");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Performance_Report()
        {
            bool success = false;
            string testName = "Performance report";
            Console.WriteLine(testName);
            Log.Clear();

            CheckUrl(BaseUrl + PerformReportPath);
            ClickElement(By.XPath("//label[contains(text(),'Custom Period')]"));
            SelectOldDate(By.Id("perf_from_date"));

            ClickElement(By.Id("perfbtnSubmit"));

            ShortWait();

            if (IsElementDisplayed(By.XPath("//div[contains(@class, 'sweet-alert showSweetAlert visible')]"), false))
                LogAlert(testName);

            var chartBars = Constants.driver.FindElements(By.CssSelector("g[class*=highcharts-series-] > rect"));
            if (chartBars.Count > 0)
            {
                foreach (IWebElement bar in chartBars)
                {
                    try
                    {
                        if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
                        {
                            // two clicks needed on firefox.
                            bar.Click();
                        }
                        bar.Click();
                        if (WaitForElement(By.Id("btnkpiusersclose_incident"), false) || WaitForElement(By.Id("btnkpiusersclose_ping"), false))
                        {
                            success = true;
                            break;
                        }
                    }
                    catch (Exception)
                    {
                        // do nothing
                    }
                }
            }

            if (success)
            {
                Log.Entry(Log.Pass, testName, "Success");
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Performance Report Successful.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "FAILED");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Performance Report Failed.");
            }

            return Log.GetLog();
        }
        public static List<LogItem> Device_Report()
        {
            Log.Clear();
            string testName = "Device report";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + DeviceReportPath);

            if (TableSearch(By.XPath("//input[@class = 'form-control input-inline']"), Constants.UserNumber1, By.XPath("//*[@id='devicereporttbl']/tbody/tr/td[1]")))
            {
                Log.Entry(Log.Pass, testName, "devices found");
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Device found for User.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "no device found for this user");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: no device found for this user.");
            }
            return Log.GetLog();   
        }
        public static string[] Data_Export()
        {
            CheckUrl(BaseUrl + DataExportPath);

            ClickElement(By.XPath("//*[@id='tblDataExport']/tbody/tr[1]/td[2]"));
            ClickElement(By.XPath("//*[@id='tblDataExport']/tbody/tr[2]/td[2]"));
            ClickElement(By.XPath("//*[@id='tblDataExport']/tbody/tr[3]/td[2]"));
            ClickElement(By.XPath("//*[@id='tblDataExport']/tbody/tr[4]/td[2]"));

            return SuccessMsg;

        }
        public static string[] Data_Audit()
        {
            CheckUrl(BaseUrl + DataAuditPath);
            ClickElement(By.XPath("//*[@id='select2-module-container']"));
            ClickElement(By.XPath("//option[contains(text(),'Locations')]"));

            ClickElement(By.XPath("//*[@id='select2-moduleitem-container']"));
            ClickElement(By.XPath("//option[contains(text(),'Locations')]"));
            WriteInElement(By.XPath("//input[contains(@class, 'select2-search__field')]"), NewLocation + Keys.Enter);

            //ClickElement(By.XPath("//label[contains(text(),'Last Month')]"));

            ClickElement(By.XPath("//label[contains(text(),'Custom Period')]"));

            ClickElement(By.XPath("#audit_date_set > div:nth-child(2) > div > div > i"));
            ClickElement(By.CssSelector("#reports_dataaudit > div.datepicker.datepicker-dropdown.dropdown-menu.datepicker-orient-left.datepicker-orient-top > div.datepicker-days > table > thead > tr:nth-child(2) > th.datepicker-switch"));
            ClickElement(By.CssSelector("#reports_dataaudit > div.datepicker.datepicker-dropdown.dropdown-menu.datepicker-orient-left.datepicker-orient-top > div.datepicker-months > table > thead > tr:nth-child(2) > th.datepicker-switch"));
            ClickElement(By.CssSelector("#reports_dataaudit > div.datepicker.datepicker-dropdown.dropdown-menu.datepicker-orient-left.datepicker-orient-top > div.datepicker-years > table > tbody > tr > td > span:nth-child(2)"));

            ClickElement(By.CssSelector("#reports_dataaudit > div.datepicker.datepicker-dropdown.dropdown-menu.datepicker-orient-left.datepicker-orient-top > div.datepicker-months > table > tbody > tr > td > span:nth-child(1)"));
            ClickElement(By.CssSelector("#reports_dataaudit > div.datepicker.datepicker-dropdown.dropdown-menu.datepicker-orient-left.datepicker-orient-top > div.datepicker-days > table > tbody > tr:nth-child(1) > td:nth-child(6)"));

            ClickElement(By.Id("auditbtnSubmit"));

            if (TableSearch(By.XPath("//input[@class = 'form-control input-inline']"), UserNumber1, By.XPath("//*[@id='tblAudit']/tbody/tr/td[2]")))
                return SuccessMsg;
            else
                return ErrorMsg;
        }
    }
}
