using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace PasswordVault
{
    public class FileHandler
    {
        public string clPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\client.dat";
        public string sPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\server.dat";
        public FileHandler()
        {
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
