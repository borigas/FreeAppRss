using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.ServiceModel.Syndication;

namespace FreeAppRss.AppRssFeed
{
    [ServiceContract]
    public interface IAppRssService
    {
        [OperationContract]
        [WebGet(UriTemplate = "GetApp")]
        string GetApp();

        [OperationContract]
        [WebGet]
        string GetData(string value);

        [OperationContract]
        [WebGet(UriTemplate = "")]
        Rss20FeedFormatter GetFeed();

        [OperationContract]
        [WebGet(UriTemplate = "Feed.rss")]
        Rss20FeedFormatter GetFeedWithName();
    }
}
