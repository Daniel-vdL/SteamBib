namespace SteamBibApi.Models
{
    public class User
    {
        public static User CurrentUser { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? StatusId { get; set; }
    }

    public class UserDto
    {
        public static User CurrentUser { get; set; }
        public int? FailedLoginAttempts { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public int? StatusId { get; set; }
    }

    public class UserLoginDto
    {
        public int? FailedLoginAttempts { get; set; }
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? StatusId { get; set; }
    }
}
