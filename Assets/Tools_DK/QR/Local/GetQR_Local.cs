//=====================================================
// - FileName:      GetQR_Local.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

public class GetQR_Local : MonoBehaviour {

	public static Texture CreatQR_texture(string message)
    {
        Texture2D encoded;
        encoded = new Texture2D(256, 256);
        if (message.Length > 1)
        {
            //二维码写入图片    
            var color32 = Encode(message, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
        }
        else
        {
            Debug.Log("生成二维码失败");
        }
        return encoded;
    }

    public static Sprite CreatQR_sprite(string message)
    {
        Texture2D encoded;
        encoded = new Texture2D(256, 256);
        if (message.Length > 1)
        {
            //二维码写入图片    
            var color32 = Encode(message, encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
        }
        else
        {
            Debug.Log("生成二维码失败");
        }
        Sprite sprite = Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height), Vector2.zero);
        return sprite;
    }

    /// <summary>
    /// 定义方法生成二维码 
    /// </summary>
    /// <param name="textForEncoding">需要生产二维码的字符串</param>
    /// <param name="width">宽</param>
    /// <param name="height">高</param>
    /// <returns></returns>       
    private static Color32[] Encode(string textForEncoding, int width, int height)
    {
        var writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };
        return writer.Write(textForEncoding);
    }
}
