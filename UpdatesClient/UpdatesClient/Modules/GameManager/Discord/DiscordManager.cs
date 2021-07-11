using DiscordRPC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Modules.GameManager.Discord
{
    public class DiscordManager
    {
        private const string AppID = "720433908352549034";
        private readonly DiscordRpcClient Client = new DiscordRpcClient(AppID);
        private readonly RichPresence presence = new RichPresence();
        private bool run = true;

        public DiscordManager(string ServerName)
        {
            Client.Initialize();
            presence.Details = ServerName;
            presence.Assets = new Assets()
            {
                LargeImageKey = "skymp",
                LargeImageText = "SkyMP",
                //SmallImageKey = "image_small"
            };
        }

        public void Start()
        {
            presence.Timestamps = new Timestamps(DateTime.UtcNow);
            Client.SetPresence(presence);
            run = true;
            Update();
        }

        private async void Update()
        {
            while (run)
            {
                Client.Invoke();
                await Task.Delay(300);
            }
        }

        public void Stop()
        {
            run = false;
            Client.Deinitialize();
            Client.Dispose();
        }
    }
}
