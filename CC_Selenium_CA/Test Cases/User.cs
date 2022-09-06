using Selenium_CC_CA.Initialisers;
using OpenQA.Selenium;
using System.Collections.Generic;
using System;

namespace Selenium_CC_CA.Initialisers
{

    public class User : Constants
    {
        private const string PagePath = "/users";
        private const string ImportPath = "/dataimport/user";
        private const string BulkPath = "/users/bulkadd";
        public static List<LogItem> Create()
        {
            Log.Clear();
            string testname = "Create new user";
            CheckUrl(BaseUrl + PagePath);

            // Check if the test user exists
            if (!TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstName + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
            {
                //// this search is redondant as the original name is part of the edited name. It might be required if the edited name changes significantly.
                //if (!TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstNameAfterEdit + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
                //{
                // if the user does not exist, create it
                ClickElement(By.XPath("//button[contains(text(),'Add User')]"));
                ClickElement(By.XPath("//a[contains(@href,'" + Constants.BaseUrl + "/users/create')]"));
                WaitForElement(By.Id("FirstName"), true);

                if (Constants.driver.GetType().Name.ToLower().Contains("firefox")) // firefox only code -- not working yet issues with firefox clicking on certain items.
                {
                    MoveToElement(By.ClassName("select2-search--inline"));
                    ClickElement(By.ClassName("select2-search--inline"));
                    WriteInElement(By.ClassName("select2-search__field"), Constants.NewLocation + Keys.Enter);
                }
                else // all other browsers
                {
                    MoveToElement(By.XPath("(//input[@type='search'])[1]"));
                    ClickElement(By.XPath("(//input[@type='search'])[1]"));
                    WriteInElement(By.XPath("(//input[@type='search'])[1]"), Constants.NewLocation + Keys.Enter);
                }
                ClickElement(By.XPath("(//input[@type='search'])[2]"));
                WriteInElement(By.XPath("(//input[@type='search'])[2]"), Constants.NewDepartmentAfterEdit + Keys.Enter);

                MoveToElement(By.Id("FirstName"));
                WriteInElement(By.Id("FirstName"), Constants.NewUserFirstName);
                WriteInElement(By.Id("CCUN"), Constants.NewUserEmail);
                WriteInElement(By.Id("LastName"), Constants.NewUserLastName);
                WriteInElement(By.Id("Mobile"), Constants.NewUserMobile);
                ClickElement(By.XPath("//div[@id='inviteCheck']//div//label"));
                ClickElement(By.Id("select2-SecurityGroup-container"));
                ClickElement(By.ClassName("select2-results__option--highlighted"));
                
                // Department is required for User Creation depending on data segregation config
                ClickElement(By.Id("select2-Department-container"));
                WriteInElement(By.XPath("//*[@id='users_create']/span/span/span[1]/input"), Constants.NewDeptAfterEdit + Keys.Enter);
                
                ClickElement(By.Id("btn_submit"));

                //CheckForLicenseWarning();
                LogAlert(testname);
                Console.WriteLine("Test: " + testname + " | Result: PASS " + " | Outcome: User Created");


            }
            else
            {
                Log.Entry(Log.Fail, testname, "User already exists.");
                Console.WriteLine("Test: " + testname + " | Result: FAIL " + " | Outcome: User Already exists.");
            }
            return Log.GetLog();

        }

        public static void CheckForLicenseWarning()
        {
            while (FindAlert_SA()[0] == "License Warning")
            {
                ShortWait();
                ClosePopUp("License Warning");      
            }
        }


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testname = "Edit existing user";
            CheckUrl(BaseUrl + PagePath);

            // check if the user has already been edited
            if (!TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstNameAfterEdit + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
            {
                // look for user to edit
                if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstName + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
                {
                    ClickElement(By.CssSelector("td.multiselect.text-left"));
                    ClickElement(By.XPath("//button[contains(@class, 'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                    ClickElement(By.LinkText("Edit"));

                    WriteInElement(By.Id("FirstName"), Constants.NewUserFirstNameAfterEdit);
                    ClickElement(By.Id("btn_submit"));

                    while (FindAlert()[0] == "License Warning")
                    {
                        ClosePopUp("License Warning");
                        ShortWait();
                    }

                    LogAlert(testname);
                    Console.WriteLine("Test: " + testname + " | Result: PASS " + " | Outcome: User has been updated.");
                }
            }
            else
            {
                Log.Entry(Log.Fail, testname, "User already edited.");
                Console.WriteLine("Test: " + testname + " | Result: FAIL " + " | Outcome: User already edited.");
            }

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testname = "Delete user";
            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstNameAfterEdit + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.CssSelector("td.multiselect.text-left"));
                ClickElement(By.XPath("//button[contains(@class, 'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                if (ClosePopUp("Delete Confirmation"))
                {
                    LogAlert(testname);
                    Console.WriteLine("Test: " + testname + " | Result: PASS " + " | Outcome: User Deleted.");
                }
                else
                {
                    Log.Entry(Log.Fail, testname, "Confiramtion pop up did not show.");
                    Console.WriteLine("Test: " + testname + " | Result: FAIL " + " | Outcome: Confiramtion pop up did not show.");
                }
            }
            else
            {
                Log.Entry(Log.Fail, testname, "User does not exist.");
                Console.WriteLine("Test: " + testname + " | Result: FAIL " + " | Outcome: User does not exist.");

            }

            return Log.GetLog();

        }

        public static string[] Import()
        {
            CheckUrl(BaseUrl + PagePath);

            //ClickElement(By.XPath("//button[contains(@class,'btn btn-success dropdown-toggle')]"));
            ClickElement(By.XPath("//button[contains(text(),'Add User')]"));
            ClickElement(By.XPath("//a[contains(@href,'" + BaseUrl + ImportPath + "')]"));
            ClickElement(By.Id("import_dropzone"));
            WaitForElement(By.Id("next"), true);
            if (IsElementDisplayed(By.Id("next"),true))
            {
                ClickElement(By.Id("next"));
                WaitForElement(By.Id("btn_start_import"), true);
                if (IsElementDisplayed(By.Id("btn_start_import"),true))
                {
                    if (FindAlert()[0] == "License Warning")
                    {
                        ClosePopUp("License Warning");
                    }

                    ClickElement(By.Id("btn_start_import"));

                    if (ClosePopUp("Import Confirmation"))
                    {
                        ClickElement(By.XPath("//button[contains(@class,'confirm')]"));

                        return FindAlert();
                    }


                }

            }

            return ErrorMsg; ;
        }

        public static string[] DeleteImportedUser()
        {
            CheckUrl(BaseUrl + PagePath);

            ClickElement(By.XPath("//button[contains(@class,'btn btn-success dropdown-toggle')]"));
            ClickElement(By.XPath("//a[contains(@href,'" + BaseUrl + "/dataimport/user')]"));
            ClickElement(By.Id("import_dropzone"));
            if (IsElementDisplayed(By.Id("next"),true))
            {
                ClickElement(By.Id("next"));
                if (IsElementDisplayed(By.Id("btn_start_import"),true))
                {
                    ClosePopUp("License Warning");

                    ClickElement(By.Id("btn_start_import"));


                    if (ClosePopUp("Import Confirmation"))
                    {
                        ClickElement(By.XPath("//button[contains(@class,'confirm')]"));

                        return FindAlert();

                    }


                }

            }

            return ErrorMsg; ;
        }

        public static List<LogItem> bulkAdd()
        {
            Log.Clear();
            string testName = "Bulk add users";
            CheckUrl(BaseUrl + PagePath);

            ClickElement(By.XPath("//button[contains(text(),'Add User')]"));
            //ClickElement(By.XPath("//button[contains(@class,'btn btn-success dropdown-toggle')]"));
            ClickElement(By.XPath("//a[contains(@href,'" + Constants.BaseUrl + BulkPath + "')]"));
            WaitForElement(By.Id("select2-SecurityGroup-container"), true);

            SelectFromDropDown("select2-UserRole-container", "select2-UserRole-results");

            ShortWait();
            SelectFromDropDown("select2-SecurityGroup-container", "select2-SecurityGroup-results");
            //WaitForElement(By.XPath("//*[@id='select2-SecurityGroup-results']/li[contains(text(),'Basic User')]"), false);
            //ClickElement(By.XPath("//*[@id='select2-SecurityGroup-results']/li[contains(text(),'Basic User')]"));
            //ClickElement(By.Id("user_option"));

            //ClickElement(By.Id("select2-UserRole-container"));
            //WaitForElement(By.XPath("//*[@id='select2-UserRole-results']/li[contains(text(),'Staff')]"), true);
            //ClickElement(By.XPath("//*[@id='select2-UserRole-results']/li[contains(text(),'Staff')]"));

            ClickElement(By.Id("select2-UserLanguage-container"));
            WaitForElement(By.XPath("//*[@id='select2-UserLanguage-results']/li[contains(text(),'English')]"), false);
            ClickElement(By.XPath("//*[@id='select2-UserLanguage-results']/li[contains(text(),'English')]"));

            ClickElement(By.XPath("//div[contains(@class,'radio radio-primary')]/label[contains(text(),'No')]"));

            // Department is now required for User Creation
            ClickElement(By.XPath("//*[@id='bulkuseroptions']/div/div[1]/div[4]/div[2]/span/span[1]/span"));
            WriteInElement(By.XPath("//*[@id='users_bulkadd']/span/span/span[1]/input"), Constants.NewDeptAfterEdit + Keys.Enter);

            ClickElement(By.XPath("(//input[@type='search'])[2]"));
            WriteInElement(By.XPath("(//input[@type='search'])[2]"), Constants.NewDepartmentAfterEdit + Keys.Enter);

            ClickElement(By.XPath("//input[@type='search']"));
            WriteInElement(By.XPath("//input[@type='search']"), Constants.NewLocation + Keys.Enter);

            // Timezone ins now required for User Creation
            ClickElement(By.XPath("//*[@id='bulkuseroptions']/div/div[3]/div[1]/div[2]/span/span[1]/span"));
            WriteInElement(By.XPath("//*[@id='users_bulkadd']/span/span/span[1]/input"), Constants.TimeZone + Keys.Enter);

            MoveToElement(By.Id("btn_submit"));
            ClickElement(By.Id("btn_submit"));
            WaitForElement(By.Id("bulk_user_table_wrapper"), true);

            WriteInElement(By.Id("FirstName"), Constants.NewUserFirstName);
            WriteInElement(By.Id("LastName"), Constants.NewUserLastName);
            WriteInElement(By.Id("Email"), Constants.NewUserEmail);
            WriteInElement(By.Id("Mobile"), Constants.NewUserMobile);

            ClickElement(By.CssSelector("th.actionCol > #btn_submit"));
            //ClickElement(By.XPath("//*[contains(text(),'Save')]"));

            if (FindAlert()[0] == "License Warning")
            {
                ClosePopUp("License Warning");
                ShortWait();
            }

            if (TableSearch(By.XPath("//div[@id='bulk_user_table_filter']/label/input"), NewUserEmail, By.XPath("//table[@id='bulk_user_table']/tbody/tr/td[3]")))
            {
                Log.Entry(Log.Pass, testName, "User successfully created.");
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: User successfully created.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "User not created.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: User not created.");
            }

            return Log.GetLog();
        }

        /// <summary>
        /// select staff / standard in the list of otpions for this dropdown.
        /// </summary>
        /// <param name="inputBoxId"></param>
        /// <param name="resultUlId"></param>
        private static void SelectFromDropDown(string inputBoxId, string resultUlId)
        {
            ClickElement(By.Id(inputBoxId));
            WaitForElement(By.Id(resultUlId), false);
            var selection = Constants.driver.FindElements(By.XPath($"//*[@id='{resultUlId}']/li"));
            foreach (IWebElement item in selection)
            {
                if (item.Text.ToLower().Contains("basic") || item.Text.ToLower().Contains("standard") || item.Text.ToLower().Contains("staff"))
                {
                    item.Click();
                    break;
                }
            }
        }

        public static List<LogItem> Delete_bulkAdd()
        {
            Log.Clear();
            string testName = "Delete bulk added user";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + PagePath);
            WaitForElement(By.Id("tblUsers"), true);

            if (TableSearch(By.XPath("//input[contains(@class, 'form-control input-inline')]"), Constants.NewUserFirstName + Constants.NewUserLastName, By.XPath("//*[@id='tblUsers']/tbody/tr[1]/td[2]")))
            {
                ClickElement(By.CssSelector("td.multiselect.text-left"));
                ClickElement(By.XPath("//button[contains(@class, 'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                if (ClosePopUp("Delete Confirmation"))
                {
                    LogAlert(testName);
                    Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Bulk Add User successfully created.");
                }
                else
                {
                    Log.Entry(Log.Fail, testName, "No delete confirmation.");
                    Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: No delete confirmation.");
                    LogAlert(testName);
                }
            }
            else
            {
                Log.Entry(Log.Fail, testName, "User does not exist.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: User does not exist.");
            }

            return Log.GetLog();

        }
    }
}

