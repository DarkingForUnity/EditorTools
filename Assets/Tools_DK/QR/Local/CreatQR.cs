using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;

public class CreatQR : MonoBehaviour {

    //需要生产二维码的字符串 
    string QrCodeStr = "https://DarkingForUnity.github.io";
    //在屏幕上显示二维码载体  
    public RawImage image;//用于UGUI
    public SpriteRenderer SpriteRenderer;//用于sprite图片
  
    void Start()
    {
        image.texture = GetQR_Local.CreatQR_texture(QrCodeStr);
        SpriteRenderer.sprite= GetQR_Local.CreatQR_sprite(QrCodeStr);
    }

}
