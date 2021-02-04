//=====================================================
// - FileName:      LitJsonTest.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using LitJson;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

//一个材料信息，包含如下成员
[Serializable]
public class MaterialItem
{
    //public static List<MaterialItem> materialList = new List<MaterialItem>();
    public string materialManufacturer;
    public string regularLabelling;
    public string sheetLabelling;
}

public class LitJsonTest : MonoBehaviour {

    
    public List<MaterialItem> materialList = new List<MaterialItem>();

    void Start () {
        //接口url
        //string url = " http://192.168.42.46/Windchill/servlet/Navigation/MaterialLibraryServlet ";
        string url = Application.dataPath + "/Resources/inputdate.json";
        //签名
        string restName = "Authorization";
        //认证
        string restValue = "Basic d2NhZG1pbjp3Y2FkbWlu";
        //传入参数，读取Json字符串
        //GetJsonData(url, restName, restValue);
        GetJsonData(url);
    }
    /// <summary>
    /// Json解析的方法封装
    /// </summary>
    /// <param name="tmpUrlI">传入的接口Url</param>
    /// <param name="tmpKeyName">签名</param>
    /// <param name="tmpKeyValue">认证</param>
    public void GetJsonData(string tmpUrlI, string tmpKeyName, string tmpKeyValue)
    {
        //获取请求
        HttpWebRequest request = WebRequest.Create(tmpUrlI) as HttpWebRequest;
        //加入请求头
        request.Headers.Add(tmpKeyName, tmpKeyValue);
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {
            //获取响应数据流
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
            //获得json字符串
            string jsonstr = reader.ReadLine();
            JsonData jsonData = JsonMapper.ToObject(jsonstr);
            if (jsonData != null)
            {
                //取library下键值字典
                JsonData jsonDicList = jsonData["library"];
                foreach (JsonData item in jsonDicList)
                {
                    MaterialItem JO = new MaterialItem();
                    JO.materialManufacturer = item["materialManufacturer"].ToString();
                    JO.regularLabelling = item["regularLabelling"].ToString();
                    JO.sheetLabelling = item["sheetLabelling"].ToString();
                    //MaterialItem.materialList.Add(JO);
                }
            }
        }
    }

    [Button]
    void set()
    {
        string url = Application.dataPath + "/Resources/inputdate.json";
        
        GetJsonData(url);
    }

    public void GetJsonData(string tmpUrlI)
    {
        StreamReader reader = new StreamReader(tmpUrlI, Encoding.UTF8);
        Debug.Log(tmpUrlI);
        string jsonstr = reader.ReadToEnd();
        Debug.Log(jsonstr);
        JsonData jsonData = JsonMapper.ToObject(jsonstr);
        if (jsonData != null)
        {
            //取library下键值字典
            JsonData jsonDicList = jsonData["library"];
            foreach (JsonData item in jsonDicList)
            {
                MaterialItem JO = new MaterialItem();
                JO.materialManufacturer = item["materialManufacturer"].ToString();
                JO.regularLabelling = item["regularLabelling"].ToString();
                JO.sheetLabelling = item["sheetLabelling"].ToString();
                materialList.Add(JO);
            }
            Debug.Log(materialList.Count);
        }
    }
}
/*
{
	"library": 
	[{
		"materialManufacturer": "11",
		"regularLabelling": "",
		"sheetLabelling": ""
	},
	{
		"materialManufacturer": "fqwfewq",
		"regularLabelling": "",
		"sheetLabelling": ""
	}
	]
}



 */
