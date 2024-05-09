using System.IO;
using System.Security.Cryptography;
using System.Text;

public static class AesEncryptor
{
    private static byte[] _IV;
    private static byte[] _key;

    private const string IVSource = "61KAnvXaAy9yDwN9";
    private const string KeySource = "B7LfVHXL86jtc7gsdOYr2qG9iIpVNLIs";

    static AesEncryptor()
    {
        _IV = Encoding.ASCII.GetBytes(IVSource);
        _key = Encoding.ASCII.GetBytes(KeySource);
    }

    public static byte[] Encrypt(string plainText)
    {
        byte[] encrypted;

        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform encryptor = aes.CreateEncryptor(_key, _IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter(cs))
                        sw.Write(plainText);

                    encrypted = ms.ToArray();
                }
            }
        }

        return encrypted;
    }

    public static string Decrypt(byte[] cipherText)
    {
        string plaintext = null;

        using (AesManaged aes = new AesManaged())
        {
            ICryptoTransform decryptor = aes.CreateDecryptor(_key, _IV);

            using (MemoryStream ms = new MemoryStream(cipherText))
            {
                using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader reader = new StreamReader(cs))
                        plaintext = reader.ReadToEnd();
                }
            }
        }

        return plaintext;
    }
}
