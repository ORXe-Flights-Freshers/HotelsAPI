using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectAPI.Core
{
    public class User
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }
    }
}
