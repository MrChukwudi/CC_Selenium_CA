using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    class Off_Duty : Constants
    {
        private static readonly string PagePath = "/dashboard";
        /// <summary>
        /// 
        /// </summary>
        public static void Activate()
        {
            //Log.AddTestStart("Off Duty Start");
            CheckUrl(Constants.BaseUrl + PagePath);
            //Making sure that the test taskes place on the dashboard

            //string subTest = "Enabling Off Duty Test";

            SetUp();

            //Clicking on the start time Element for Off Duty
            ClickElement(By.XPath("//*[@id='OffDutyStartTime']"));

            //Waiting for element to be visible before clicking the increment time option
            WaitForElement(By.XPath("//*[@id='dashboard_index']/div[8]/table/tbody/tr[1]/td[1]/a"), true);
            ClickElement(By.XPath("//*[@id='dashboard_index']/div[8]/table/tbody/tr[1]/td[1]/a"));

            //Waiting for element or else it tries to do this too fast
            WaitForElement(By.Id("btn_submit"), true);

            //Submits the changes
            ClickElement(By.Id("btn_submit"));

            //Waiting for Successful pop up message - before clicking on the OK btn
            WaitForElement(By.ClassName("sa-confirm-button-container"), true);
            ClickElement(By.XPath("//*[@id='dashboard_index']/div[7]/div[7]/div/button"));

            //Waiting for element or else it tries to do this too fast
            WaitForElement(By.Id("ccHeaderMenu"), true);

            SetUp();
            // Runs a short wait before repeating the setup again to start validation for the test.

            //Waits for the container to appear before continuing with the test
            WaitForElement(By.Id("dlg_Offcall"), true);

            //Checks for the delete btn - if present off duty is active - if not off duty not activated - log result accordinly
            if (IsElementDisplayed(By.Id("btn_delete_offduty"), true))
            {
                //Log.AddPass(subTest, "Off Duty Activated");
            }
            else
            {
                //Log.AddFail(subTest, "Off Duty not Activated");
            }
        }

        public static void Deactivate()
        {
            CheckUrl(Constants.BaseUrl + PagePath);

            //Log.AddTestStart("Off Duty Deactivate");
            string subTest = "Deactivating Off Duty";

            //Runs the setup again for the deactivation test 
            SetUp();

            WaitForElement(By.Id("btn_delete_offduty"), true);
            //ClickElement(By.Id("btn_delete_offduty"));

            if (IsElementDisplayed(By.Id("btn_delete_offduty"), true))
            {
                ClickElement(By.Id("btn_delete_offduty"));

                ShortWait();

                //Waiting for Successful pop up message - before clicking on the OK btn
                WaitForElement(By.ClassName("sa-confirm-button-container"), true);
                ClickElement(By.XPath("//*[@id='dashboard_index']/div[7]/div[7]/div/button"));

                ShortWait();

                SetUp();
                // Checking if Off Duty has been deactivated - if object cannot be found log test pass if can be found log error
                // If delete btn is present off duty is still active - log fail | if it cannot be found log pass
                if (IsElementDisplayed(By.Id("btn_delete_offduty"), true))
                {
                    //Log.AddFail(subTest, "Off Duty not Dectivated");
                }
                else
                {
                    //Log.AddPass(subTest, "Off Duty Deactivated");
                }
            }
            else
            {
                //Log.AddFail(subTest, "Off Duty Not Enabled.");
            }
        }
        //reusing code snippet
        public static void SetUp()
        {
            ClickElement(By.Id("ccHeaderMenu"));//'//*[@id="ccHeaderMenu"]/ul/li[1]/a'

            ClickElement(By.Id("dlgoffduty"));//'//*[@id="dlgoffduty"]'
        }

    }
}
