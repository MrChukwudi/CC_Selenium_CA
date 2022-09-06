using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    public class Incident : Constants
    {
        private const string SetupPagePath = "/incident/incidentsetup";
        private const string ActivePagePath = "/incident/activeincidents";
        private const string CreatePagePath = "/incident/create";
        private const string LaunchPagePath = "/incident/newincident";
        private const string AwaitingPagePath = "/incident/awaitinglaunch";

        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Create an incident";
            Console.WriteLine(testName);
            CheckUrl(Constants.BaseUrl + SetupPagePath);
            ShortWait();

            if (!TableSearch(By.XPath("//input[contains(@type,'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//a[contains(@class,'btn btn-primary dropdown-toggle')]"));
                ClickElement(By.XPath("//a[contains(@href,'" + Constants.BaseUrl + CreatePagePath + "')]"));

                WriteInElement(By.Id("Name"), Constants.NewIncident);
                ClickElement(By.Id("Name")); // necessary for next step to work

                ClickElement(By.Id("select2-IncidentType-container"));
                WriteInElement(By.XPath(".//*[@id='incident_create']/span/span/span[1]/input"), "Civil" + Keys.Enter);

                WriteInElement(By.Id("Description"), Constants.NewIncident);

                WriteInElement(By.XPath("//input[contains(@class,'select2-search__field')]"), Constants.UserNumber1);
                ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

                WriteInElement(By.XPath("//input[contains(@class,'select2-search')]"), Constants.UserNumber2);
                ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

                ClickElement(By.Id("select2-Severity-container"));
                ClickElement(By.XPath("//li[contains(text(),'5')]"));

                ClickElement(By.Id("select2-NumberOfKeyHolders-container"));
                ClickElement(By.XPath("//li[contains(text(),'1')]"));

                try
                {
                    ClickElement(By.Id("selecInciIcon"));
                    WaitForElement(By.Id("IconSearch"), false);
                    ShortWait();
                    WriteInElement(By.Id("IconSearch"), "fireball");
                    ClickElement(By.XPath("//img[@alt='Fireball']"));
                    WaitForElement(By.Id("CurIcon"), true);
                    //    // wait for the icons to be available
                    //    driver.FindElement(By.XPath("//*[@id='incidentIcons']/div/div[2]/img[33]"));
                    //    // then select the icon
                    //    ClickElement(By.XPath("//*[@id='incidentIcons']/div/div[2]/img[33]"));
                    //ClickElement(By.XPath("//*[@id='incidentIcons']/div/div[2]/img[33]"));
                }
                catch (Exception)
                {
                    Log.Entry(Log.Error, testName, "Error selecting incident icon.");
                }

                ShortWait();
                // try to click on the button until there is an alert and go through the loop a maximun of 6 times
                //MultipleTryClick(By.ClassName("sa-confirm-button-container"), true, By.Id("btn_submit"));
                //int x = Constant.Loop;
                //while (!WaitForElement(By.ClassName("sa-confirm-button-container")) && x >= 0)
                //{
                //    ClickElement(By.Id("btn_submit"));
                //    x--;
                //}
                MoveToElement(By.XPath("//*[@id='incident_new']/div[3]"));
                ClickElement(By.Id("btn_submit"));

                LogAlert(testName);
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Incident already exists.");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Task_Create()
        {
            Log.Clear();
            string testName = "Create task 1";
            CheckUrl(Constants.BaseUrl + SetupPagePath);

            //
            // 1. find incident
            //

            if (TableSearch(By.XPath("//input[contains(@type,'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));

                //
                // 2. add task 1
                //

                ClickElement(By.Id("incident_task_tab"));

                if (WaitForElement(By.Id("btn_config_task"), false))
                {
                    ClickElement(By.Id("btn_config_task"));
                }

                string incidentTaskSetupPage = Constants.driver.Url;

                if (!TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewIncident, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")) &&
                    !TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewTaskAfterEdit, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")))
                {
                    ClickElement(By.Id("btn_add_task"));

                    WriteInElement(By.Id("TaskTitle"), Constants.NewIncident);
                    ClickElement(By.XPath("//iframe[contains(@class,'k-content')]"));

                    try
                    {
                        Constants.driver.SwitchTo().Frame(Constants.driver.FindElement(By.XPath("//iframe[contains(@class,'k-content')]")));
                        WriteInElement(By.XPath("/html/body"), Constants.NewIncident);
                        Constants.driver.SwitchTo().DefaultContent();

                    }
                    catch (Exception e)
                    {
                        Log.Entry(Log.Error, testName, e.Message);

                    }

                    WriteInElement(By.XPath("//input[contains(@placeholder,'Select Action Groups')]"), Constants.NewDepartment + Keys.Enter);
                    WriteInElement(By.XPath("//input[contains(@placeholder,'Select Escalation Groups')]"), Constants.NewDepartment + Keys.Enter);

                    // move the screen to show the required button - otherwise it will not work.
                    MoveToElement(By.Id("btn_save_task"));
                    //ClickElement(By.Id("btn_save_task"));
                    // try to click on the button until the button is done and go through the loop a maximun of 6 times
                    ShortWait();
                    //MultipleTryClick(By.Id("task0"), true, By.Id("btn_save_task"));
                    SaveTask();

                    CheckTaskCreated(testName, Constants.NewIncident, "Task one");
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "Task already exists.");
                }

                //
                // 3. Add Second Task
                //

                CheckUrl(incidentTaskSetupPage);
                //ShortWait();

                testName = "add task 2";
                ShortWait();
                if (!TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewSecondTask, By.XPath("//*[@id='incident_task_list']/tbody/tr[1]/td[2]")))
                {
                    ClickElement(By.Id("btn_add_task"));

                    WaitForElement(By.Id("TaskTitle"), false);
                    WriteInElement(By.Id("TaskTitle"), Constants.NewSecondTask);
                    WaitForElement(By.XPath("//input[contains(@placeholder,'Select predecessor')]"), true);
                    if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
                    {
                        // two clicks required on firefox.
                        ClickElement(By.XPath("//input[contains(@placeholder,'Select predecessor')]"));
                    }
                    ClickElement(By.XPath("//input[contains(@placeholder,'Select predecessor')]"));
                    ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

                    ClickElement(By.XPath("//iframe[contains(@class,'k-content')]"));

                    try
                    {
                        Constants.driver.SwitchTo().Frame(Constants.driver.FindElement(By.XPath("//iframe[contains(@class,'k-content')]")));
                        WriteInElement(By.XPath("/html/body"), Constants.NewIncident);
                        Constants.driver.SwitchTo().DefaultContent();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message.ToString());
                    }

                    WriteInElement(By.XPath("//input[contains(@placeholder,'Select Action Groups')]"), Constants.NewDepartment + Keys.Enter);
                    WriteInElement(By.XPath("//input[contains(@placeholder,'Select Escalation Groups')]"), Constants.NewDepartment + Keys.Enter);

                    ShortWait();
                    SaveTask();
                    //MultipleTryClick(By.Id("task0"), true, By.Id("btn_save_task"));

                    CheckTaskCreated(testName, Constants.NewSecondTask, "Task two");
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "second task already exists.");
                }

                //
                // 4. edit task 1
                //

                CheckUrl(incidentTaskSetupPage);
                ShortWait();
                testName = "Edit task 1";

                if (TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewIncident, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")))
                {
                    // move the screen to show the required buttons - otherwise it will not work.
                    EditTask();

                    WaitForElement(By.Id("TaskTitle"), true);
                    WriteInElement(By.Id("TaskTitle"), Constants.NewTaskAfterEdit);

                    ClickElement(By.Id("select_task_asset"));
                    WaitForElement(By.Id("tbl_task_asset_filter"), true);
                    do
                    {
                        ClickElement(By.XPath("//*[@id='tbl_task_asset']/tbody/tr[1]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_task_asset']/tbody/tr[2]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_task_asset']/tbody/tr[3]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_task_asset']/tbody/tr[4]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_task_asset']/tbody/tr[5]/td[1]/div/label"));
                        ClickElement(By.Id("btn_task_save"));
                        ShortWait();
                    }
                    while (IsElementDisplayed(By.Id("tbl_task_asset"), false));


                    //ShortWait();
                    //ClickElement(By.Id("btn_save_task"));
                    SaveTask();

                    CheckTaskCreated(testName, Constants.NewTaskAfterEdit, "Edited task one");
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "First task does not exist or already edited. Edit skipped.");
                }

                AddChecklistToTask(incidentTaskSetupPage);
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Incident does not exist. Task creation skipped.");
            }
            return Log.GetLog();
        }

        private static void EditTask()
        {
            MoveToElement(By.Id("btn_clone_incident"));

            ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
            ClickElement(By.LinkText("Edit"));
        }

        /// <summary>
        /// Add a checklist to the first task.
        /// </summary>
        /// <param name="taskPage"></param>
        private static void AddChecklistToTask(string taskPage)
        {
            CheckUrl(taskPage);

            string testName = "Add checklist";

            if (TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewTaskAfterEdit, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")))
            {
                AddChecklist(testName);
            }
            else if (TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), Constants.NewIncident, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")))
            {
                AddChecklist(testName);
            }
            else
            {
                Log.Entry(Log.Fail, testName, "First task does not exist. test skipped.");
            }

        }

        /// <summary>
        /// Create a checklist in the first task.
        /// </summary>
        /// <param name="testName"></param>
        private static void AddChecklist(string testName)
        {
            try
            {
                EditTask();
                WaitForElement(By.Id("grid-task-list"), true);
                if (IsElementPresent(By.Id("taskchklisttab"), false))
                {
                    ClickElement(By.Id("taskchklisttab"));
                    ScrollToTop();
                    if (!IsElementDisplayed(By.Id("chk_list_item[]"), false))
                    {
                        WriteInElement(By.Id("new_chk_list_item"), Constants.NewChecklistItem1);
                        ClickElement(By.Id("btn_add_chkitem"));
                        ShortWait();
                        WriteInElement(By.Id("new_chk_list_item"), Constants.NewChecklistItem2);
                        ClickElement(By.Id("btn_add_chkitem"));
                        ShortWait();

                        ClickElement(By.XPath("//*[@id='checklist_drag']/ol/li[1]/div[2]/div/button")); //Clicks on the Action Btn
                        ClickElement(By.LinkText("Edit"));

                        WaitForElement(By.Id("dlg_check_list_item"), true);
                        ClickElement(By.XPath("//*[@id='chk_advance_option']/div/fieldset/div[3]/div[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='chk_advance_option']/div/fieldset/div[3]/div[2]/div/label"));
                        ClickElement(By.XPath("//*[@id='chk_advance_option']/div/fieldset/div[3]/div[3]/div/label"));
                        ClickElement(By.XPath("//*[@id='chk_advance_option']/div/fieldset/div[3]/div[4]/div/label"));

                        ClickElement(By.Id("btn_confirm_option"));
                        ShortWait();
                        WaitForElement(By.Id("btn_save_task"), true);
                        ClickElement(By.Id("btn_save_task"));
                        // TODO: add better check to find out if checklist has been created properly.
                        if (WaitForElement(By.Id("incident_task_list"), false))
                        {
                            Log.Entry(Log.Pass, testName, "Checklist added");
                        }
                        else
                        {
                            Log.Entry(Log.Fail, testName, "Problem creating checklist");
                        }
                    }
                    else
                    {
                        Log.Entry(Log.Fail, testName, "Checklist already exixsts.");
                    }
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "Checklist not available.");
                }
            }
            catch (Exception ex)
            {
                if (ex is OpenQA.Selenium.NoSuchWindowException)
                {
                    throw ex;
                }
                else
                {
                    Log.Entry(Log.Error, testName, ex.Message);
                }
            }
        }

        /// <summary>
        /// Tasks are hard to save, this makes sure it works - well it fails less.
        /// </summary>
        private static void SaveTask()
        {
            MoveToElement(By.Id("btn_save_task"));
            ClickElement(By.Id("btn_save_task"));
            ShortWait();

            if (!IsElementDisplayed(By.Id("task_list_table"), false))
            {
                MoveToElement(By.Id("btn_save_task"));
                ClickElement(By.Id("btn_save_task"));
            }
        }

        /// <summary>
        /// check the task has been successfully created. call from the task list page.
        /// </summary>
        /// <param name="testName">test name for logging purposes</param>
        /// <param name="taskName">task name for searching task</param>
        /// <param name="taskDescriptionForLog">task description for feedback (used in log)</param>
        private static void CheckTaskCreated(string testName, string taskName, string taskDescriptionForLog)
        {
            // move the screen to show the required buttons - otherwise it will not work.
            ShortWait();
            MoveToElement(By.Id("btn_clone_incident"));
            // search for the newly created task.
            if (TableSearch(By.XPath("//div[@id='incident_task_list_filter']/label/input"), taskName, By.XPath("//table[@id='incident_task_list']/tbody/tr/td[2]")))
            {
                Log.Entry(Log.Pass, testName, " Saved successfully.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, " not saved successfully.");
            }
            // clear the search
            WriteInElement(By.XPath("//div[@id='incident_task_list_filter']/label/input"), string.Empty + Keys.Enter);
        }

        public static List<LogItem> Launch()
        {
            Log.Clear();
            string testName = "Incident launch";
            CheckUrl(Constants.BaseUrl + ActivePagePath);
            WaitForElement(By.Id("launchedIncidentTable_filter"), true);

            if (!TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewIncident, By.XPath("//*[@id='launchedIncidentTable']/tbody/tr[1]/td[2]")))
            {
                CheckUrl(Constants.BaseUrl + LaunchPagePath);
                WaitForElement(By.Id("tbl_incident_list_filter"), true);

                //WriteInElement(By.XPath("//input[contains(@class, 'form-control input-inline')]"), NewIncident);
                //WriteInElement(By.XPath("//input[contains(@type, 'search')]"), NewIncident); // TODO check this, why is are we entering the search string twice

                if (TableSearch(By.XPath("//input[contains(@type, 'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_list']/tbody/tr[1]/td[2]")))
                {
                    ClickElement(By.XPath("//a[contains(text(),'Initiate')]"));
                    WaitForElement(By.Id("Name"), true);

                    WriteInElement(By.XPath("//input[contains(@placeholder,'Select Impacted Location')]"), Constants.NewLocation + Keys.Enter);

                    ClickElement(By.XPath("//input[contains(@placeholder, 'Select Location(s) to notify')]"));
                    WriteInElement(By.XPath("//input[contains(@placeholder, 'Select Location(s) to notify')]"), Constants.NewLocation + Keys.Enter);

                    ClickElement(By.Id("btnAckOptions"));
                    WaitForElement(By.Id("tbl_msg_response_filter"), true);

                    do
                    {
                        ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[1]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[2]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[3]/td[1]/div/label"));
                        ClickElement(By.XPath("//*[@id='tbl_msg_response']/tbody/tr[4]/td[1]/div/label"));
                        ClickElement(By.Id("btn_select"));
                    }
                    while (!IsElementDisplayed(By.Id("dragging_list"), false));

                    MoveToElement(By.Id("btn_activate_incident"));
                    // keep only push as a comm method
                    RemoveExtraComms();
                    //ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
                    //ClickElement(By.XPath("//li[contains(@title,'Phone')]/span[contains(@class,'select2-selection__choice__remove')]"));
                    //ClickElement(By.XPath("//li[contains(@title,'Email')]/span[contains(@class,'select2-selection__choice__remove')]"));
                    ScrollToTop();
                    ClickElement(By.Id("Description"));
                    MoveToElement(By.Id("view_task_list"));
                    ClickElement(By.Id("view_task_list"));

                    MoveToElement(By.Id("btn_activate_incident"));
                    ClickElement(By.Id("btn_activate_incident"));

                    if (WaitForElement(By.Id("InciPwdConfirm"),true))
                    {
                        ClickElement(By.Id("AuthenPwd"));
                        WriteInElement(By.Id("AuthenPwd"), password);
                        ClickElement(By.Id("InciPwdConfirm"));
                    }

                    LogAlert(testName);
                }
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Incident already launched.");
            }


            return Log.GetLog();
        }

        internal static void RemoveExtraComms()
        {
            if (IsElementClickable(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]")))
                ClickElement(By.XPath("//li[contains(@title,'Text')]/span[contains(@class,'select2-selection__choice__remove')]"));
            if (IsElementClickable(By.XPath("//li[contains(@title,'Phone')]/span[contains(@class,'select2-selection__choice__remove')]")))
                ClickElement(By.XPath("//li[contains(@title,'Phone')]/span[contains(@class,'select2-selection__choice__remove')]"));
            if (IsElementClickable(By.XPath("//li[contains(@title,'Email')]/span[contains(@class,'select2-selection__choice__remove')]")))
                ClickElement(By.XPath("//li[contains(@title,'Email')]/span[contains(@class,'select2-selection__choice__remove')]"));
        }

        public static List<LogItem> IncidentDetails_Report()
        {
            Log.Clear();
            string testName = "incident report";
            //View Report
            if (SelectMenuItem("View Report", By.Id("tbl_active_incident_messages")))
            {
                if (WaitForElement(By.ClassName("get_conf"), false))
                {
                    ClickElement(By.ClassName("get_conf"));
                    if (FindAlert()[0] == Constants.ErrorMsg[0])
                    {
                        ClosePopUp(Constants.ErrorMsg[0]);
                    }
                    else
                    {
                        WaitForElement(By.Id("conf_list_wrapper"), true);
                        ClickElement(By.Id("btn_close"));
                    }
                }

                // TODO check the conference button
                WaitForElement(By.Id("tbl_active_incident_messages"), false);
                if (IsElementDisplayed(By.Id("tbl_active_incident_messages"), true))
                {
                    Log.Entry(Log.Pass, testName, Constants.SuccessMsg[0]);
                }
                else
                {
                    Log.Entry(Log.Fail, testName, Constants.ErrorMsg[0]);
                }
            }
            else
            {
                Log.Entry(Log.Fail, testName, "View Report not avilable");
            }
            return Log.GetLog();
        }

        public static List<LogItem> IncidentDetails_Timeline()
        {
            Log.Clear();
            string testName = "Incident timeline";

            //Incident Timeline
            if (SelectMenuItem("Incident Timeline", By.Id("timelinelist")))
            {
                WaitForElement(By.Id("timelinelist"), true);
                ClickElement(By.LinkText("Task Timeline"));

                if (WaitForElement(By.Id("task_timeline"), true)) // Displayed.Id("task_message")
                {
                    Log.Entry(Log.Pass, testName, Constants.SuccessMsg[0]);
                }
                else
                {
                    Log.Entry(Log.Fail, testName, Constants.ErrorMsg[0]);
                }
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Inicident Timeline not available");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Incident_Audit()
        {
            Log.Clear();
            string testName = "Incident audit";
            //Incident Timeline
            SelectMenuItem("Incident Audit");

            if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
            {
                ShortWait();
            }

            var tabs = Constants.driver.WindowHandles;
            if (tabs.Count > 1)
            {
                Constants.driver.SwitchTo().Window(tabs[1]);
                WaitForElement(By.Id("incidentaudit_view"), true);
                Constants.driver.Close();
                Constants.driver.SwitchTo().Window(tabs[0]);
                Log.Entry(Log.Pass, testName, Constants.SuccessMsg[0]);
            }
            else
            {
                Log.Entry(Log.Fail, testName, Constants.ErrorMsg[0]);
            }
            return Log.GetLog();
        }

        public static List<LogItem> IncidentDetails_Acknowledgements()
        {
            Log.Clear();
            string testName = "Incident Acknowledgements";
            //Acknowledgements
            if (SelectMenuItem("Acknowledgements", By.Id("btn_close_inci_list")))
            {
                WaitForElement(By.Id("tblIncident"), false);
                if (IsElementDisplayed(By.Id("tblIncident"), true))
                {
                    Log.Entry(Log.Pass, testName, Constants.SuccessMsg[0]);
                }
                else
                {
                    Log.Entry(Log.Fail, testName, Constants.ErrorMsg[0]);
                }
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Acknowledgements not available");
            }
            return Log.GetLog();

        }

        public static List<LogItem> IncidentDetails_Conference_Call()
        {
            Log.Clear();
            string testName = "Incident - Conference call";
            //Conference Call
            if (SelectMenuItem("Start Conference Call", By.Id("conf_users")))
            {
                if (WaitForElement(By.Id("conf_form"), true))
                {
                    ClickElement(By.XPath("//*[@id='conf_users']/tbody/tr[1]/td[1]/div[1]/label"));
                    ClickElement(By.XPath("//*[@id='conf_users']/tbody/tr[2]/td[1]/div[1]/label"));

                    ClickElement(By.Id("btn_init_conference"));

                    if (FindAlert()[1] == "Please select at least two or more users to start the conference.")
                    {
                        ClickElement(By.XPath("//button[contains(@class, 'confirm')]"));
                        if (!Constants.driver.FindElement(By.XPath("//*[@id='conf_users']/tbody/tr[1]/td[1]/div[1]/input")).Selected)
                        {
                            ClickElement(By.XPath("//*[@id='conf_users']/tbody/tr[1]/td[1]/div/label"));
                        }
                        if (!Constants.driver.FindElement(By.XPath("//*[@id='conf_users']/tbody/tr[2]/td[1]/div[1]/input")).Selected)
                        {
                            ClickElement(By.XPath("//*[@id='conf_users']/tbody/tr[2]/td[1]/div/label"));
                        }
                        else
                        {
                            ClickElement(By.Id("btn_init_conference"));
                        }
                    }

                }

                LogAlert(testName);
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Conference call not available");
            }
            return Log.GetLog();
        }

        public static List<LogItem> IncidentDetails_Send_Update()
        {
            Log.Clear();
            string testName = "Incident Update";
            string Push = "Push";
            //Send Update
            if (SelectMenuItem("Send Update", By.Id("btn_send_notification")))
            {
                if (WaitForElement(By.Id("notifyincident"), true))
                {

                    WriteInElement(By.Id("InciMsg"), Constants.NewIncident);

                    ClickElement(By.XPath("//input[contains(@placeholder, 'Select Location(s) to notify')]"));
                    WriteInElement(By.XPath("//input[contains(@placeholder, 'Select Location(s) to notify')]"), Constants.NewLocation);
                    ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

                    ClickElement(By.XPath("//input[contains(@placeholder, 'select communication channels')]"));
                    WriteInElement(By.XPath("//input[contains(@placeholder, 'select communication channels')]"), Push);
                    ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));
                    RemoveExtraComms();

                    ClickElement(By.Id("btn_send_notification"));
                }

                LogAlert(testName);
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Send Update not available");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Deactivate()
        {
            Log.Clear();
            string testName = "Incident Deactivation";

            // try to click on close
            if (SelectMenuItem("Close", By.Id("InciCancel")))
            {
                SubmitClosingform();
            }
            else
            {
                // if close is not available, try to click on deactivate.
                if (SelectMenuItem("Deactivate", By.Id("InciCancel")))
                {
                    SubmitClosingform();
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "Deactivate or Close not available.");
                }
            }
            return Log.GetLog();
        }

        /// <summary>
        /// submit closing form.
        /// </summary>
        private static void SubmitClosingform()
        {
            if (WaitForElement(By.Id("AuthenReason"), true))
            {
                WriteInElement(By.Id("AuthenReason"), Constants.NewIncident);
                try
                {
                    ClickElement(By.Id("setjournaleditor"));
                    IWebElement iframe = Constants.driver.FindElement(By.XPath("//iframe"));
                    Constants.driver.SwitchTo().Frame(iframe);
                    WriteInElement(By.XPath("//body"), Constants.NewIncident);
                    Constants.driver.SwitchTo().DefaultContent();
                }
                catch (Exception)
                {
                    // TODO log error.
                }

                WriteInElement(By.Id("AuthenPwd"), password);
                ClickElement(By.Id("InciCancel"));
            }

            LogAlert("Incident deactivation");
        }

        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Edit incident";
            CheckUrl(Constants.BaseUrl + SetupPagePath);

            if (TableSearch(By.XPath("//input[contains(@type,'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(text(),'Action')]"));
                ClickElement(By.LinkText("Edit"));

                //Add message

                ClickElement(By.LinkText("Additional Messages"));

                WaitForElement(By.Id("addInciAction"), true);
                ClickElement(By.Id("addInciAction"));

                WaitForElement(By.Id("ActionDescription"), false);
                WriteInElement(By.Id("Title"), Constants.NewIncident);
                WriteInElement(By.Id("ActionDescription"), Constants.NewIncident);
                ClickElement(By.Id("btnSubmitAct"));
                //ClickElement(By.ClassName("confirm"));

                //Add Media Asset

                ClickElement(By.LinkText("Media Assets"));

                WaitForElement(By.Id("selectInciAsset"), true);
                ClickElement(By.Id("selectInciAsset"));
                WaitForElement(By.Id("tblAsset_wrapper"), true);
                while (IsElementPresent(By.Id("btnSubmitAst"), false))
                {
                    ClickElement(By.CssSelector("td.astitle.sorting_1"));

                    if (IsElementPresent(By.ClassName("selectedAsset"), false))
                    {
                        ClickElement(By.Id("btnSubmitAst"));
                        break;
                    }
                }

                WaitForElement(By.Id("selectInciAsset"), true);

                ClickElement(By.LinkText("Incident Details"));
                WaitForElement(By.Id("btn_submit"), true);
                ClickElement(By.Id("btn_submit"));
                //WaitForElement(By.ClassName("sweet-overlay"), false);
            }

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            CheckUrl(Constants.BaseUrl + SetupPagePath);
            string testName = "delete task 2";
            //
            // 1. delete tasks
            //

            if (TableSearch(By.XPath("//input[contains(@type,'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(text(), 'Action')]"));
                ClickElement(By.LinkText("Incident Tasks"));
                //WaitForElement(By.Id("incident_task_tab"));
                //ClickElement(By.Id("incident_task_tab"));
                WaitForElement(By.Id("grid-task-list"), true);
                ScrollToTop();
                ClickElement(By.CssSelector("#incident_task_list > tbody > tr.even > td.actionCol"));
                ClickElement(By.LinkText("Delete"));
                //WaitForElement(By.Id("taskdel1"));
                //ClickElement(By.Id("taskdel1"));
                ShortWait();
                if (!ClosePopUp("Task Delete Alert"))
                {
                    Log.Entry(Log.Fail, testName, "No confirmation popup detected");
                }
                WaitForElement(By.Id("grid-task-list"), true);
                ClickElement(By.CssSelector("#incident_task_list > tbody > tr.odd > td.actionCol"));
                ClickElement(By.LinkText("Delete"));
                //WaitForElement(By.Id("taskdel0"));
                //ClickElement(By.Id("taskdel0"));
                testName = "delete task 1";
                ShortWait();
                if (!ClosePopUp("Task Delete Alert"))
                {
                    Log.Entry(Log.Fail, testName, "No confirmation popup detected");
                }

                // TODO add some check that all the tasks have been deleted.
                // maybe check that the table is empty, and if not count the remaining tasks.
                // then log the success / error.
            }

            //
            // 2. Delete incident
            //
            testName = "Incident deletion";
            CheckUrl(Constants.BaseUrl + SetupPagePath);

            if (TableSearch(By.XPath("//input[contains(@type,'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");
            }

            //ShortWait();
            LogAlert(testName);

            return Log.GetLog();
        }


        /// <summary>
        /// Go to the incident page, filter incidents and click on menu item with the specific text.
        /// </summary>
        /// <param name="menuText"></param>
        private static void SelectMenuItem(string menuText)
        {
            CheckUrl(Constants.BaseUrl + ActivePagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewIncident, By.XPath("//*[@id='launchedIncidentTable']/tbody/tr[1]/td[2]")))
            {
                do
                {
                    ShortWait();
                    ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                }
                while (!IsElementDisplayed(By.XPath(string.Format("//*[contains(text(),'{0}')]", menuText)), false));

                ClickElement(By.LinkText(menuText));
            }
        }

        /// <summary>
        /// Go to the incident page, filter incidents and click on menu item with the specific text.
        /// </summary>
        /// <param name="menuText"></param>
        /// <param name="conditionToFind"></param>
        private static bool SelectMenuItem(string menuText, By conditionToFind)
        {
            CheckUrl(Constants.BaseUrl + ActivePagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewIncident, By.XPath("//*[@id='launchedIncidentTable']/tbody/tr[1]/td[2]")))
            {
                ShortWait();
                ClickElement(By.XPath("//button[contains(text(),'Action')]"));
                if (IsElementClickable(By.LinkText(menuText)))
                {
                    ClickElement(By.LinkText(menuText));
                    if (WaitForElement(conditionToFind, false))
                    {
                        return true;
                    }
                }
                //int x = Constants.Loop;
                //do
                //{
                //    x--;

                //} while (!WaitForElement(conditionToFind, false) && x >= 0);
            }
            return false;
        }

    }
}
