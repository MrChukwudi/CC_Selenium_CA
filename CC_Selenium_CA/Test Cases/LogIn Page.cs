using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    public class LogIn_Page : Constants
    {
        public const string PagePath = "/users/login";
        public const string LogInDesc = "Login";
        public const string LogOutDesc = "Log Out";
        public static List<LogItem> LogIn()
        {
            Log.Clear();
            CheckUrl(Constants.BaseUrl + PagePath);
            string testName = "Login Test";
            Console.WriteLine(testName);
            //
            // Closing Cookies Pop-Up 
            //

            if (WaitForElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"), false)) //"#dashboard_index > div.alert.alert-secondary.text-center.cookiealert.show > button"
            {
                ClickElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"));
            }

            //
            // Actual login
            //

            string subTest = "Valid User Login Test";
            while (Constants.driver.Url == Constants.BaseUrl + PagePath)
            {
                if (Constants.IsDebug) // I don't want to enter my email and password every time I run this code...
                {
                    Constants.email = Constants.seleniumUserEmail;
                    Constants.password = Constants.seleniumPwd;
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

                    userName = Constants.driver.FindElement(By.XPath("//*[@id='headerNavbarCollapse']/ul/li[1]/a")); // Lib portal doesn't have the role= btn thing 
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
            Console.WriteLine("Test: " + subTest + " \t Result: PASS " + " \t Outcome: User logged in." + "\t URL: " + Constants.driver.Url);

            return Log.GetLog();
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

        public static List<LogItem> LogOut()
        {
            Log.Clear();
            string startName = "\nLog Out Test";
            string testName = "Log Out Test";
            Console.WriteLine(startName);

            ShortWait();
            WaitForElement(By.Id("offdutysts"), true);
            ClickElement(By.XPath("//*[@id='headerNavbarCollapse']/ul/li[1]/a")); 

            ShortWait();
            WaitForElement(By.Id("dlgoffduty"), true);
            ClickElement(By.XPath("//*[@id='headerNavbarCollapse']/ul/li[1]/div/a[5]")); 

            if(driver.Url == Constants.BaseUrl + LoginPage)
            {
                Log.Entry(Log.Pass, LogOutDesc, "User logged out successfully.");
                Console.WriteLine("Test: " + testName + " \t Result: PASS " + " \t Outcome: User logged out." + "\t URL: " + Constants.driver.Url + "\n");
            }
            else
            {
                Log.Entry(Log.Fail, LogOutDesc, "Failed to log out user.");
                Console.WriteLine("Test: " + testName + " \t Result: FAIL " + " \t Outcome: Failed to log out user." + "\t URL: " + Constants.driver.Url + "\n");
            }

            return Log.GetLog();
        }
    }
}
