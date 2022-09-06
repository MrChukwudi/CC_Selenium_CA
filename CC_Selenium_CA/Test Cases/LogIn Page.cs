using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    public class LogIn_Page : Constants
    {
        public const string PagePath = "/users/login";
        public const string LogInDesc = "Login";
        public static List<LogItem> LogIn()
        {
            Log.Clear();
            CheckUrl(Constants.BaseUrl + PagePath);

            //
            // Closing Cookies Pop-Up 
            //

            if (WaitForElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"), false)) //"#dashboard_index > div.alert.alert-secondary.text-center.cookiealert.show > button"
            {
                ClickElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"));
            }


            //
            // 1. check non existing user
            //
            Console.WriteLine("Login Tests:");
            string subTest = "Test non existing credentials";
            bool success = false;
            TryFirstLogin("test@test.com", "pass");

            WaitForElement(By.Id("toast-container"), false);
            if (IsElementPresent(By.XPath("//*[contains(text(),'User does not exist in Crises Control')]"), true))
            {
                Log.Entry(Log.Pass, subTest, "Error message shown");
                Console.WriteLine("Test: " + subTest + " | Result: PASS " + " | Outcome: Error message shown");
                success = true;
            }

            if (driver.Url != Constants.BaseUrl + PagePath)
            {
                if (ClickElement(By.ClassName("glyphicon-off")))
                {
                    Log.Entry(Log.Fail, subTest, "Logged in");
                    Console.WriteLine("Test: " + subTest + " | Result: Fail " + " | Outcome: Logged In");
                }
                else
                {
                    Log.Entry(Log.Fail, subTest, "Redirection to " + driver.Url);
                    CheckUrl(Constants.BaseUrl + PagePath);
                }
            }
            else
            {
                if (!success)
                {
                    Log.Entry(Log.Fail, subTest, "No error shown");
                    Console.WriteLine("Test: " + subTest + " | Result: FAIL" + " | No error shown."); 
                }
            }

            //
            // 2. Check input validation (non conform email address)
            //

            subTest = "Check invalid email address";
            TryFirstLogin("testtest.com", "pass");

            if (!IsElementDisplayed(By.Id("Primary_Email-error"),true) /*Displayed.XPath(driver, "//div[contains(@class,'tooltip fade top in')]")*/)
            {
                Log.Entry(Log.Fail, subTest, "Error Message not shown");
                Console.WriteLine("Test: " + subTest + " | Result: Fail " + " | Outcome: Error Message not shown.");
            }
            else
            {
                Log.Entry(Log.Pass, subTest, "Error Message Shown");
                Console.WriteLine("Test: " + subTest + " | Result: PASS " + " | Outcome: Error Message Shown.");
            }

            //
            // 3. Check actual login - User being used for the testing 
            //

            subTest = "Valid User Login Test";
            while (Constants.driver.Url == Constants.BaseUrl + PagePath)
            {
                if (Constants.IsDebug) // I don't want to enter my email and password every time I run this code...
                {
                    Constants.email = "juan-pierre.roussow@transputec.com";
                    Constants.password = "Welcome$123";
                }
                // get the user login details.
                else
                {
                    Console.WriteLine("Please Enter Your Email");
                    Constants.email = Console.ReadLine();

                    Console.WriteLine("Please Enter Your Password");
                    GetConsoleSecurePassword();
                    Console.WriteLine("\r******************************\n");
                }

                // edge needs special treatment.
                TryLogin(Constants.email, Constants.password);
                if (driver.GetType().Name.ToLower().Contains("edge"))
                {
                    // Edge can find non-existing alerts...
                    ShortWait();
                }

                // if the entered credentials are wrong, catch error.
                if (FindAlert()[0] == Constants.ErrorMsg[0])
                {
                    //ClickElement(By.XPath("//button[contains(@class,'confirm')]"));
                    driver.FindElement(By.ClassName("confirm")).Click();
                    Console.WriteLine("Invalid ID or password.");
                    Console.WriteLine("");
                }
                else if (IsElementDisplayed(By.CssSelector("div[role = 'tooltip']"),true) /*Displayed.XPath(driver, "//div[contains(@class,'tooltip fade top in')]")*/)
                {
                    Console.WriteLine("Invalid login id or password.");
                    Console.WriteLine("");
                }

            }

            // get the display name of the current user for use throughout the tests.
            IWebElement userName = driver.FindElement(By.CssSelector("div.user-info > div.username > a"));
            if (userName != null)
            {
                // if the element is not visible, Text will be empty.
                if (string.IsNullOrWhiteSpace(userName.Text))
                {
                    // in that case, look somwhere else for the name.
                    userName = null;
                    userName = driver.FindElement(By.CssSelector("a[role = 'button']"));
                    if (userName != null)
                    {
                        // store the user name.
                        UserNumber1 = userName.Text.TrimStart().TrimEnd();
                    }
                }
                // store the user name
                else
                {
                    UserNumber1 = userName.Text.TrimStart().TrimEnd();
                }
            }
            // log successful login.
            Log.Entry(Log.Pass, LogInDesc, "User logged in");
            Console.WriteLine("Test: " + subTest + " | Result: PASS " + " | Outcome: User logged in.");

            if (!Constants.IsDebug)
            {
                // offer the possibility to enter a secondary user for use throughout the tests.
                Console.WriteLine("\n\nPress F1 to edit the secondary user. This user will not be modified.\nPress any other key to continue. ");
                if (Console.ReadKey().Key == ConsoleKey.F1)
                {
                    Console.WriteLine("\n\nEnter the full name of the secondary user: ");
                    Constants.UserNumber2 = Console.ReadLine();
                }
                Console.WriteLine("\n\n");
            }

            return Log.GetLog();
        }

        /// <summary>
        /// Inputs email ID and checks if there is an error - email does not exist 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static void TryFirstLogin(string email, string password)
        {
            //WriteInElement(By.Id("CustomerId"), companyID); //Company ID no longer required 
            WriteInElement(By.Id("Primary_Email"), email);

            //Adding section after inserting the email
            ClickElement(By.Id("btn_login_next"));
        }

        /// <summary>
        /// Inputs user id, password and clicks login.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static void TryLogin(string email, string password)
        {
            WriteInElement(By.Id("Primary_Email"), email);

            //Adding section after inserting the email
            ClickElement(By.Id("btn_login_next"));
            WaitForElement(By.Id("password"), true);

            //SendKeys.Id("Primary_Email", email);
            if (driver.GetType().Name.ToLower().Contains("edge"))
            {
                // if not for this, Edge might try to add a saved password at the same time as entering the password which causes problems.
                ClickElement(By.Id("password"));
            }
            WriteInElement(By.Id("password"), password);
            //SendKeys.Id("password", password);

            ClickElement(By.Name("btn_reg_submit"));

        }

        public static string[] Password_Reset()
        {
            CheckUrl(BaseUrl + "/dashboard");

            ClickElement(By.Id("user-options"));
            ClickElement(By.XPath("//*[contains(text(),'Logout')]"));
            WaitForElement(By.Id("btn_forgot_pass"), true);

            ClickElement(By.Id("btn_forgot_pass"));


            while (FindAlert()[0] != Constants.SuccessMsg[0])
            {
                if (FindAlert()[0] == Constants.ErrorMsg[0])
                {
                    Console.WriteLine("Incorrect captcha code entered!.");
                    Console.WriteLine("");

                    ClickElement(By.XPath("//button[contains(@class,'confirm')]"));
                }

                WriteInElement(By.XPath("//*[@id='Email']"), email);

                Console.WriteLine("Type the captcha characters in the picture here.");
                string Captcha = Console.ReadLine();
                WriteInElement(By.Id("Captcha"), Captcha);

                ClickElement(By.XPath("//*[@id='btn_continue']"));
            }

            return SuccessMsg;
        }
    }
}
