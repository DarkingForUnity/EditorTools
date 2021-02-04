//=====================================================
// - FileName:      AES.cs
// - CreateTime:    2021.1.15
// - Description:   AES加密算法，可实现加密解密功能(平台的适配性待验证)
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class AES : MonoBehaviour {


	void Start () {
        string user = Encrypt("admin");
        string pass = Encrypt("666666");
        Debug.Log("密文   "+user+"   ****************   "+ pass);
        string GetUser = Decrypt(user);
        string GetPass = Decrypt(pass);
        Debug.Log("明文   " + GetUser + "   ****************   " + GetPass);
    }

    /// <summary>
    /// 获取密钥
    /// </summary>
    private static string Key
    {
        get { return @"qO[NB]6,YF}gefcaj{+oESb9d8>Z'e9M"; }
    }

    /// 获取向量
    /// </summary>
    private static string IV
    {
        get { return @"L+\~f4.Ir)b$=pkf"; }
    }

    /// <summary>
    /// AES加密
    /// </summary>
    /// <param name="plainStr">明文字符串</param>
    /// <returns>密文</returns>
    public static string Encrypt(string plainStr)
    {
        byte[] bKey = Encoding.UTF8.GetBytes(Key);
        byte[] bIV = Encoding.UTF8.GetBytes(IV);
        byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);

        string encrypt = null;
        Rijndael aes = Rijndael.Create();
        try
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    encrypt = Convert.ToBase64String(mStream.ToArray());
                }
            }
        }
        catch { }
        aes.Clear();

        return encrypt;
    }

    /// <summary>
    /// AES解密
    /// </summary>
    /// <param name="encryptStr">密文字符串</param>
    /// <returns>明文</returns>
    public static string Decrypt(string encryptStr)
    {
        byte[] bKey = Encoding.UTF8.GetBytes(Key);
        byte[] bIV = Encoding.UTF8.GetBytes(IV);
        byte[] byteArray = Convert.FromBase64String(encryptStr);

        string decrypt = null;
        Rijndael aes = Rijndael.Create();
        try
        {
            using (MemoryStream mStream = new MemoryStream())
            {
                using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                {
                    cStream.Write(byteArray, 0, byteArray.Length);
                    cStream.FlushFinalBlock();
                    decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                }
            }
        }
        catch { }
        aes.Clear();

        return decrypt;
    }
}
