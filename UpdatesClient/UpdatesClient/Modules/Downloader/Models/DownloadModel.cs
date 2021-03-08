using System;

namespace UpdatesClient.Modules.Downloader.Models
{
    public class DownloadModel
    {
        public string Url { get; }
        public string DestinationPath { get; }
        public string Description { get; }
        public string Version { get; }

        public Action PostAction { get; }
        public string PostActionDescription { get; }

        public bool Performed { get; set; } = false;
        public bool Success { get; set; } = false;

        public DownloadModel(string url, string destinationPath, string description, string version, Action action, string postActionDescription)
        {
            Url = url;
            DestinationPath = destinationPath;
            Description = description;
            Version = version;
            PostAction = action;
            PostActionDescription = postActionDescription;
        }
    }
}
