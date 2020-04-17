namespace App.Core
{
    public class AuthConfig
    {
        public string Authority { get; set; }
        public string Audience { get; set; }
        public bool RequireHttps { get; set; }
        public bool ByPass { get; set; }
    }
}