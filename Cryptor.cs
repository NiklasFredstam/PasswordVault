using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PasswordVault
{
    public class Cryptor
    {
        public byte[] vaultKey { get; set; }
        public byte[] iv { get; set; }

        public Cryptor(byte[] secretKey, string masterPwd, byte[] iv)
        {
            vaultKey = CreateVaultKey(masterPwd, secretKey);
            this.iv = iv;
        }

        private byte[] CreateVaultKey(string masterPwd, byte[] secretKey)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(masterPwd, secretKey);
            return rfc.GetBytes(16);
        }

        public byte[] Encrypt(string toEncrypt)
        {
            byte[] encrypted;
            Aes aes = Aes.Create();
            aes.Key = vaultKey;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aes.CreateEncryptor(vaultKey, iv);
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        swEncrypt.Write(toEncrypt);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
            return encrypted;
        }

        public string Decrypt(byte[] toDecrypt)
        {
            string decrypted;
            Aes aes = Aes.Create();
            aes.Key = vaultKey;
            aes.IV = iv;
            aes.Padding = PaddingMode.PKCS7;
            ICryptoTransform decryptor = aes.CreateDecryptor(vaultKey, iv);
            using (MemoryStream msDecrypt = new MemoryStream(toDecrypt))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {
                        decrypted = srDecrypt.ReadToEnd();
                    }
                }
            }
            return decrypted;
        }
    }
}
