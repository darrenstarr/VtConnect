using System;
using System.Collections.Generic;
using System.Text;

namespace VtConnect
{
    public class UsernamePasswordCredentials : NetworkCredentials
    {
        public string Password { get; set; }
    }
}
