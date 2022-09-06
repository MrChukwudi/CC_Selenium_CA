using OpenQA.Selenium;
using System;
using System.Collections.Generic;


namespace Selenium_CC_CA.Initialisers
{
    public class Asset : Constants
    {
        private static string PagePath = "/mediaasset";

        public static List<LogItem> Create()
        {
            Log.Clear();
            string testName = "Asset creation";
            CheckUrl(Constants.BaseUrl + PagePath);

            WaitForElement(By.Id("tblAsset"), true);
            ClickElement(By.LinkText("Add Asset"));

            WaitForElement(By.Id("AssetTitle"), true);

            WriteInElement(By.XPath("//input[contains(@id, 'AssetTitle')]"), Constants.NewAsset);

            ClickElement(By.Id("select2-AssetTypeId-container"));
            ClickElement(By.XPath("//li[text() [contains(.,'Incident Asset')]]"));
            ClickElement(By.Id("select2-AssetType-container"));
            ClickElement(By.XPath("//li[text() [contains(.,'Website')]]"));

            WriteInElement(By.Id("AssetDescription"), Constants.SimpleText);
            WriteInElement(By.Id("WebsiteURL"), "https://www.test.com");

            ClickElement(By.CssSelector("#select2-AssetOwner-container > .select2-selection__placeholder"));
            WriteInElement(By.XPath("//input[contains(@class,'select2-search__field')]"), Constants.UserNumber1 + Keys.Enter);
            ClickElement(By.XPath("//*[contains(@class,'select2-results__option select2-results__option--highlighted')]"));
            ClickElement(By.Id("AssetTitle"));

            //ClickElement(By.Id("select2-ReviewFrequency-container"));
            //ClickElement(By.XPath("//li[text() [contains(.,'Yearly')]]"));

            ClickElement(By.Id("btn_submit"));

            LogAlert(testName);

            return Log.GetLog();

        }
        public static List<LogItem> Edit()
        {
            Log.Clear();
            string testName = "Asset edit";

            CheckUrl(Constants.BaseUrl + PagePath);

            if (TableSearch(By.XPath("//input[contains(@type, 'search')]"), Constants.NewAsset, By.XPath("//*[@id='tblAsset']/tbody/tr[1]/td[1]/a")))
            {
                ClickElement(By.XPath("//*[@id='tblAsset']/tbody/tr[1]/td[1]/a"));

                if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
                {
                    ShortWait();
                }

                var tabs = Constants.driver.WindowHandles;
                if (tabs.Count > 1)
                {
                    Constants.driver.SwitchTo().Window(tabs[1]);
                    Constants.driver.Close();
                    Constants.driver.SwitchTo().Window(tabs[0]);
                }

                ExtendLineView();

                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Edit"));

                WaitForElement(By.Id("WebsiteURL"), true);

                WriteInElement(By.Id("WebsiteURL"), "https://www.testtwo.com");

                ClickElement(By.Id("btn_submit"));
            }

            LogAlert(testName);

            return Log.GetLog();
        }

        public static List<LogItem> Delete()
        {
            Log.Clear();
            Delete(Constants.NewAsset);

            return Log.GetLog();
        }

        public static void Delete(string asset)
        {
            string testName = "Asset Deletion";
            CheckUrl(Constants.BaseUrl + PagePath);

            WaitForElement(By.Id("tblAsset"), true);

            if (TableSearch(By.XPath("//input[contains(@type, 'search')]"), asset, By.XPath("//*[@id='tblAsset']/tbody/tr[1]/td[1]/a")))
            {
                if (asset.Equals(Constants.NewAsset, System.StringComparison.OrdinalIgnoreCase))
                {
                    // if the asset is not the default one, then do not risk downloading the asset as the dialog box is impossible to handle at the moment.
                    ExtendLineView();
                }
                ShortWait();
                ClickElement(By.XPath("//button[contains(@class,'btn btn-primary btn-sm btn-small dropdown-toggle')]"));
                ClickElement(By.LinkText("Delete"));
                ShortWait();
                ClosePopUp("Delete Confirmation");
            }

            LogAlert(testName);
        }

        /// <summary>
        /// If the table entry has been collapsed due to lack of display width, extends the line to make sure the buttons are visible.
        /// </summary>
        private static void ExtendLineView()
        {
            if (IsElementPresent(By.XPath("//table[@id='tblAsset']/tbody/tr/td"), false/*By.CssSelector("td.p20.sorting_1")*/))
            {
                // clicking on this element will extend the line but also open the asset link in a new tab.
                ClickElement(By.CssSelector("td.p20.sorting_1"));
                if (Constants.driver.GetType().Name.ToLower().Contains("firefox"))
                {
                    ShortWait();
                }
                // here we close the tab opened by the link and then get back to the original tab.
                System.Collections.ObjectModel.ReadOnlyCollection<string> tabs = Constants.driver.WindowHandles;
                if (tabs.Count > 1)
                {
                    Constants.driver.SwitchTo().Window(tabs[1]);
                    Constants.driver.Close();
                    Constants.driver.SwitchTo().Window(tabs[0]);
                }
            }
        }
    }
}
