//=====================================================
// - Description:   查找空精灵图片以及未激活的图片
//======================================================
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CheckEmptySprite : EditorWindow
{
    List<SpriteRenderer> Empty_sprite = new List<SpriteRenderer>();
    List<SpriteRenderer> Disenable_sprite = new List<SpriteRenderer>();
    List<SpriteRenderer> Disenable_obj = new List<SpriteRenderer>();
    private Vector2 m_ScrollPosition;
    private Vector2 m_ScrollPosition_0;
    private Vector2 m_ScrollPosition_1;
    [MenuItem("Tool_DK/SpriteManage/CheckEmptySprite")]
    public static void Openwindows()
    {
        EditorWindow window = GetWindow(typeof(CheckEmptySprite));
        window.minSize = new Vector2(300, 330);
        //window.maxSize = new Vector2(420, 330);
        window.Show();
    }
    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal("HelpBox");

        EditorGUILayout.BeginVertical("HelpBox");
        GUILayout.Space(20);
        GUILayout.Label("图片为空:");
        GUILayout.Space(20);
        m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
        if (Empty_sprite.Count > 0)
        {
            for (int i = 0; i < Empty_sprite.Count; i++)
            {
                GameObject obj = EditorGUILayout.ObjectField(Empty_sprite[i], typeof(GameObject), true) as GameObject;
            }

        }
        EditorGUILayout.EndScrollView();
        GUI.color = Color.red;
        if (GUILayout.Button("清除组件（不可逆）"))
        {
            remove_sprite_Component(Empty_sprite);
        }
        if (GUILayout.Button("清除对象（不可逆）"))
        {
            Delete(Empty_sprite);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        GUILayout.Space(20);
        GUILayout.Label("图片组件未激活:");
        GUILayout.Space(20);
        m_ScrollPosition_0 = EditorGUILayout.BeginScrollView(m_ScrollPosition_0);
        if (Disenable_sprite.Count > 0)
        {
            for (int i = 0; i < Disenable_sprite.Count; i++)
            {
                GameObject obj = EditorGUILayout.ObjectField(Disenable_sprite[i], typeof(GameObject), true) as GameObject;
            }

        }
        EditorGUILayout.EndScrollView();
        GUI.color = Color.red;
        if (GUILayout.Button("清除组件（不可逆）"))
        {
            remove_sprite_Component(Disenable_sprite);
        }
        if (GUILayout.Button("清除对象（不可逆）"))
        {
            Delete(Disenable_sprite);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("HelpBox");
        GUILayout.Space(20);
        GUILayout.Label("对象未激活:");
        GUILayout.Space(20);
        m_ScrollPosition_1 = EditorGUILayout.BeginScrollView(m_ScrollPosition_1);
        if (Disenable_obj.Count > 0)
        {
            for (int i = 0; i < Disenable_obj.Count; i++)
            {
                GameObject obj = EditorGUILayout.ObjectField(Disenable_obj[i], typeof(GameObject), true) as GameObject;
            }

        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("搜索"))
        {
            check();
        }
    }

    void check()
    {
        SpriteRenderer[] sprite = Resources.FindObjectsOfTypeAll(typeof(SpriteRenderer)) as SpriteRenderer[];
        Debug.Log(sprite.Length);
        Empty_sprite.Clear();
        Disenable_sprite.Clear();
        Disenable_obj.Clear();

        for (int i = 0; i < sprite.Length; i++)
        {
            EditorUtility.DisplayProgressBar("空图片整理中", sprite[i].name, (float)i / (float)sprite.Length);
            if (sprite[i].sprite == null)
            {
                Empty_sprite.Add(sprite[i]);
            }
            if (sprite[i].enabled == false)
            {
                Disenable_sprite.Add(sprite[i]);
            }
            if (sprite[i].gameObject.activeSelf == false)
            {
                Disenable_obj.Add(sprite[i]);
            }
        }
        Debug.Log("完成引用检索");
        EditorUtility.ClearProgressBar();
    }

    void remove_sprite_Component(List<SpriteRenderer> _sprite)
    {
        for (int i = 0; i < _sprite.Count; i++)
        {
            EditorUtility.DisplayProgressBar("图片组件清理中", _sprite[i].name, (float)i / (float)_sprite.Count);
            DestroyImmediate(_sprite[i].gameObject.transform.GetComponent<SpriteRenderer>());
        }
        Debug.Log("完成图片组件清理");
        EditorUtility.ClearProgressBar();
        check();
    }

    void Delete(List<SpriteRenderer> _sprite)
    {
        for (int i = 0; i < _sprite.Count; i++)
        {

            if (_sprite[i])
            {
                EditorUtility.DisplayProgressBar("对象清理中", _sprite[i].name, (float)i / (float)_sprite.Count);
                DestroyImmediate(_sprite[i].gameObject);
            }
                
        }
        Debug.Log("完成对象清理");
        EditorUtility.ClearProgressBar();
        check();
    }

    private void OnInspectorUpdate()
    {
        Repaint();//重绘
        // Debug.Log("监视面板调用");
    }
}
