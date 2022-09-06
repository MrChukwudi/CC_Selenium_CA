using OpenQA.Selenium;
using Selenium_CC_CA.Initialisers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC_Selenium_CA.Test_Cases
{
    public class Email_Template : Constants
    {
        private static readonly string PagePath = "/emailtemplate/view/";
        private const string EmailTemp = "Launch Incident";
        private const string EmailText = "Test Text Input ";

        public static void Modify()
        {
            //Log.AddTestStart("Email Template Edit");
            CheckUrl(Constants.BaseUrl + PagePath);

            string subTest = "Email Template Testing Edit:";

            WaitForElement(By.Id("tbl_tmpl_wrapper"), true);
            //Editing the Email Template
            //Seatching for the desired Email Template to Edit - Avoiding editing the wrong template
            if (!TableSearch(By.XPath("//*[@id='tbl_tmpl_filter']/label/input"), EmailTemp, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//*[@id='tbl_tmpl']/tbody/tr/td[3]/a"));

                //Waiting for the iFrame to be visible before interacting with it
                WaitForElement(By.ClassName("k-content"), true);

                ClickElement(By.XPath("//iframe[contains(@class,'k-content')]"));
                //Clicking on the iFrame and Inputing the desired text into the template
                try
                {
                    Constants.driver.SwitchTo().Frame(Constants.driver.FindElement(By.XPath("//iframe[contains(@class,'k-content')]")));
                    WriteInElement(By.XPath("/html/body/table/tbody/tr/td/div[2]/div/div/div/div/div/div[1]/div/div/p[3]"), EmailText);
                }
                catch (Exception e)
                {
                    //Log.AddError("Add description", e.Message);
                }
                //Swaping back to the parent frame to continue the test
                Constants.driver.SwitchTo().DefaultContent();

                ShortWait();

                //Clicking the submit btn at the bottom of the page
                ClickElement(By.Id("btn_submit"));
                MoveToElement(By.ClassName("card-footer"));

                ShortWait();
                WaitForElement(By.XPath("//*[@id='emailtemplate_edittmpl']/div[12]"), true);
                ClickElement(By.XPath("//*[@id='emailtemplate_edittmpl']/div[12]/div[7]/div/button"));

                //Waiting for the element to be visible before moving on to validation
                WaitForElement(By.Id("tbl_tmpl_wrapper"), true);
            }
            //Validation 1 
            //Searching for the desired Template to Edit - Avoiding editing the wrong template
            WriteInElement(By.XPath("//*[@id='tbl_tmpl_filter']/label/input"), EmailTemp);

            ShortWait();
            // if the image is displayed it is a company copy, if img is not displayed it is a factory original
            if (IsElementDisplayed(By.XPath("//*[@id='tbl_tmpl']/tbody/tr/td[1]/i"), true))
            {
                //Log.AddPass(subTest, "Email Template Modified.");
            }
            else
            {
                //Log.AddFail(subTest, "Email Template Failed");
            }

            //Validation 2 
            if (!TableSearch(By.XPath("//*[@id='tbl_tmpl_filter']/label/input"), EmailTemp, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//*[@id='tbl_tmpl']/tbody/tr/td[3]/a"));

                //Waiting for the iFrame to be visible before interacting with it
                WaitForElement(By.ClassName("k-content"), true);

                //ClickElement(By.XPath("//iframe[contains(@class,'k-content')]"));

                //Clicking on the iFrame and Inputing the desired text into the template
                ShortWait();


                if (IsElementPresent(By.XPath("//*[contains(text(),'Test Text Input')]"), true))
                {
                    //Log.AddPass(subTest, "Email Modification Found");
                }
                else
                {
                    //Log.AddFail(subTest, "Email Modification not found");
                }

            }

        }

        public static void Restore()
        {

            //Log.AddTestStart("Email Template - Restore Original");

            //Log test start + navigates to correct Url
            CheckUrl(Constants.BaseUrl + PagePath);
            string subTest = "Restoring Email Template ";
            //Seatching for the desired Email Template to Edit - Avoiding editing the wrong template
            if (!TableSearch(By.XPath("//*[@id='tbl_tmpl_filter']/label/input"), EmailTemp, By.XPath("//*[@id='tbl_incident_setup_active']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//*[@id='tbl_tmpl']/tbody/tr/td[3]/a"));

                //Navigating to the restore btn to start the process
                ScrollToBottom();
                ClickElement(By.Id("btn_restore"));

                ShortWait();

                //Waiting for the Pop-up to appear for Restoration
                WaitForElement(By.ClassName("sa-confirm-button-container"), true);
                ClickElement(By.ClassName("confirm"));

                ShortWait();

                //Waiting for the Successful Pop-Up to appear
                WaitForElement(By.XPath("//*[@id='emailtemplate_edittmpl']/div[12]"), true);
                ClickElement(By.XPath("//*[@id='emailtemplate_edittmpl']/div[12]/div[7]/div/button"));

                WaitForElement(By.ClassName("k-content"), true);

                //Using Cancel to go back to the email template edit page
                ScrollToBottom();
                //ClickElement(By.XPath("//*[@id='btn_cancel']"));
                ClickElement(By.Id("btn_cancel"));

                WaitForElement(By.Id("tmpl_list"), true);
            }

            //Validation
            WriteInElement(By.XPath("//*[@id='tbl_tmpl_filter']/label/input"), EmailTemp);

            ShortWait();
            // if the image is displayed it is a company copy, if img is not displayed it is a factory original
            if (IsElementDisplayed(By.XPath("//*[@id='tbl_tmpl']/tbody/tr/td[1]/i"), false))
            {
                //Log.AddFail(subTest, "Template Restoration Failed");
            }
            else
            {
                //Log.AddPass(subTest, "Email Template Restored");
            }

        }
    }
}
