using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace Selenium_CC_CA.Initialisers
{
    public class Response_Options : Constants
    {
        private const string PagePath = "/messaging/messageresponse";
        private static string OptionToEditType = "Ping";
        private static string OptionToEdit = "No opinion";
        private const string EditedOption = "No Opinion On This Matter.";
        private const string TableID = "tblMsgResponsePing";


        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Response options edit";
            Console.WriteLine(testName);
            if (GoToResponsOptionsPage(testName))
            {
                OptionToEdit = GetText(By.XPath($"//table[@id='{TableID}']/tbody/tr[1]/td[1]")) ?? OptionToEdit; // Constants.driver.FindElement(By.XPath("//table[@id='tblMsgResponse']/tbody/tr[1]/td[1]")).Text;
                EditResponseOption(OptionToEdit, EditedOption, string.Empty);

                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Response Option Edit Successful.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Response Option Edit Failed.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Response Option Edit Failed.");
            }
            return Log.GetLog();
        }

        public static List<LogItem> Edit2()
        {
            Log.Clear();
            string testName = "Response options reverse edit";
            Console.WriteLine(testName);
            if (GoToResponsOptionsPage(testName))
            {
                EditResponseOption(EditedOption, OptionToEdit, OptionToEditType);
                Console.WriteLine("Test: " + testName + " | Result: PASS " + " | Outcome: Response options reverse edit Successful.");
            }
            else
            {
                Log.Entry(Log.Fail, testName, "Response options reverse edit Failed.");
                Console.WriteLine("Test: " + testName + " | Result: FAIL " + " | Outcome: Response options reverse edit Failed.");
            }
            return Log.GetLog();
        }

        /// <summary>
        /// edits a response option to the new value and changes the type to the type selected.
        /// </summary>
        /// <param name="optionToEdit"></param>
        /// <param name="editOptionTo"></param>
        /// <param name="optionType">Obsolete, the type of a response option cannot be changed anymore</param>
        /// <returns></returns>
        public static void EditResponseOption(string optionToEdit, string editOptionTo, string optionType)
        {
            if (TableSearch(By.XPath("//div[@id='tblMsgResponsePing_filter']/label/input"), optionToEdit, By.XPath($"//*[@id='{TableID}']/tbody/tr/td[1]")))
            {
                ClickElement(By.LinkText("Edit")); // XPath("//a[contains(@class,'btn btn-sm btn-small btn-warning')]"));
                WaitForElement(By.Id("msgresponsefrm"), true);

                WriteInElement(By.Id("ResponseLabel"), editOptionTo);

                ClickElement(By.Id("btn_submit"));
            }
            LogAlert("Response options edit");

        }

        /// <summary>
        /// go to the response options page and wait for the table to be displayed.
        /// </summary>
        private static bool GoToResponsOptionsPage(string testName)
        {
            CheckUrl(Constants.BaseUrl + PagePath);
            ClickElement(By.LinkText("Ping"));
            if (!WaitForElement(By.Id(TableID), true))
            {
                Log.Entry(Log.Fail, testName, $"Table with ID {TableID} could not be found.");
                return false;
            }
            return true;
        }

    }
}
