using System;
namespace TaskList.Droid.Facebook
{
    public class FacebookLoginResult
    {
        public string Token { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string UserId { get; set; }
        public string Error { get; set; }
        public bool IsCancelled { get; set; }
    }
}
