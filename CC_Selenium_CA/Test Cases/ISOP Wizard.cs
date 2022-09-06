using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{

    public class ISOP_Wizard : Constants
    {
        private static readonly string PagePath = "/sop/isop";
        //private static readonly string MediaPath = "/mediaasset";


        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "iSOP wizard - create";
            Console.WriteLine(testName);
            CheckUrl(BaseUrl + PagePath);

            WaitForElement(By.LinkText("Add New Document"), true);
            ClickElement(By.LinkText("Add New Document"));
            WaitForElement(By.Id("select2-IncidentID-container"), true);

            ShortWait();

            //
            // 1. isop details
            //

            ClickElement(By.Id("select2-IncidentID-container"));
            WriteInElement(By.CssSelector("span.select2-search.select2-search--dropdown > input.select2-search__field"), Constants.NewIncident);
            ClickElement(By.ClassName("select2-results__option--highlighted"));

            ClickElement(By.Id("select2-SOPOwner-container"));
            WriteInElement(By.CssSelector("span.select2-search.select2-search--dropdown > input.select2-search__field"), Constants.UserNumber1);
            ClickElement(By.ClassName("select2-results__option--highlighted"));
            WriteInElement(By.Id("SOPVersion"), "Version 1");

            ClickElement(By.Id("ReviewDate"));
            WaitForElement(By.ClassName("today"), false);
            ClickElement(By.ClassName("today"));

            ClickElement(By.Id("select2-ReviewFrequency-container"));
            ClickElement(By.XPath("//li[text() [contains(.,'Yearly')]]"));

            WriteInElement(By.Id("BriefDescription"), "Test ISOP");
            ClickElement(By.XPath("//input[contains(@placeholder,'Select sections')]"));
            if (!IsElementPresent(By.XPath("//li[text() [contains(.,'Executive Summary')]]"), false))
            {
                // second click required on firefox
                ClickElement(By.XPath("//input[contains(@placeholder,'Select sections')]"));
            }
            ClickElement(By.XPath("//li[text() [contains(.,'Executive Summary')]]"));

            ClickElement(By.Id("SOPVersion"));

            if (!ClickElement(By.Id("btn_submit")))
            {
                ClickElement(By.Id("user_list"));
                ClickElement(By.Id("btn_submit"));
            }

            //
            // 2. edit isop section
            //

            WaitForElement(By.Id("btn_add_section"), false);
            ClickElement(By.Id("btn_add_section"));
            WaitForElement(By.Id("frm_sop_section"), true);
            WriteInElement(By.XPath("//input[contains(@placeholder, 'Select owners for this section.')]"), Constants.UserNumber1);
            ClickElement(By.XPath("//li[contains(@class,'select2-results__option select2-results__option--highlighted')]"));

            Constants.driver.SwitchTo().Frame(Constants.driver.FindElement(By.XPath("//iframe[contains(@class,'k-content')]")));
            WriteInElement(By.XPath("/html/body"), Constants.NewTask);
            Constants.driver.SwitchTo().DefaultContent();

            WaitForElement(By.Id("btn_save_section"), false);
            ClickElement(By.Id("btn_save_section"));
            //ClickElement(By.XPath("//button[contains(@class,'confirm')]"));

            ClickElement(By.Id("btn_submit"));

            //
            // 3. isop preview
            //

            WaitForElement(By.Id("btn_generate_sop_top"), false);
            ClickElement(By.Id("btn_generate_sop_top"));

            //
            // 4. add isop to assets and to incident
            //

            for (int i = 0; i < 3; i++)
            {
                if (WaitForElement(By.Id("attach_sop_to_incident"), true))
                    i = 3;
            }
            //ClickElement(By.Id("add_sop_to_assets"));
            //WaitForElement(By.ClassName("sa-button-container"));
            //ClosePopUp("Success");
            ClickElement(By.Id("attach_sop_to_incident"));
            //WaitForElement(By.ClassName("sa-button-container"), true);
            LogAlert(testName);
            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            string testName = "ISOP Wizard Delete";
            Console.WriteLine(testName);
            Asset.Delete("SOP-" + NewIncident); // TODO log result

            CheckUrl(BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@type, 'search')]"), Constants.NewIncident, By.XPath("//*[@id='tbl_active_sop_list']/tbody/tr[1]/td[1]")))
            {
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");
                //ShortWait();
            }

            LogAlert(testName);
            return Log.GetLog();
        }
    }
}
