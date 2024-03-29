using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamBibApi.Models
{
    public class AppDetails
    {
        public string AppId { get; set; }
        public bool Success { get; set; }
        public AppData Data { get; set; }
    }

    public class AppData
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Steam_AppId { get; set; }
        public JsonElement Required_Age { get; set; }
        public bool Is_Free { get; set; }
        public string Detailed_Description { get; set; }
        public string About_The_Game { get; set; }
        public string Short_Description { get; set; }
        public string Supported_Languages { get; set; }
        public string Header_Image { get; set; }
        public string Capsule_Image { get; set; }
        public string Capsule_ImageV5 { get; set; }
        public string Website { get; set; }
        public JsonElement Pc_Requirements { get; set; }
        public Dictionary<string, string> Recommended { get; set; }
        public List<string> MacRequirements { get; set; }
        public List<string> LinuxRequirements { get; set; }
        public string Legal_Notice { get; set; }
        public List<string> Developers { get; set; }
        public List<string> Publishers { get; set; }
        public List<object> Package_Groups { get; set; }
        public Dictionary<string, bool> Platforms { get; set; }
        public List<Category> Categories { get; set; }
        public List<Genre> Genres { get; set; }
        public List<Screenshot> Screenshots { get; set; }
        public List<Movie> Movies { get; set; }
        public ReleaseDate Release_Date { get; set; }
        public SupportInfo Support_Info { get; set; }
        public string Background { get; set; }
        public string BackgroundRaw { get; set; }
        public ContentDescriptors Content_Descriptors { get; set; }
        public object Ratings { get; set; }

        public string PlainTextAboutTheGame
        {
            get
            {
                return RemoveHtmlTags(About_The_Game);
            }
        }

        private string RemoveHtmlTags(string input)
        {
            string plainText = Regex.Replace(input, "<.*?>", string.Empty);

            plainText = Regex.Replace(plainText, @"(?<=[.!?])(?<!\s+)", " ");

            return plainText;
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public class Genre
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }

    public class Screenshot
    {
        public int Id { get; set; }
        public string Path_Thumbnail { get; set; }
        public string Path_Full { get; set; }
    }

    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Thumbnail { get; set; }
        public Dictionary<string, string> Webm { get; set; }
        public Dictionary<string, string> Mp4 { get; set; }
        public bool Highlight { get; set; }
    }

    public class ReleaseDate
    {
        public bool ComingSoon { get; set; }
        public string Date { get; set; }
    }

    public class SupportInfo
    {
        public string Url { get; set; }
        public string Email { get; set; }
    }

    public class ContentDescriptors
    {
        public List<object> Ids { get; set; }
        public object Notes { get; set; }
    }

}
