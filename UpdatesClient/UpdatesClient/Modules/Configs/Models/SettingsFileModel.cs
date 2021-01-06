using Security.Extensions;

namespace UpdatesClient.Modules.Configs.Models
{
    public class SettingsFileModel
    {
        public string PathToSkyrim { get; set; }
        public string LastVersion { get; set; }
        public int? LastServerID { get; set; }
        public int? UserId { get; set; }
        public SecureString? UserToken { get; set; }
        public string Locale { get; set; }

        public SettingsFileModel()
        {
            LastServerID = -1;
            UserId = -1;
            UserToken = new SecureString();
        }
    }
}
