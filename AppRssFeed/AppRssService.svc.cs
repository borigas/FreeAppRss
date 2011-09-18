using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Activation;
using FreeAppRss.ScreenScrapper;
using System.ServiceModel.Syndication;
using System.Configuration;
using FreeAppRss.AppRssDatabase;
using System.Web.Caching;
using System.Web;

namespace FreeAppRss.AppRssFeed
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AppRssService : IAppRssService
    {
        private static string APP_RSS_FEED_CACHE_KEY = "APP_RSS_FEED_CACHE";

        public string GetApp()
        {
            ScreenScraper.CheckForApp();
            return "GetTest() Result";
        }

        public string GetData(string value)
        {
            return string.Format("You entered: {0}", value);
        }

        public Rss20FeedFormatter GetFeedWithName()
        {
            return GetFeed();
        }

        public Rss20FeedFormatter GetFeed()
        {
            if (HttpRuntime.Cache[APP_RSS_FEED_CACHE_KEY] == null)
            {
                CacheFeed();
            }
            Rss20FeedFormatter feed = HttpRuntime.Cache[APP_RSS_FEED_CACHE_KEY] as Rss20FeedFormatter;

            return feed;
        }

        private static void CacheFeed()
        {
            try
            {
                ScreenScraper.CheckForApp();
            }
            catch (Exception) { }

            double cacheTimeoutMinutes = double.Parse(ConfigurationManager.AppSettings["CacheTimeoutMinutes"]);
            DateTime expireCache = DateTime.Now.AddMinutes(cacheTimeoutMinutes);
            HttpRuntime.Cache.Add(APP_RSS_FEED_CACHE_KEY, CreateAppRssFeed(), null, expireCache, Cache.NoSlidingExpiration,
                CacheItemPriority.Normal, AppFeedRemovedCallback);
        }

        private static Rss20FeedFormatter CreateAppRssFeed()
        {
            string feedTitle = ConfigurationManager.AppSettings["FreeAppFeedTitle"];
            string feedDescription = ConfigurationManager.AppSettings["FreeAppFeedDescption"];
            string feedUri = ConfigurationManager.AppSettings["FreeAppFeedUri"];
            string feedAuthor = ConfigurationManager.AppSettings["FreeAppFeedAuthor"];
            string feedCategory = ConfigurationManager.AppSettings["FreeAppFeedCategory"];

            SyndicationFeed feed = new SyndicationFeed(feedTitle, feedDescription, new Uri(feedUri));
            feed.Authors.Add(new SyndicationPerson(feedAuthor));
            feed.Categories.Add(new SyndicationCategory(feedCategory));

            feed.Items = DatabaseAccessor.GetAllApps().Select(x => x.ToSyndicationItem());

            Rss20FeedFormatter formatter = new Rss20FeedFormatter(feed);
            formatter.SerializeExtensionsAsAtom = false;

            return formatter;
        }

        public static void AppFeedRemovedCallback(String key, object value,
        CacheItemRemovedReason removedReason)
        {
            CacheFeed();
        }
    }
}
