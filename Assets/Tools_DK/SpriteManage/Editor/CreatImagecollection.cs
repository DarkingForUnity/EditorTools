//=====================================================
/*
 * 主要功能：
 *     1、
 *     统计场景中已引用的图片存放文件夹；
 *     
 *     统计已引用图集
 *     2、
 *     根据图集显示场景中对象
 *     3、
 *      在App/Image collection/文件夹下添加场景名字命名的文件夹
 *      将场景内引用的所有图片复制过去
 *      改变对象引用
 */
//======================================================
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class CreatImagecollection : EditorWindow
{
    private static CreatImagecollection window;
    string prjPath;
    int select = 0;
    string[] labls = new string[3] { "统计", "详情", "迁移（慎用）"};
    List<GameObject> objs = new List<GameObject>();
    List<Sprite> sprites = new List<Sprite>();
    List<string> paths_ = new List<string>();
    List<string> Imagecollections_ = new List<string>();
    List<bool> Show_Imagecollections_ = new List<bool>();
    private Vector2 m_ScrollPosition;

    bool show_path=true;
    bool show_file = false;
    bool show_Packname = true;

    GameObject _obj_;
    int success=0;
    int fail = 0;
    [MenuItem("Tool_DK/SpriteManage/CreatImagecollection")]
    public static void Init()
    {
        window = (CreatImagecollection)EditorWindow.GetWindow(typeof(CreatImagecollection), false, "创建图片图集");
        window.Show();
    }

    void OnGUI()
    {
        if (select == 2)
        {
            GUI.color = Color.red;
        }
        else
        {
            GUI.color = Color.white;
        }
        labls[1] = "详情   Sprite对象总数" + objs.Count;
        select = GUILayout.Toolbar(select, labls, GUILayout.Height(50));//GUILayout.Width(200),
        GUI.color = Color.white;
        GUILayout.Space(10);
        switch (select)
        {
            case 0:
                if (paths_.Count > 0)
                {
                    EditorGUILayout.LabelField("场景中引用的文件地址：");
                    GUILayout.Space(10);
                    foreach (string str in paths_)
                    {
                        EditorGUILayout.LabelField(str);
                    }
                    GUILayout.Space(20);
                    EditorGUILayout.LabelField("已引用图集：");
                    GUILayout.Space(10);
                    foreach (string str in Imagecollections_)
                    {
                        if (str.Length != 0)
                        { EditorGUILayout.LabelField(str); }
                        else
                        {
                            GUIStyle Style_ = new GUIStyle();
                            Style_.normal.textColor = Color.red;
                            EditorGUILayout.LabelField("存在未打包图片", Style_);
                        }
                        
                    }
                }
                break;
            case 1:
                EditorGUILayout.BeginHorizontal("HelpBox");
                if (show_file)
                {
                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.LabelField("文件名地址");
                    EditorGUILayout.LabelField("----------------------------------------");
                    EditorGUILayout.EndVertical();
                }
                if (show_path)
                {
                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.LabelField("文件存放地址");
                    EditorGUILayout.LabelField("----------------------------------------");
                    EditorGUILayout.EndVertical();
                }
                if (show_Packname)
                {
                    EditorGUILayout.BeginVertical("HelpBox");
                    EditorGUILayout.LabelField("图集名称");
                    EditorGUILayout.LabelField("----------------------------------------");
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.BeginVertical("HelpBox");
                EditorGUILayout.LabelField("Sprite对象");
                EditorGUILayout.LabelField("----------------------------------------");
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
                m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
                for (int i = 0; i < objs.Count; i++)
                {
                    if (objs[i].GetComponent<SpriteRenderer>())
                    {
                        string path_whole = AssetDatabase.GetAssetPath(objs[i].GetComponent<SpriteRenderer>().sprite);
                        TextureImporter importer = AssetImporter.GetAtPath(path_whole) as TextureImporter;
                        if(importer)
                        if (Show_Imagecollections_[Imagecollections_.IndexOf(importer.spritePackingTag)])
                        {
                            EditorGUILayout.BeginHorizontal("HelpBox");
                            if (show_file)
                            {
                                EditorGUILayout.LabelField(path_whole);
                            }
                            string path_ = path_whole.Substring(0, path_whole.LastIndexOf("/"));
                            if (show_path)
                            {
                                EditorGUILayout.LabelField(path_);
                            }
                            if (show_Packname)
                            {
                                    if (importer.spritePackingTag == "")
                                    { EditorGUILayout.LabelField("未打包"); }
                                    else
                                    { EditorGUILayout.LabelField(importer.spritePackingTag); }
                            }

                            _obj_ = EditorGUILayout.ObjectField(objs[i], typeof(GameObject), true) as GameObject;
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                
                EditorGUILayout.EndScrollView();
                GUILayout.Space(10);
                EditorGUILayout.LabelField("----------------------------------------");
                EditorGUILayout.LabelField("选择显示图集");
                GUILayout.Space(10);
                for (int i = 0; i < Imagecollections_.Count; i++)
                {
                    if (Imagecollections_[i] == "")
                    { Show_Imagecollections_[i] = EditorGUILayout.Toggle("未打包", Show_Imagecollections_[i]); }
                    else
                    { Show_Imagecollections_[i] = EditorGUILayout.Toggle(Imagecollections_[i], Show_Imagecollections_[i]); }
                        
                }
                EditorGUILayout.LabelField("----------------------------------------");
                GUILayout.Space(10);
                show_file = EditorGUILayout.Toggle("是否显示文件名", show_file);
                show_path = EditorGUILayout.Toggle("是否显示路径",show_path);
                show_Packname = EditorGUILayout.Toggle("是否显示已打包名称", show_Packname);

                break;
            case 2:
                
                GUIStyle Style = new GUIStyle();
                //Style.normal.background = Color.yellow; //这是设置背景填充的
                Style.normal.textColor = Color.red;
                Style.fontSize = 15;
                EditorGUILayout.LabelField("该模块功能可以将场景内使用的所有图片资源复制到‘App/Image collection/+场景名’的文件夹中", Style);
                EditorGUILayout.LabelField("并修改场景图片中的引用地址 ", Style);
                GUILayout.Space(10);
                prjPath = Application.dataPath + "/App/Image collection/" + EditorSceneManager.GetActiveScene().name;
                DirectoryInfo mydir = new DirectoryInfo(prjPath);
                if (!mydir.Exists)
                {
                    if (GUILayout.Button("创建场景图片文件夹"))
                    {
                        Directory.CreateDirectory(prjPath);
                        AssetDatabase.Refresh();
                        Debug.Log("场景图片文件夹创建完成");
                    }
                }
                else
                {
                    EditorGUILayout.LabelField("场景文件夹已存在");
                }
                EditorGUILayout.LabelField("Clone成功数量" + success);
                EditorGUILayout.LabelField("Clone失败数量" + fail);
                
                if (GUILayout.Button("Clone"))
                {
                    write();
                }
                break;
        }
        if (GUILayout.Button("搜索场景所有图片对象"))
        {
            check();
        }
    }

    void check()
    {
        SpriteRenderer[] sprite = Resources.FindObjectsOfTypeAll(typeof(SpriteRenderer)) as SpriteRenderer[];
        Debug.Log(sprite.Length);
        success = 0;
        fail = 0;
        objs.Clear();
        paths_.Clear();
        sprites.Clear();
        for (int i = 0; i < sprite.Length; i++)
        {
            EditorUtility.DisplayProgressBar("图片整理中", sprite[i].name, (float)i / (float)sprite.Length);
            if (sprite[i].sprite != null)
            {

                string path_whole = AssetDatabase.GetAssetPath(sprite[i].sprite);
                string path_ = path_whole.Substring(0, path_whole.LastIndexOf("/"));
                if (!paths_.Contains(path_))
                {
                    paths_.Add(path_);
                }
                TextureImporter importer = AssetImporter.GetAtPath(path_whole) as TextureImporter;
                if(importer)
                {
                    if (importer.spritePackingTag != null)
                    {
                        if (!Imagecollections_.Contains(importer.spritePackingTag))
                        {

                            Imagecollections_.Add(importer.spritePackingTag);
                            Show_Imagecollections_.Add(false);
                        }
                    }
                }
                objs.Add(sprite[i].gameObject);
            }

        }
        Debug.Log("完成检索");
        EditorUtility.ClearProgressBar();
    }

    void write()
    {
        for (int i = 0; i < objs.Count; i++)
        {
            string path_1 = AssetDatabase.GetAssetPath(objs[i].GetComponent<SpriteRenderer>().sprite);
            string strFileName = path_1.Substring(path_1.LastIndexOf("/") + 1, path_1.Length - path_1.LastIndexOf("/") - 1);
            string path = "Assets/App/Image collection/" + EditorSceneManager.GetActiveScene().name + "/" + strFileName;
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
            if (importer)
            {
                EditorUtility.DisplayProgressBar("图片复制中", objs[i].name, (float)i / (float)objs.Count);
                string path_2 = "Assets/App/Image collection/" + EditorSceneManager.GetActiveScene().name + "/" + strFileName;

                if (path_1 != path_2)
                {
                    success++;
                    File.Copy(path_1, prjPath + "/" + strFileName, true);
                }
                else
                {
                    fail++;
                }

                AssetDatabase.Refresh();
                //获取设置
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                Sprite Sprite_new = AssetDatabase.LoadAssetAtPath<Sprite>(path);
                objs[i].GetComponent<SpriteRenderer>().sprite = Sprite_new;
            }
            

        }
        EditorUtility.ClearProgressBar();

    }

    private void OnInspectorUpdate()
    {
        Repaint();//重绘
        // Debug.Log("监视面板调用");
    }
    
}
