## [QR Code](https://github.com/DarkingForUnity/EditorTools/tree/master/Assets/Tools_DK/QR)
* 二维码生成器
	* Local：本地生成；
	* Network request ：请求草料二维码API ，返回二维码图片，需要网络

### Loca：

使用本地生成二维码需要引入“zxing.dll”文件，对字符串进行解析。

* 1、将字符串数据转换成二维码数据：

```C#
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
```

* 2、将二维码数据交给对应的图片，目前图片的显示主要为UGUI中的Texture格式和Sprite精灵图片，以下就分别赋值给两种格式

Texture：

```C#
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
```
Sprite:

```C#
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

```

### Net：

网络请求二维码，是直接调用草料二维码在线生成二维码API，通过解析返回页面，获取对应二维码图片，再交给对应图片对象；

* 网络请求：
    * 查询草料官网的API，找到请求格式，这就是之后我们需要请求的地址；<br>
![QR](https://github.com/DarkingForUnity/DarkingForUnity.github.io/blob/main/images/草料API.png)<br>

    * 在浏览器根据API的格式输入请求地址 https://cli.im/api/qrcode/code?text=https://DarkingForUnity.github.io <br>
    * 在浏览器界面按下F12，调出界面代码，依次点击 1、2，在3区域能看到二维码的图片链接，我们需要的就是提取`src="//`后面的图片链接。<br>
![QR](https://github.com/DarkingForUnity/DarkingForUnity.github.io/blob/main/images/QRpage.png)<br>
    * 将提取到的图片链接直接输入浏览器，就可以看到成功出现了二维码，就表示这个地址可用，我们可以在unity中使用该地址获取图片<br>
![QR](https://github.com/DarkingForUnity/DarkingForUnity.github.io/blob/main/images/QRpage1.png)<br>

```C#
 WWW w = new WWW("https://cli.im/api/qrcode/code?text=" + message);
        yield return w;
        //获取'src=" //' 后所有的数据
        string s = w.text.Substring(w.text.IndexOf("<img src=") + 12, w.text.Length - (w.text.IndexOf("<img src=") + 12));
        //截取src="" 内部的链接地址，不包括'//'
        string result = s.Substring(0, s.IndexOf("\""));
        
        WWW www = new WWW(result);
        yield return www;
        encoded.texture= www.texture;

```

* 获取图片信息后的赋值操作和上面的一样，就不重复赘述。

![QR](https://github.com/DarkingForUnity/DarkingForUnity.github.io/blob/main/images/QR.png)<br>


---
