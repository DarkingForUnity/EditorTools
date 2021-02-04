//=====================================================
// - FileName:      SetMap.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================

using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using UnityEngine;

[Serializable]
public class GameStatus
{
    public string gameName;
    public string version;
    public bool isTure;
    public bool isUseHardWare;
    public List<refencenes> refencenes;
}

[Serializable]
public class refencenes
{
    public string name;
    public int id;
}


public class JasonTest : MonoBehaviour
{

    GameStatus _gameStatus = new GameStatus();

    GameStatus inputDate
    {
        get
        {
            return _gameStatus;
        }
        set
        {
            _gameStatus = value;
        }
    }

    string path;
    bool truename;

    void Start()
    {
        path = Application.dataPath + "/Resources/inputdate.json";

        if (LoadFromFile() != null)
            inputDate = LoadFromFile();

        foreach (var v in inputDate.refencenes)
        {
            Debug.Log(v.id + "   " + v.name);
        }
    }

    /// <summary>
    /// 读取Jason并返回该类型
    /// </summary>
    /// <returns></returns>
    GameStatus LoadFromFile()
    {
        if (!File.Exists(path))
            return null;

        StreamReader sr = new StreamReader(path);

        if (sr == null)
            return null;

        string json = sr.ReadToEnd();

        if (json.Length > 0)
            return JsonUtility.FromJson<GameStatus>(json);

        return null;
    }

    /// <summary>
    /// 退出时写入
    /// </summary>
    void OnApplicationQuit()
    {
        string json = JsonUtility.ToJson(inputDate, true);
        File.WriteAllText(path, json, Encoding.UTF8);
    }

    /// <summary>
    /// 更新值
    /// </summary>
    [Button]
    public void RangNumber()
    {
        path = Application.dataPath + "/Resources/inputdate.json";
        GameStatus ide = new GameStatus();
        

        if (truename)
            truename = false;
        else
            truename = true;

       
        List<refencenes> _refencenes=new List<refencenes>();
        refencenes refencenes_0 = new refencenes();
        refencenes_0.id = 0;
        refencenes_0.name = "DK";
        _refencenes.Add(refencenes_0);
        refencenes refencenes_1 = new refencenes();
        refencenes_1.id = 1;
        refencenes_1.name = "DK01";
        _refencenes.Add(refencenes_1);

        ide.gameName = truename ? "AdvancePikachu" : "进击的皮卡丘";
        ide.isTure = truename;
        ide.refencenes = _refencenes;
        Debug.Log(ide.gameName+ "    "+ide.isTure);
        inputDate = ide;

        string json = JsonUtility.ToJson(ide, true);
        File.WriteAllText(path, json, Encoding.UTF8);

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
