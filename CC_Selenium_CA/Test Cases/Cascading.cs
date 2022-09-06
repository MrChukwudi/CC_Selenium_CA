using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;

namespace CC_Selenium_CA.Test_Cases
{
    class Cascading : Constants
    {
        private static readonly string PagePath = "/company/configuration";
        private const string AutoCascading = "Auto Cascading";

        public static void Create()
        {
            string testName = "Cascading Plan Creation";
            //Log.AddTestStart(testName);

            SetUp();

            //Checking if an automaticaally created cascading plan is present
            if (!TableSearch(By.XPath("//*[@id='tbl_cascade_filter']/label/input"), AutoCascading, By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[1]")))
            {
                //Creating a new cascading plan
                ClickElement(By.XPath("//*[@id='btn_new_cascade_plan']"));

                WaitForElement(By.XPath("//*[@id='frmcascading']/div[1]/div[1]/div[1]/label"), true);

                // Writing plan name
                WriteInElement(By.Id("PlanName"), AutoCascading);

                //Saving the Cascading plan
                ClickElement(By.Id("btn_cascade_submit"));

                LogAlert("Cascading Plan");
            }

        }

        public static void Edit()
        {
            SetUp();

            if (TableSearch(By.XPath("//*[@id='tbl_cascade_filter']/label/input"), AutoCascading, By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[3]/div/button"));
                //Clicking the Edit Option
                ClickElement(By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[3]/div/div/a[1]"));

                //Removing step 3
                ClickElement(By.Id("StepRemove3"));

                //Saving
                ClickElement(By.Id("btn_cascade_submit"));

                LogAlert("Cascading Plan");
            }
        }

        public static void Delete()
        {
            SetUp();

            if (TableSearch(By.XPath("//*[@id='tbl_cascade_filter']/label/input"), AutoCascading, By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[3]/div/button"));
                //Clicking the Delete Option
                ClickElement(By.XPath("//*[@id='tbl_cascade']/tbody/tr[1]/td[3]/div/div/a[2]"));

                WaitForElement(By.XPath("//*[@id='company_configuration']/div[7]"), true);
                ClickElement(By.XPath("//*[@id='company_configuration']/div[7]/div[7]/div/button"));

                LogAlert("Deleting Cascading Plan");
            }
        }

        public static void SetUp()
        {
            //CheckUrl(Constant.BaseUrl + PagePath);
            WaitForElement(By.Id("settings_tabs"), true);

            ClickElement(By.LinkText("Channel Cascading"));
            WaitForElement(By.XPath("//*[@id='container']/div[2]/div[2]/div"), true);
        }
    }
}
