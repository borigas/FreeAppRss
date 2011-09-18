using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace FreeAppRss.AppRssDatabase
{
    class AppRssDatabase : DbContext
    {
        public AppRssDatabase() : base("AppRssDatabase")
        {
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<AppRssDatabase>());
        }
        public DbSet<App> Apps { get; set; }
    }
}
