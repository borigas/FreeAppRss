using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;

namespace FreeAppRss.AppRssDatabase
{
    public class App
    {
        public App()
        {
            CreatedAt = DateTime.Now;
        }

        public int AppId { get; set; }
        public string Name { get; set; }
        public string Vendor { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public double ListPrice { get; set; }
        public DateTime CreatedAt { get; set; }

        public SyndicationItem ToSyndicationItem()
        {
            string title = string.Format("{0} by {1} - Previously {2:C}", Name, Vendor, ListPrice);
            string content = Description;
            string id = AppId.ToString();
            Uri link = new Uri(Link);
            string author = Vendor;

            SyndicationItem item = new SyndicationItem(title, content, link, id, CreatedAt);
            item.Authors.Add(new SyndicationPerson(author));

            return item;
        }

        public bool Validate()
        {
            bool isValid =
                Link != null && Link != string.Empty &&
                Name != null && Name != string.Empty &&
                Vendor != null && Vendor != string.Empty &&
                ListPrice >= 0;
            return isValid;
        }
    }
}
