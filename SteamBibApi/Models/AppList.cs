namespace SteamBibApi.Models
{
    public class App
    {
        public int Appid { get; set; }
        public string Name { get; set; }
    }

    public class AppList
    {
        public List<App> Apps { get; set; }
    }

    public class RootObject
    {
        public AppList Applist { get; set; }
    }
}
