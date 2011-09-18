using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FreeAppRss.AppRssDatabase;
using FreeAppRss.ScreenScrapper;

namespace OldAppParser
{
    class Program
    {
        public const string AMAZON_SEARCH_BASE_URL = "http://www.amazon.com/s/ref=nb_sb_noss?url=search-alias%3Dmobile-apps&field-keywords=";
        static void Main(string[] args)
        {
            List<string> lines = File.ReadAllLines(@"../../../PreviousApps.txt").ToList();
            foreach (string line in lines)
            {
                string[] appParams = line.Split(new char[] { '-' });
                for (int i = 0; i < appParams.Length; i++)
                {
                    appParams[i] = appParams[i].Trim();
                }
                App curApp = ScreenScraper.GetAppByName(appParams[1]);
                curApp.CreatedAt = DateTime.Parse(appParams[0]).AddHours(4);

                Console.WriteLine("Search Name: " + appParams[1] + ", Found Name: " + curApp.Name);

                DatabaseAccessor.AddAppIfNew(curApp);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
