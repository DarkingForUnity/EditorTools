using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class CreatQR : MonoBehaviour {

    //需要生产二维码的字符串数组  
    string[] QrCodeStr = { "https://www.baidu.com/", "https://www.cnblogs.com/Mr-Miracle/", "https://unity3d.com/cn", "https://www.sogou.com/" };
    //在屏幕上显示二维码  
    public RawImage image;
    //存放二维码  
    Texture2D encoded;
    int Nmuber = 0;
    // Use this for initialization  
    void Start()
    {
        encoded = new Texture2D(256, 256);
    }
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Btn_CreatQr();
            Nmuber++;
            if (Nmuber >= QrCodeStr.Length)
            {
                Nmuber = 0;
            }
        }
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


    /// <summary>  
    /// 生成二维码  
    /// </summary>  
    public void Btn_CreatQr()
    {
       
        if (QrCodeStr[Nmuber].Length > 1)
        {
            //二维码写入图片    
            var color32 = Encode(QrCodeStr[Nmuber], encoded.width, encoded.height);
            encoded.SetPixels32(color32);
            encoded.Apply();
            //生成的二维码图片附给RawImage    
            image.texture = encoded;
        }
        else
        {
            GameObject.Find("Text_1").GetComponent<Text>().text = "没有生成信息";
        }
    }
}
