using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace API.Helpers
{
    public static class Helper
    {
        private static byte[] key = Convert.FromBase64String("AQo7EqUS/kPzOGF2t5ckQA==");
        public static string PublicKey
        {
            get => Convert.ToBase64String(key);
            set => PublicKey = value;
        }

        private static string PrivateKey
        {
            get
            {
                var key = "3fb7fe5dbb0643caa984f53de6fffd0f";

                const string envVarName = "APP_ENCRYPTION_SECRET_KEY";

                var envKeyValue = Environment.GetEnvironmentVariable(envVarName);

                if (envKeyValue != null)
                {
                    key = envKeyValue;
                }
                return key;
            }
        }

        public static string Decrypt(string cipherText, string publicKey)
        {
            if (cipherText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(cipherText));
            if (PrivateKey is not { Length: > 0 })
                throw new ArgumentNullException(nameof(PrivateKey));
            if (publicKey is not { Length: > 0 })
                throw new ArgumentNullException(nameof(publicKey));

            using var aesAlg = Aes.Create();
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Key = CreateAesKey(PrivateKey);
            aesAlg.IV = Convert.FromBase64String(publicKey);

            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            var plaintext = srDecrypt.ReadToEnd();

            return plaintext;
        }

        public static string Encrypt(string plainText, string publicKey)
        {
            if (plainText is not { Length: > 0 })
                throw new ArgumentNullException(nameof(plainText));
            if (PrivateKey is not { Length: > 0 })
                throw new ArgumentNullException(nameof(PrivateKey));
            if (publicKey is not { Length: > 0 })
                throw new ArgumentNullException(nameof(publicKey));

            byte[] encrypted;

            using (var aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Key = CreateAesKey(PrivateKey);
                aesAlg.IV = Convert.FromBase64String(publicKey);
                //aesAlg.GenerateKey();
                //aesAlg.GenerateIV();

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted);
        }

        public static byte[] GenerateRandomPublicKey()
        {
            var iv = new byte[16]; // AES > IV > 128 bit
            iv = RandomNumberGenerator.GetBytes(iv.Length);
            return iv;
        }

        public static T SafeGet<T>(dynamic input, T defaultValue = default!)
        {
            if (input is null) return defaultValue;

            if (input is JsonElement element)
            {
                if (element.ValueKind == JsonValueKind.Null) return defaultValue;

                if (typeof(T).IsArray)
                {
                    // Case A: It's a real array -> Deserialize it
                    if (element.ValueKind == JsonValueKind.Array)
                        return element.Deserialize<T>() ?? defaultValue;

                    // Case B: It's a string (e.g. "None") -> Return empty array
                    if (element.ValueKind == JsonValueKind.String)
                        return defaultValue;
                }


                if (typeof(T) == typeof(int))
                {
                    if (element.ValueKind == JsonValueKind.Number)
                        return (T)(object)element.GetInt32();

                    if (element.ValueKind == JsonValueKind.String && int.TryParse(element.GetString(), out int i))
                        return (T)(object)i;
                }

                if (typeof(T) == typeof(string))
                {
                    return (T)(object)element.ToString();
                }


                try { return element.Deserialize<T>() ?? defaultValue; }
                catch { return defaultValue; }
            }

            return (T)input;
        }

        private static byte[] CreateAesKey(string inputString)

        {
            return Encoding.UTF8.GetByteCount(inputString) == 32 ? Encoding.UTF8.GetBytes(inputString) : SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
    }
}
