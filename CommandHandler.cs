using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace PasswordVault
{
    public class CommandHandler
    {

        public InputHandler ih { get; set; }
        public FileHandler fh { get; set; }
        //Probably doesnt need to be properties
        public PasswordVault pwdVault { get; set; }
        //Same with this one
        public Client client { get; set; }
        private Cryptor cryptor { get; set; }
        public Dictionary<string, string> passwords { get; set; } = new Dictionary<string, string>() { };

        public CommandHandler(InputHandler ih)
        {
            this.ih = ih;
            fh = new FileHandler(ih.clPath, ih.sPath);
        }

        public void RunCommand()
        {
            switch (ih.cmd)
            {
                case "init":
                    Init();
                    ih.ConfirmCommandSuccesful();
                    break;
                case "create":
                    Create();
                    ih.ConfirmCommandSuccesful();
                    break;
                case "get":
                    Get();
                    break;
                case "set":
                    Set();
                    break;
                case "delete":
                    Delete();
                    ih.ConfirmCommandSuccesful();
                    break;
                case "secret":
                    Secret();
                    break;
            }
        }

        private void Init()
        {
            byte[] iv = new byte[16];
            byte[] secretKey = new byte[16];
            RandomNumberGenerator.Create().GetBytes(iv);
            RandomNumberGenerator.Create().GetBytes(secretKey);
            client = new Client(ByteArrayToString(secretKey));
            cryptor = new Cryptor(secretKey, ih.mPwd, iv);
            UpdateServerFile();
            UpdateClientFile();
        }

        private void Create()
        {
            GetServerObject();
            byte[] key = StringToByteArray(ih.secretKey);
            byte[] iv = StringToByteArray(pwdVault.iv);
            cryptor = new Cryptor(key, ih.mPwd, iv);
            client = new Client(ByteArrayToString(key));
            FillPasswordDictionary();
            UpdateClientFile();
        }

        private void Get()
        {
            GetServerObject();
            GetClientObject();
            FillPasswordDictionary();
            string toPrint = "";
            if(ih.prop == "")
            {
                foreach (var i in passwords)
                {
                    toPrint += i.Key + ": " + i.Value + "\n";
                }
                ih.PrintTextToConsole(toPrint);
            }
            else if (passwords.TryGetValue(ih.prop, out string pwd))
            {
                ih.PrintTextToConsole(pwd);
            }
        }

        public void Set()
        {
            GetServerObject();
            GetClientObject();
            FillPasswordDictionary();
            if (ih.generatePassword)
            {
                ih.propPwd = GeneratePassword();
            }
            passwords.Add(ih.prop, ih.propPwd);
            UpdateServerFile();
        }

        private void Delete()
        {
            GetServerObject();
            GetClientObject();
            FillPasswordDictionary();
            if (!passwords.Remove(ih.prop))
            {
                ih.PrintErrorToConsoleAndExit("No password stored under " + ih.prop);
            }
            UpdateServerFile();
        }

        private void Secret()
        {
            GetClientObject();
            ih.PrintTextToConsole(client.secret);
        }

        private void FillPasswordDictionary()
        {
            try
            {
                string decrStr = GetDecryptedString();
                passwords = JsonSerializer.Deserialize<Dictionary<string, string>>(decrStr);
            }
            catch(CryptographicException e)
            {
                ih.PrintErrorToConsoleAndExit("Decryption unsuccessful: " + e.Message);
            }

        }

        private string GetDecryptedString()
        {
            byte[] key = StringToByteArray(client.secret);
            byte[] iv = StringToByteArray(pwdVault.iv);
            byte[] data = StringToByteArray(pwdVault.data);
            cryptor = new Cryptor(key, ih.mPwd, iv);
            return cryptor.Decrypt(data);
        }

        private byte[] EncryptPasswordData()
        {
            string pwdsJson = JsonSerializer.Serialize(passwords);
            return cryptor.Encrypt(pwdsJson);
        }

        private byte[] StringToByteArray(string s)
        {
            try
            {
                return Convert.FromBase64String(s);
            }
            catch (FormatException e)
            {
                ih.PrintErrorToConsoleAndExit(e.Message + " Decryption failed");
                return null;
            }
        }

        private string ByteArrayToString(byte[] b)
        {
            try
            {
                return Convert.ToBase64String(b);
            }
            catch (FormatException e)
            {
                ih.PrintErrorToConsoleAndExit(e.Message);
                return null;
            }
        }

        private void UpdateServerFile()
        {
            string ivStr = ByteArrayToString(cryptor.iv);
            string dataStr = ByteArrayToString(EncryptPasswordData());
            pwdVault = new PasswordVault(ivStr, dataStr);
            string pwdVaultJson = JsonSerializer.Serialize(pwdVault);
            fh.WriteAllToServerFile(pwdVaultJson);
        }

        private void UpdateClientFile()
        {
            string clientJson = JsonSerializer.Serialize(client);
            fh.WriteAllToClientFile(clientJson);
        }

        private void GetClientObject()
        {
            try
            {
                string strClient = fh.ReadSecretFromClientFile();
                client = JsonSerializer.Deserialize<Client>(strClient);
            }
            catch(FileNotFoundException e)
            {
                ih.PrintErrorToConsoleAndExit(e.Message);
            }
        }

        private void GetServerObject()
        {
            try
            {
                string strServer = fh.ReadAllFromServerFile();
                pwdVault = JsonSerializer.Deserialize<PasswordVault>(strServer);
            }
            catch(FileNotFoundException e)
            {
                ih.PrintErrorToConsoleAndExit(e.Message);
            }
        }

        private string GeneratePassword()
        {
            char[] result = new char[20];
            char[] chars = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            for(int i = 0; i < result.Length; i++)
            {
                int rnd = RandomNumberGenerator.GetInt32(chars.Length);
                result[i] = chars[rnd];
            }
            return new string(result);
        }
    }
}
