using System;
using System.Collections.Generic;
using System.Text;

namespace HotelAPI.Core
{
    public class User
    {
        public string email { get; set; }
        public string password { get; set; }
        public bool returnSecureToken { get; set; }
    }
}
