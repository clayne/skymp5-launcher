namespace UpdatesClient.UI.Pages.Models.AuthModels
{
    public class AuthModel
    {
        public string Email { get; set; }
        public bool RememberMe { get; set; }

        public AuthModel(string email, bool rememberMe)
        {
            Email = email;
            RememberMe = rememberMe;
        }

        public AuthModel()
        {
        }
    }
}
