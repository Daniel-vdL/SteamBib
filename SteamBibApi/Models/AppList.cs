namespace SteamBibApi.Models
{
    public class App
    {
        public int appid { get; set; }
        public string name { get; set; }
    }

    public class AppList
    {
        public List<App> apps { get; set; }
    }

    public class RootObject
    {
        public AppList applist { get; set; }
    }
}
