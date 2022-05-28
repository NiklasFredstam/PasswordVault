using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVault
{
    public class Client
    {
        public string secret { get; set; }

        public Client() { }
        public Client(string secret)
        {
            this.secret = secret;
        }
    }
}
