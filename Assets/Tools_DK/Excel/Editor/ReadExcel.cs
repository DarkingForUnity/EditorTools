//=====================================================
// - FileName:      ReadExcel.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ReadExcel : EditorWindow
{
    private static ReadExcel window;
    List<DepenceTableData> listdata=new List<DepenceTableData>();
    List<DepenceTableData> listdata_towrite = new List<DepenceTableData>();
    [MenuItem("Tool_DK/ReadExcel")]
    public static void Init()
    {
        window = (ReadExcel)EditorWindow.GetWindow(typeof(ReadExcel), false, "Read Excel");
        window.Show();
    }

    private void OnGUI()
    {
        if (listdata.Count > 0)
        {
            for(int i=0;i< listdata.Count;i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.TextField(listdata[i].instruct);
                EditorGUILayout.TextField(listdata[i].word);
                EditorGUILayout.EndHorizontal();
            }
        }
        if (GUILayout.Button("获取表格数据"))
        {
            read();
        }
    }

    void read()
    {
        listdata = DoExcel.Load(Application.dataPath + "/Tools_DK/Excel/Data/" + "test.xlsx");
    }
}
