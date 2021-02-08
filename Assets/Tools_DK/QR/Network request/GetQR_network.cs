//=====================================================
// - FileName:      GetQR_network.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetQR_network : MonoBehaviour {

    public static IEnumerator getQR(RawImage encoded,string message)
    {
        WWW w = new WWW("https://cli.im/api/qrcode/code?text=" + message);
        yield return w;
        //获取'src=" //' 后所有的数据
        string s = w.text.Substring(w.text.IndexOf("<img src=") + 12, w.text.Length - (w.text.IndexOf("<img src=") + 12));
        //截取src="" 内部的链接地址，不包括'//'
        string result = s.Substring(0, s.IndexOf("\""));
        
        WWW www = new WWW(result);
        yield return www;
        encoded.texture= www.texture;
    }

    public static IEnumerator getQR(SpriteRenderer sprite, string message)
    {
        Texture2D encoded;
        WWW w = new WWW("https://cli.im/api/qrcode/code?text=" + message);
        yield return w;
        print("https://cli.im/api/qrcode/code?text=" + message);
        print(w.text);
        //获取'src=" //' 后所有的数据
        string s = w.text.Substring(w.text.IndexOf("<img src=") + 12, w.text.Length - (w.text.IndexOf("<img src=") + 12));
        //截取src="" 内部的链接地址，不包括'//'
        string result = s.Substring(0, s.IndexOf("\""));
        print(result);
        WWW www = new WWW(result);
        yield return www;
        encoded = www.texture;
        sprite.sprite = Sprite.Create(encoded, new Rect(0, 0, encoded.width, encoded.height), Vector2.zero);
        //SpriteRenderer.sprite = sprite;
        //SpriteRenderer.sprite = (Sprite)texture;
    }
}
