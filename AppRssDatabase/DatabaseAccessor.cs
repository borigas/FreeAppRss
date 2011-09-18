using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeAppRss.AppRssDatabase
{
    public static class DatabaseAccessor
    {
        public static void AddAppIfNew(App app)
        {
            if (app.Validate())
            {
                using (AppRssDatabase db = new AppRssDatabase())
                {
                    int existingCount = db.Apps.Count(x => x.Name == app.Name);
                    if (existingCount == 0)
                    {
                        db.Apps.Add(app);
                        db.SaveChanges();
                    }
                }
            }
        }

        public static List<App> GetAllApps()
        {
            using (AppRssDatabase db = new AppRssDatabase())
            {
                return db.Apps.OrderByDescending(x => x.CreatedAt).ToList();
            }
        }
    }
}
