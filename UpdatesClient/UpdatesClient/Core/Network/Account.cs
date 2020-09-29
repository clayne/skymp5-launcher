using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdatesClient.Core.Network
{
    public class Account
    {
        public const string URL_Api = "https://skymp.io/api/";

        public static async Task<string> Register(string email, string name, string password)
        {
            string ver = await Net.Request($"{URL_Api}users", null);
            throw new NotImplementedException();
            return null;
        }

        public static async Task<string> Login(string email, string password)
        {
            string ver = await Net.Request($"{URL_Api}login", null);
            throw new NotImplementedException();
            return null;
        }

        public static async Task<string> Verify(int id, string email, string password, string pin)
        {
            string ver = await Net.Request($"{URL_Api}{id}/users", null);
            throw new NotImplementedException();
            return null;
        }

    }
}
