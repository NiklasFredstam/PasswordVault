using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PasswordVault
{
    public class PasswordVault
    {
        public string iv { get; set; }
        public string data { get; set; }

        public PasswordVault() { }

        public PasswordVault(string iv, string data)
        {
            this.iv = iv;
            this.data = data;
        }

    }
}
