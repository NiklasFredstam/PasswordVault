using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordVault
{
    public class Program
    {
        static public void Main(string[] args)
        {
            InputHandler ih = new InputHandler(args);
            CommandHandler ch = new CommandHandler(ih);
            ch.RunCommand();
        }
    }
}
