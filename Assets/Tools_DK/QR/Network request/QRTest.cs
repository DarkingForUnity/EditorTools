//=====================================================
// - FileName:      QRTest.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QRTest : MonoBehaviour {
    public string text;

    public Texture2D texture;
    public SpriteRenderer SpriteRenderer;
	void Start () {
        StartCoroutine(getqr());
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        StartCoroutine(getqr());
    }

    IEnumerator getqr()
    {
        WWW w = new WWW("https://cli.im/api/qrcode/code?text="+ text);
        yield return w;
        //print(w.text);
        //获取'src=" //' 后所有的数据
        string s = w.text.Substring(w.text.IndexOf("<img src=") + 12, w.text.Length - (w.text.IndexOf("<img src=") + 12));
        //截取src="" 内部的链接地址，不包括'//'
        string result = s.Substring(0, s.IndexOf("\""));
        print(result);
        WWW www = new WWW(result);
        yield return www;
        texture = www.texture;
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        SpriteRenderer.sprite = sprite;
        //SpriteRenderer.sprite = (Sprite)texture;
    }
}
