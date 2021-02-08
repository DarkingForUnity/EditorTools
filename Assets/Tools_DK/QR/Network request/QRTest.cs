//=====================================================
// - FileName:      QRTest.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QRTest : MonoBehaviour {
   
    string QrCodeStr = "https://DarkingForUnity.github.io";
    public RawImage texture;
    public SpriteRenderer SpriteRenderer;
	void Start () {
        StartCoroutine(GetQR_network.getQR(texture, QrCodeStr));
        StartCoroutine(GetQR_network.getQR(SpriteRenderer, QrCodeStr));
    }

}
