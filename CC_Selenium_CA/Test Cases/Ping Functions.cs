using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    class Ping_Functions : Constants
    {
        internal static void Login(string Username, string Password)
        {
            ShortWait();
            Functions.CPCheckUrl(Constants.BaseUrl + Constants.LoginPage);

            //WriteInElement(By.Id("CustomerId"), companyID); //Company ID no longer required 
            WriteInElement(By.Id("Primary_Email"), Username);

            //Adding section after inserting the email
            ClickElement(By.Id("btn_login_next"));
            ShortWait();

            WaitForElement(By.Id("password"), true);

            if (Constants.driver.GetType().Name.ToLower().Contains("edge"))
            {
                // if not for this, Edge might try to add a saved password at the same time as entering the password which causes problems.
                ClickElement(By.Id("password"));
            }
            WriteInElement(By.Id("password"), Password);

            ClickElement(By.Name("btn_reg_submit"));
        }

        internal static void LogOut()
        {
            ShortWait();
            WaitForElement(By.Id("ccHeaderMenu"), true);
            ClickElement(By.XPath("//*[@id='ccHeaderMenu']/ul/li[1]/a"));

            ShortWait();
            WaitForElement(By.Id("dlgoffduty"), true);
            ClickElement(By.XPath("//*[@id='ccHeaderMenu']/ul/li[1]/div/a[5]")); //*[@id="ccHeaderMenu"]/ul/li[1]/div/a[5]
        }

        internal static void CloseCookieBR()
        {
            if (WaitForElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"), false)) //"#dashboard_index > div.alert.alert-secondary.text-center.cookiealert.show > button"
            {
                ClickElement(By.CssSelector("div.alert.alert-secondary.text-center.cookiealert.show > button"));
            }
        }

    }
}
