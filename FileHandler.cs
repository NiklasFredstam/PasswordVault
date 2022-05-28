using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PasswordVault
{
    public class FileHandler
    {
        public string clPath { get; set; }
        public string sPath { get; set; }
        public FileHandler(string clPath, string sPath)
        {
            this.clPath = clPath;
            this.sPath = sPath;
        }

        public void WriteAllToServerFile(string data)
        {
            //formatting? write iv and data, line break inbetween or as one PasswordVaultobject?
            File.WriteAllText(sPath, data);
        }

        public void WriteAllToClientFile(string jsonStr)
        {
            //formatting? write iv and data, line break inbetween or as one PasswordVaultobject?
            File.WriteAllText(clPath, jsonStr);
        }
        public string ReadAllFromServerFile()
        {
            return File.ReadAllText(sPath);

        }

        public string ReadSecretFromClientFile()
        {
            return File.ReadAllText(clPath);
        }
    }
}
