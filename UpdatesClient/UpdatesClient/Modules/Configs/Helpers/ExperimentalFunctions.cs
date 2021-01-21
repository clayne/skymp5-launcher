using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UpdatesClient.Core;

namespace UpdatesClient.Modules.Configs.Helpers
{
    public class ExperimentalFunctions
    {
        public static void Use(string message, Action action)
        {
            if (!Settings.Loaded) throw new TypeUnloadedException("Settings not loaded");

            try
            {
                if (Settings.ExperimentalFunctions == true)
                {
                    action.Invoke();
                }
            }
            catch (Exception e)
            {
                Logger.Error($"ExpFunc_{message}", e);
            }
        }




    }
}
