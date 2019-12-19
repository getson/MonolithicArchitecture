namespace MyApp.IdentityServer.Models
{
    public class LoginViewModel
    {
        public string ReturnUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
