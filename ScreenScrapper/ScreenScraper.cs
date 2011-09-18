using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Configuration;
using FreeAppRss.AppRssDatabase;

namespace FreeAppRss.ScreenScrapper
{
    public static class ScreenScraper
    {
        public static void CheckForApp()
        {
            string appStoreHomePageUrl = ConfigurationManager.AppSettings["AppStoreHomePageUrl"];

            string appUrl = GetFreeAppLink(appStoreHomePageUrl);

            App todaysApp = GetApp(appUrl);

            DatabaseAccessor.AddAppIfNew(todaysApp);
        }

        private static string GetFreeAppLink(string appStoreHomePageUrl)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlNode appStoreHomeMainNode = htmlWeb.Load(appStoreHomePageUrl).GetElementbyId("centercol");
            string appUrl = "http://www.amazon.com" + SelectFirstNodeAttribute(appStoreHomeMainNode, "//div[@class='app-info-name']/a", "href");
            return appUrl;
        }

        public static App GetApp(string appUrl)
        {
            HtmlWeb htmlWeb = new HtmlWeb();

            App todaysApp = new App();
            todaysApp.Link = appUrl;

            HtmlNode appPageMainNode = htmlWeb.Load(appUrl).GetElementbyId("divsinglecolumnminwidth");

            todaysApp.Name = SelectFirstNodeInnerHtml(appPageMainNode, "//span[@id='btAsinTitle']");
            todaysApp.Vendor = SelectFirstNodeInnerHtml(appPageMainNode, "//div[@class='buying']/span/a");

            string priceString = SelectFirstNodeInnerHtml(appPageMainNode, "//span[@class='listprice']").TrimStart(new char[] { '$', ' ' });
            if (priceString == string.Empty)
            {
                priceString = SelectFirstNodeInnerHtml(appPageMainNode, "//b[@class='priceLarge']").TrimStart(new char[] { '$', ' ' });
            }
            todaysApp.ListPrice = double.Parse(priceString);

            todaysApp.Description = SelectFirstNodeInnerHtml(appPageMainNode, "//div[@class='aplus']");
            return todaysApp;
        }

        public static App GetAppByName(string appName)
        {
            string searchUrl = ConfigurationManager.AppSettings["AppStoreSearchPageUrl"] + appName.Replace(' ', '+');

            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlNode firstResult = htmlWeb.Load(searchUrl).GetElementbyId("results");
            if (firstResult != null)
            {
                HtmlNode searchNode = firstResult.SelectNodes("//span[@class='srTitle']")[0].ParentNode;

                string appUrl = SelectNodeAttribute(searchNode, "href");

                return GetApp(appUrl);
            }
            else
            {
                return new App() { Name = appName };
            }
        }

        private static HtmlNode SelectFirstNode(HtmlNode ancestorNode, string xpath)
        {
            if (ancestorNode != null)
            {
                HtmlNodeCollection nodes = ancestorNode.SelectNodes(xpath);
                if (nodes != null && nodes.Count > 0)
                {
                    return nodes[0];
                }
            }
            return null;
        }

        private static string SelectNodeInnerHtml(HtmlNode node)
        {
            string innerHtml = string.Empty;
            if (node != null)
            {
                innerHtml = node.InnerHtml;
            }
            return innerHtml;
        }

        private static string SelectFirstNodeInnerHtml(HtmlNode ancestorNode, string xpath)
        {
            string innerHtml = string.Empty;
            HtmlNode node = SelectFirstNode(ancestorNode, xpath);
            return SelectNodeInnerHtml(node);
        }

        private static string SelectFirstNodeAttribute(HtmlNode ancestorNode, string xpath, string attributeName)
        {
            string attributeValue = string.Empty;
            HtmlNode node = SelectFirstNode(ancestorNode, xpath);
            if (node != null)
            {
                attributeValue = SelectNodeAttribute(node, attributeName);
            }
            return attributeValue;
        }

        private static string SelectNodeAttribute(HtmlNode node, string attributeName)
        {
            string attributeValue = string.Empty;
            if (node.Attributes.Contains(attributeName))
            {
                attributeValue = node.Attributes[attributeName].Value;
            }
            return attributeValue;
        }
    }
}
