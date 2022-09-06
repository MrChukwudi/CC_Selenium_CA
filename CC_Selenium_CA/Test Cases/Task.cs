using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;

namespace Selenium_CC_CA.Initialisers
{
    public class Task : Constants
    {
        public static string PagePath = "/incidenttask/mytask";
        public static string TaskUrl = "";

        public static List<LogItem> Accept()
        {
            Log.Clear();
            string testName = "Task acceptance";
            // go to the task manager
            CheckUrl(Constants.BaseUrl + PagePath);

            GoToTask();

            //
            // 1. Accept task
            //

            WaitForElement(By.Id("acrtasklist"), true);
            TaskUrl = Constants.driver.Url; // save task url for later use
            IWebElement acceptButton = null;

            if (!ClickTaskActionButton("btn_accept_task0"))
            {
                // if the button was not found, show all tasks and look for a task that can be accepted.
                try
                {
                    ClickElement(By.Id("task_filters"));
                    ClickElement(By.LinkText("All Tasks"));
                    ClickElement(By.ClassName("task-unallocated"));
                    WaitForElement(By.ClassName("btn_accept_task"), true);
                    if (IsElementPresent(By.Id("btn_accept_task0"), false))
                    {
                        acceptButton = Constants.driver.FindElement(By.Id("btn_accept_task0"));
                    }
                    else if (IsElementPresent(By.Id("btn_accept_task1"), false))
                    {
                        acceptButton = Constants.driver.FindElement(By.Id("btn_accept_task1"));
                    }
                    else
                    {
                        Log.Entry(Log.Fail, testName, "No tasks found to accept.");
                    }

                }
                catch (Exception)
                {
                    Log.Entry(Log.Fail, testName, "Error trying to accept task.");
                }
            }

            if (acceptButton != null)
            {
                acceptButton.ClickUntilCondition(By.ClassName("sweet-overlay"), true);

            }

            //
            // 2. check popup message
            //

            if (FindAlert_SA()[0].Equals("Task Acceptance confirmation"))
            {
                ShortWait();
                //WaitForElement(By.Id("waiting_for_javascripts_to_load")); // force to wait before clicking - if click is too fast, nothing happens.
                ClickElement(By.ClassName("confirm"));
                if (!Constants.UseSso)
                    CheckIfPasswordIsRequired();
                WaitForElement(By.XPath(string.Format("//*[contains(text(),'{0}')]", Constants.SuccessMsg[0])), false);
            }

            LogAlert(testName);

            return Log.GetLog();

        }

