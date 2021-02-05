//=====================================================
// - FileName:      setBezierLine.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class setBezierLine : EditorWindow
{
    private static setBezierLine window;
    GameObject tag0;
    GameObject tag1;
    GameObject tag2;
    int number;
    LineRenderer line;
    
    [MenuItem("Tool_DK/setBezierLine")]
    public static void Init()
    {
        window = (setBezierLine)EditorWindow.GetWindow(typeof(setBezierLine), false, "set Bezier Line");
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.LabelField("起点");
        tag0 = EditorGUILayout.ObjectField(tag0, typeof(GameObject), true) as GameObject;
        EditorGUILayout.LabelField("影响点");
        tag1 = EditorGUILayout.ObjectField(tag1, typeof(GameObject), true) as GameObject;
        EditorGUILayout.LabelField("终点");
        tag2 = EditorGUILayout.ObjectField(tag2, typeof(GameObject), true) as GameObject;
        EditorGUILayout.LabelField("线");
        line = EditorGUILayout.ObjectField(line, typeof(LineRenderer), true) as LineRenderer;

        number = EditorGUILayout.IntField("分段数量", number);
        if (number > 0)
        { getBezier(number); }
        
        if (GUILayout.Button("start"))
        {
            getBezier(number);
        }
    }

    void getBezier(int num)
    {
        line.positionCount = 0;
        Vector3[] vector3 = new Vector3[num+1];
        for (int i = 0; i <= num; i++)
        {
            float value = ((float)i / (float)num);
            vector3[i]= (1 - value * value) * tag0.transform.position + 2 * value * (1 - value) * tag1.transform.position + value * value * tag2.transform.position;
            //Debug.Log(vector3[i]+"   "+ value.ToString("")+"  "+ num+ "  " + i);
        }
        line.positionCount = vector3.Length;
        line.SetPositions(vector3);
        for (int i = 0; i < vector3.Length; i++)
        {
            line.SetPosition(i, vector3[i]);
        }
        
    }
    private void OnInspectorUpdate()
    {
        Repaint();//重绘
        // Debug.Log("监视面板调用");
    }
}
