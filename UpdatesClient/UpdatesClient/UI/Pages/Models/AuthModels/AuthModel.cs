using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