        public static List<LogItem> Checklist()
        {
            Log.Clear();
            string testName = "Complete Checklist";
            GoToTaskPage();

            WaitForElement(By.XPath("//div[contains(@class, 'task_active_checklist_wapper')]"), true);

            try
            {
                var items = Constants.driver.FindElements(By.XPath("//div[contains(@class, 'task_active_checklist_wapper')]/form/div/div/label"));
                int i = 0, loopCount = 0, itemCount = items.Count;
                while (itemCount > 0 && loopCount <= 5)
                {
                    if (items[i].Enabled)
                    {
                        loopCount++;
                        try
                        {
                            WebDriverWait wait = new WebDriverWait(Constants.driver, Constants.LongWait);
                            wait.Until(driver => items[i].Enabled && items[i].Displayed);
                            ShortWait();
                            items[i].Click();
                            IWebElement parent = items[i].GetParent();
                            try
                            {
                                parent.FindElement(By.XPath("//span[contains(@class,'select2-selection__rendered')]")).Click();
                                ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));
                            }
                            catch (Exception) { }
                            //parent.FindElement(By.XPath("//input[@name='ChkComment[]']")).SendKeys("sample comment"); //Once response Item is selected it automatically saves the task now
                            //WriteInElement(By.XPath("//input[@name='ChkComment[]']"), "sample comment");
                            //parent.FindElement(By.XPath("//button[@name='btn_submit']")).Click();

                            if (IsElementDisplayed(By.Id("toast-container"), true))
                            {
                                LogAlert($"{testName} {loopCount}");
                            }
                            else
                            {
                                Log.Entry(Log.Fail, $"{testName} {loopCount}", "Saving the checklist response failed.");
                            }

                            //ClickElement(By.XPath("//button[@class='confirm']"));
                            items = Constants.driver.FindElements(By.XPath("//div[contains(@class, 'task_active_checklist_wapper')]/form/div/div/label"));
                            if (items.Count == itemCount)
                            {
                                i++;
                            }
                            else
                            {
                                itemCount = items.Count;
                                i = 0;
                            }
                        }
                        catch (Exception e)
                        {
                            if (e is NoSuchWindowException)
                                throw e;
                            else
                            {
                                Log.Entry(Log.Error, testName, e.Message);
                            }

                        }
                    }
                }
                if (loopCount == 0)
                {
                    Log.Entry(Log.Fail, testName, "No checklist items were found.");
                }
            }
            catch (Exception ex)
            {
                if (ex is NoSuchWindowException)
                    throw ex;
                else
                {
                    Log.Entry(Log.Fail, testName, ex.Message);
                }

            }
            return Log.GetLog();
        }

        public static List<LogItem> Send_Update()
        {
            Log.Clear();
            string testName = "Task - send update";
            GoToTaskPage();

            ClickTaskActionButton("btn_update_task0");

            WaitForElement(By.Id("taskupdate"), true);
            // write message.
            WriteInElement(By.Id("TaskActionReason"), "Task update test");
            // remove extra comm methods
            Incident.RemoveExtraComms();
            // close the dropdown list
            Constants.driver.FindElement(By.ClassName("modal-header")).Click();
            // send update
            ClickElement(By.Id("btn_send_update"));

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Reallocation()
        {
            Log.Clear();
            string testName = "Task reallocation";
            GoToTaskPage();

            ClickTaskActionButton("btn_allocate_task0");
            ShortWait(); // force to wait before clicking - if click is too fast, nothing happens.

            ClickElement(By.ClassName("confirm"));
            CheckIfPasswordIsRequired();
            WaitForElement(By.Id("taskreallocate"), true);
            WriteInElement(By.Id("TaskActionReason"), Constants.NewIncident);
            ClickElement(By.Id("btn_confirm_task_action"));

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Delegation()
        {
            Log.Clear();
            string testName = "Task delegation";
            GoToTaskPage();

            ClickTaskActionButton("btn_delegate_task0");
            ShortWait(); // force to wait before clicking - if click is too fast, nothing happens.

            ClickElement(By.ClassName("confirm"));
            CheckIfPasswordIsRequired();
            WaitForElement(By.Id("taskdelegate"), true);

            // Selecting Group to Delegate the Task to.
            ClickElement(By.XPath("//*[@id='taskdelegate']/div[1]/div[1]/div[2]/span/span[1]/span"));
            WriteInElement(By.XPath("//*[@id='taskdelegate']/div[1]/div[1]/div[2]/span/span[1]/span/ul/li/input"), Constants.NewDepartmentAfterEdit);
            ClickElement(By.CssSelector(".select2-results__option--highlighted")); // Selecting Search Result 

            // Selecting User to Delegate the Task To.
            ClickElement(By.XPath("//*[@id='taskdelegate']/div[1]/div[1]/div[3]/span")); // Old: Click Elm By. ID ' select2-DelegateTo-container ' 
            WriteInElement(By.XPath("//*[@id='taskdelegate']/div[1]/div[1]/div[3]/span/span[1]/span/ul/li/input"), Constants.UserNumber3);
            ClickElement(By.CssSelector(".select2-results__option--highlighted")); // Selecting Search Result 

            WriteInElement(By.XPath("//textarea[contains(@id,'TaskActionReason')]"), Constants.NewIncident);

            ClickElement(By.Id("btn_confirm_task_action"));

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Complete()
        {
            Log.Clear();
            GoToTaskPage();
            string testName = "Task completion";

            ClickTaskActionButton("btn_complete_task0");
            ShortWait(); // force to wait before clicking - if click is too fast, nothing happens.

            //ClickElement(By.ClassName("confirm"));

            CheckIfPasswordIsRequired();

            WaitForElement(By.Id("completetask"), false);
            WriteInElement(By.Id("TaskActionReason"), Constants.SimpleText);
            // select recipients
            //ClickElement(By.CssSelector("input.select2-search__field"));
            //WriteInElement(By.CssSelector("input.select2-search__field"), Constants.KeyContacts + Keys.Enter);
            // if key contacts are not selected, select them
            // and if escalation team i selected, deselect it
            IWebElement kcCheckBox = Constants.driver.FindElement(By.Id("chkKEY_CONTACTS"));
            if (!kcCheckBox.Selected)
            {
                ClickElement(By.XPath("//label[@for='chkKEY_CONTACTS']"));

            }
            IWebElement escCheckBox = Constants.driver.FindElement(By.Id("chkESCALATION_MEMBERS"));
            if (escCheckBox.Selected)
            {
                ClickElement(By.XPath("//label[@for='chkESCALATION_MEMBERS']"));
            }

            ClickElement(By.Id("btn_confirm_task_action"));

            LogAlert(testName);

            return Log.GetLog();
        }

        /// <summary>
        /// Check if the task page is saved, if not, go the long way, and save the page.
        /// </summary>
        private static void GoToTaskPage()
        {
            if (string.IsNullOrWhiteSpace(TaskUrl))
            {
                GoToTask();
                TaskUrl = Constants.driver.Url; // save task url for later use
            }
            else
            {
                CheckUrl(TaskUrl);
            }
        }

        /// <summary>
        /// Go to the task view with action buttons.
        /// </summary>
        private static void GoToTask()
        {
            //
            // 1. Select incident
            //

            // get all the incidents available from the list. TODO check what happens if the list is empty try catch block might be needed.
            IReadOnlyCollection<IWebElement> incidents = Constants.driver.FindElements(By.XPath("//*[contains(@class,'task_incident_title')]"));
            // Find the first occurence of the wanted incident.
            IWebElement wantedIncident = null;
            if (incidents.Count > 0)
            {
                foreach (IWebElement incident in incidents)
                {
                    if (incident.Text.Contains(Constants.NewIncident) && wantedIncident == null)
                    {
                        wantedIncident = incident;
                    }
                }

                // if found, click and go to the task list for that incident.
                if (wantedIncident != null)
                {
                    wantedIncident.ClickUntilCondition(By.Id("incidenttask_mytasklist"), true);
                }
                else
                {
                    // TODO add error in the log.
                }
            }
            else
            {
                // TODO log error
            }

            //
            // 2. select task
            //

            // get all the tasks available from the list.
            IReadOnlyCollection<IWebElement> tasks = Constants.driver.FindElements(By.XPath("//*[contains(@class,'indes')]"));
            IWebElement wantedTask = null;
            if (tasks.Count > 0)
            {
                foreach (IWebElement task in tasks)
                {
                    if (task.Text.Contains(Constants.NewIncident) && wantedTask == null)
                    {
                        wantedTask = task;
                    }
                }

                if (wantedTask != null)
                {
                    wantedTask.ClickUntilCondition(By.Id("incidenttask_task"), true);
                }
                else
                {
                    // TODO add error in the log
                }
            }
            else
            {
                // TODO add error in the log
            }
        }

        /// <summary>
        /// If a popup asking for the password shows, enter password and accept.
        /// </summary>
        private static void CheckIfPasswordIsRequired()
        {
            WaitForElement(By.Id("confirmtaskaction"), true);
            if (IsElementPresent(By.Id("confirmtaskaction"),true))
            {
                WriteInElement(By.Id("TaskConfirmPwd"), password);
                WaitForElement(By.Id("btn_confirm_task_action"), true);
                if (driver.GetType().Name.ToLower().Contains("firefox"))
                {
                    ShortWait();
                    driver.FindElement(By.Id("btn_confirm_task_action")).Submit();
                }
                else
                {
                    ClickElement(By.Id("btn_confirm_task_action"));
                }
            }
        }

        /// <summary>
        /// avoid repeating code.
        /// </summary>
        /// <param name="buttonId"></param>
        private static bool ClickTaskActionButton(string buttonId)
        {
            try
            {
                if (IsElementPresent(By.Id(buttonId), true))
                {
                    ClickElement(By.Id(buttonId));
                }
                else
                {
                    ClickElement(By.Id(buttonId.Replace('0', '1')));
                }
                return true;
            }
            catch (Exception)
            {
                // TODO log error
                return false;
            }
        }
    }
}
