using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFileName;
    private bool encryptData;
    private readonly string passphrase = "SuperSecretPassphrase!!!";
    private const int SaltSize = 16;
    private const int Iterations = 200000;
    private const int KeySizeBits = 256;
    private const int IvSize = 16;
    private const int HmacSize = 32;

    public FileDataHandler(string _dataDirPath, string _dataFileName, bool _encryptData)
    {
        dataDirPath = _dataDirPath;
        dataFileName = _dataFileName;
        encryptData = _encryptData;
    }

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string jsonData = JsonUtility.ToJson(data, true);
            Debug.Log(jsonData);
            if (!encryptData)
            {
                File.WriteAllText(fullPath, jsonData);
            }
            else
            {
                byte[] encryptedBytes = EncryptAndHmac(jsonData);

                File.WriteAllBytes(fullPath, encryptedBytes);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving data: " + e.Message);
        }
    }

    public GameData Load()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        if (!File.Exists(fullPath))
        {
            return null;
        }

        try
        {
            if (!encryptData)
            {
                string jsonData = File.ReadAllText(fullPath);
                return JsonUtility.FromJson<GameData>(jsonData);
            }
            else
            {
                byte[] fileBytes = File.ReadAllBytes(fullPath);

                string decryptedJson = DecryptAndVerifyHmac(fileBytes);

                return JsonUtility.FromJson<GameData>(decryptedJson);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data: " + e.Message);
            return null;
        }
    }

    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
    private byte[] GenerateRandomBytes(int length)
    {
        byte[] randomBytes = new byte[length];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(randomBytes);
        }
        return randomBytes;
    }

    private byte[] EncryptAndHmac(string plainText)
    {
        byte[] salt = GenerateRandomBytes(SaltSize);

        byte[] key = DeriveKeyFromPassphrase(passphrase, salt, Iterations, KeySizeBits);

        byte[] iv = GenerateRandomBytes(IvSize);

        byte[] ciphertext;
        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

            using (var msEncrypt = new MemoryStream())
            {
                using (var encryptor = aes.CreateEncryptor())
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    csEncrypt.Write(plainBytes, 0, plainBytes.Length);
                }
                ciphertext = msEncrypt.ToArray();
            }
        }
        byte[] dataForHmac = Combine(salt, iv, ciphertext);
        byte[] hmac = ComputeHmacSha256(key, dataForHmac);

        byte[] finalBytes = Combine(salt, iv, ciphertext, hmac);

        return finalBytes;
    }
    private string DecryptAndVerifyHmac(byte[] encryptedData)
    {
        byte[] salt = new byte[SaltSize];
        Buffer.BlockCopy(encryptedData, 0, salt, 0, SaltSize);

        byte[] iv = new byte[IvSize];
        Buffer.BlockCopy(encryptedData, SaltSize, iv, 0, IvSize);

        int offsetCipher = SaltSize + IvSize;
        int offsetHmac = encryptedData.Length - HmacSize;
        int cipherLength = offsetHmac - offsetCipher;

        byte[] ciphertext = new byte[cipherLength];
        Buffer.BlockCopy(encryptedData, offsetCipher, ciphertext, 0, cipherLength);

        byte[] hmacFromFile = new byte[HmacSize];
        Buffer.BlockCopy(encryptedData, offsetHmac, hmacFromFile, 0, HmacSize);

        byte[] key = DeriveKeyFromPassphrase(passphrase, salt, Iterations, KeySizeBits);

        byte[] dataForHmac = Combine(salt, iv, ciphertext);
        byte[] hmacCalc = ComputeHmacSha256(key, dataForHmac);

        if (!CompareBytes(hmacCalc, hmacFromFile))
        {
            throw new CryptographicException("HMAC mismatch! Dữ liệu có thể đã bị chỉnh sửa hoặc sai passphrase.");
        }

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var msDecrypt = new MemoryStream(ciphertext))
            using (var decryptor = aes.CreateDecryptor())
            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (var srDecrypt = new StreamReader(csDecrypt))
            {
                return srDecrypt.ReadToEnd();
            }
        }
    }
    private byte[] DeriveKeyFromPassphrase(string pass, byte[] salt, int iterations, int keySizeBits)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(pass, salt, iterations, HashAlgorithmName.SHA256))
        {
            return pbkdf2.GetBytes(keySizeBits / 8);
        }
    }
    private byte[] ComputeHmacSha256(byte[] key, byte[] data)
    {
        using (var hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(data);
        }
    }
    private byte[] Combine(params byte[][] arrays)
    {
        int totalLength = 0;
        foreach (var arr in arrays)
        {
            totalLength += arr.Length;
        }

        byte[] combined = new byte[totalLength];
        int offset = 0;
        foreach (var arr in arrays)
        {
            Buffer.BlockCopy(arr, 0, combined, offset, arr.Length);
            offset += arr.Length;
        }

        return combined;
    }
    private bool CompareBytes(byte[] a, byte[] b)
    {
        if (a.Length != b.Length) return false;
        int diff = 0;
        for (int i = 0; i < a.Length; i++)
        {
            diff |= a[i] ^ b[i];
        }
        return diff == 0;
    }
}


//using System;
//using System.IO;
//using UnityEngine;

//public class FileDataHandler
//{
//    private string dataDirPath = "";
//    private string dataFileName = "";

//    private bool encryptData = false;
//    private string codeWord = "PkHIQBKzaBPBLAV6PKHkY_zphnai3MMYQKijAGmkspA";

//    public FileDataHandler(string _dataDirPath, string _dataFileName, bool encryptData)
//    {
//        this.dataDirPath = _dataDirPath;
//        this.dataFileName = _dataFileName;
//        this.encryptData = encryptData;
//    }
//    public void Save(GameData _data)
//    {
//        string fullPath = Path.Combine(dataDirPath, dataFileName);
//        try
//        {
//            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
//            string dataToStore = JsonUtility.ToJson(_data, true);

//            if (encryptData)
//            {
//                dataToStore = EncryptDecrypt(dataToStore);
//            }

//            using (FileStream stream = new FileStream(fullPath, FileMode.Create))
//            {
//                using (StreamWriter writer = new StreamWriter(stream))
//                {
//                    writer.Write(dataToStore);
//                }
//            }
//        }
//        catch (Exception e)
//        {
//            Debug.LogError("Error saving data: " + e.Message);
//        }
//    }
//    public GameData Load()
//    {
//        string fullPath = Path.Combine(dataDirPath, dataFileName);
//        GameData loadData = null;
//        if (File.Exists(fullPath))
//        {
//            try
//            {
//                string dataToLoad = "";
//                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
//                {
//                    using (StreamReader reader = new StreamReader(stream))
//                    {
//                        dataToLoad = reader.ReadToEnd();
//                    }
//                }
//                if (encryptData)
//                {
//                    dataToLoad = EncryptDecrypt(dataToLoad);
//                }
//                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
//            }
//            catch (Exception e)
//            {
//                Debug.LogError("Error loading data: " + e.Message);
//            }
//        }
//        return loadData;
//    }
//    public void Delete()
//    {
//        string fullPath = Path.Combine(dataDirPath, dataFileName);
//        if (File.Exists(fullPath))
//        {
//            File.Delete(fullPath);
//        }
//    }
//    private string EncryptDecrypt(string _data)
//    {
//        string modifiedData = "";

//        for (int i = 0; i < _data.Length; i++)
//        {
//            modifiedData += (char)(_data[i] ^ codeWord[(i % codeWord.Length)]);
//        }

//        return modifiedData;
//    }
//}