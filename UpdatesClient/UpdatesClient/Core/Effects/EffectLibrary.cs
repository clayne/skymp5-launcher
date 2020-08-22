using System;
using System.Reflection;
using System.Text;

namespace UpdatesClient.Core.Effects
{
    internal static class Global
    {
        public static Uri MakePackUri(string relativeFile)
        {

            StringBuilder uriString = new StringBuilder();
            uriString.Append("pack://application:,,,");
            uriString.Append("/" + AssemblyShortName + ";component/" + relativeFile);
            return new Uri(uriString.ToString(), UriKind.RelativeOrAbsolute);
        }

        private static string AssemblyShortName
        {
            get
            {
                if (_assemblyShortName == null)
                {
                    Assembly a = typeof(Global).Assembly;

                    // Pull out the short name.
                    _assemblyShortName = a.ToString().Split(',')[0];
                }

                return _assemblyShortName;
            }
        }

        private static string _assemblyShortName;
    }
}
