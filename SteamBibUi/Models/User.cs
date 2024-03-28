using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamBibUi.Models
{
    public class User
    {
        public static User currentUser { get; set; }
        public int? failedLoginAttempts { get; set; }
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int? statusId { get; set; }
    }
}
