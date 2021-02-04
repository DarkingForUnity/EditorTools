//=====================================================
// - FileName:      DkWindow.cs
// - CreateTime:    #CreateTime#
// - Description:   地图编辑器 V1.0
//======================================================
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DkWindow : EditorWindow
{
    int select = 0;
    string[] labls = new string[4] { "创建地图", "编辑地图", "删除地图", "关于" };

    //生成对象的类型
    string[] options = { "Type_0", "Type_1", "Type_2" , "Type_3", "Type_4" };
    List<string> options_name=new List<string>();
    //存放当前配置对象
    int index = 0;
    List<Vector3> cells_positions = new List<Vector3>();
    //存放多选对象
    Transform[] transforms;
    //地图编辑对象的根节点
    private GameObject cell_root;
    //单个地图编辑对象
    private GameObject cell;
    //地图规格
    Vector2Int size;
    //存放不同对象的类型
    private bool foldoutType;
    private List<GameObject> objs = new List<GameObject>();
    //private GameObject obj_0;
    //private GameObject obj_1;
    //private GameObject obj_2;
    //private GameObject obj_3;
    //private GameObject obj_4;

    //获取对象类型的图片，进行预览
    private Sprite Sprite_;


    private Sprite Sprite_to_b;
    private Sprite Sprite_to_b_1;
    private Sprite Sprite_to_b_2;
    string[] set_model = { "auto", "manual"};
    int set_model_index=0;
    bool isEditor;
    GameObject root;
    string myString = "";
    public Rect windowRect = new Rect(0, 0, 200, 200);//子窗口的大小和位置
    string name_;
    [MenuItem("Tool_DK/MapEditor Control %M")]
    public static void Init()
    {
        DkWindow window = EditorWindow.GetWindow<DkWindow>("MapEditor");
        
        window.Show();
        window.minSize = new Vector2(200, 300);
    }
    void OnGUI()
    {
        
        //BeginWindows();//标记开始区域所有弹出式窗口
        //windowRect = GUILayout.Window(1, windowRect, DoWindow, "子窗口");//创建内联窗口,参数分别为id,大小位置，创建子窗口的组件的函数，标题
        //EndWindows();//标记结束
        
        GUILayout.Space(50);
        int select_index = select;
        select_index = GUILayout.Toolbar(select_index, labls, GUILayout.Height(50));//GUILayout.Width(200), 
        select = select_index;
        GUIStyle titleStyle = new GUIStyle();
        //titleStyle2.fontSize = 20;
        titleStyle.normal.textColor = new Color(1, 0, 0, 1);
        switch (select)
        {
            case 0://创建地图
                #region 创建地图
                GUILayout.Space(20);
                cell_root = EditorGUILayout.ObjectField("Cell节点对象", cell_root, typeof(GameObject), true) as GameObject;
                cell = EditorGUILayout.ObjectField("Cell", cell, typeof(GameObject), true) as GameObject;
                size = EditorGUILayout.Vector2IntField("地图规格", size);
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("生成新地图面", GUILayout.ExpandWidth(true)))
                {
                    while (cell_root.transform.childCount > 0)
                    {
                        DestroyImmediate(cell_root.transform.GetChild(0).gameObject);
                    }
                    //cell_root.GetComponent<UIGrid>().maxPerLine = size.x;
                    int num = size.x * size.y;
                    for (int i = 0; i < num; i++)
                    {
                        GameObject obj = GameObject.Instantiate(cell);
                        obj.name = cell.name;
                        obj.transform.parent = cell_root.transform;
                    }
                    //cell_root.GetComponent<UIGrid>().Reposition();
                }
                //GUILayout.Space(120);
                if (GUILayout.Button("删除地图面(结束配置后使用)", GUILayout.ExpandWidth(true)))
                {
                    while (cell_root.transform.childCount > 0)
                    {
                        DestroyImmediate(cell_root.transform.GetChild(0).gameObject);
                    }
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
                #endregion
                break;
            case 1://编辑地图
                #region 编辑地图
                GUILayout.Space(10);
                if (objs.Count == 0)
                { objs.Add(null);}

                GUILayout.BeginHorizontal();
                foldoutType = EditorGUILayout.Foldout(foldoutType, "单体组件（1*1单位）");
                if (GUILayout.Button("+", GUILayout.ExpandWidth(false)))
                {
                    //GameObject obj_ = null;
                    objs.Add(null);
                }
                GUILayout.EndHorizontal();
                
                if (foldoutType)
                {
                    for (int i = 0; i < objs.Count; i++)
                    {
                        GUILayout.BeginHorizontal();
                        objs[i] = EditorGUILayout.ObjectField("obj" + i, objs[i], typeof(GameObject), true) as GameObject;
                        
                        if (objs[i]==null)
                        {
                            GUILayout.Label("对象不能为空", titleStyle);
                        }

                        if (objs.IndexOf(objs[i]) != i&& objs[i]!= null)
                        {
                            GUILayout.Label("对象重复", titleStyle);
                        }

                        if (GUILayout.Button("-", GUILayout.ExpandWidth(false)))
                        {
                            objs.Remove(objs[i]);
                        }

                        GUILayout.EndHorizontal();
                    }
                }
                options_name.Clear();
                for (int i = 0; i < objs.Count; i++)
                {
                    if (i >= options_name.Count)
                    {
                        options_name.Add(null);
                    }

                    if (objs[i] != null)
                        options_name[i] = objs[i].name;
                    else
                        options_name[i] = "null";
                }

                string[] options_name_array = options_name.ToArray();

                GUILayout.Space(10);
                index = EditorGUILayout.Popup("配置类型选择", index,options_name_array);
                GUILayout.Space(10);
                Sprite sprite = null;
                if (index < objs.Count)
                {
                    if (objs[index])
                    {
                        if (objs[index].GetComponent<SpriteRenderer>())
                        { sprite = objs[index].GetComponent<SpriteRenderer>().sprite; }
                    }

                    GUILayout.Label("    当前配置 :" + options_name_array[index]);
                    Sprite_ = EditorGUILayout.ObjectField("预览", sprite, typeof(Sprite), false) as Sprite;
                    root = GameObject.Find("Root");
                    transforms = Selection.transforms;
                    GUILayout.Space(10);
                    set_model_index = EditorGUILayout.Popup("配置模式选择", set_model_index, set_model);
                    if (set_model_index == 1)
                    {
                        GUILayout.Space(10);
                        if (GUILayout.Button("Clone"))
                        {
                            int i = 0;
                            foreach (Transform T in transforms)
                            {
                                GameObject pre = null;
                                pre = objs[index];

                                GameObject obj = GameObject.Instantiate(pre);
                                obj.name = index.ToString();
                                obj.transform.parent = root.transform;
                                obj.transform.position = T.position;
                                obj.transform.tag = "Clone_cell";
                                i++;
                            }
                            Debug.Log("Clone Done ,All number is [" + i + "]");
                        }
                    }
                    GUILayout.Space(10);
                }
                else
                {

                    GUILayout.Label("  对象错误  ");
                }

                #endregion
                break;
            case 2://删除地图
                #region 删除地图
                set_model_index = EditorGUILayout.Popup("配置模式选择", set_model_index, set_model);
                if (GUILayout.Button("Delete All"))
                {
                    while (root.transform.childCount > 0)
                    {
                        DestroyImmediate(root.transform.GetChild(0).gameObject);
                    }
                }
                #endregion
                break;
            case 3://关于
                GUILayout.Label("DK_shuai");
                Sprite_to_b = EditorGUILayout.ObjectField(" ", Sprite_to_b, typeof(Sprite), true) as Sprite;
                Sprite_to_b_1 = EditorGUILayout.ObjectField(" ", Sprite_to_b_1, typeof(Sprite), true) as Sprite;
                Sprite_to_b_2 = EditorGUILayout.ObjectField(" ", Sprite_to_b_2, typeof(Sprite), true) as Sprite;
                GUILayout.Space(10);
                break;
        }
       
        //绘制当前正在编辑的场景
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 12;
        GUI.skin.label.alignment = TextAnchor.UpperLeft;
        GUILayout.Label("当前操作场景名称:" + EditorSceneManager.GetActiveScene().name);

        if (GUI.changed)
        {
            
        }

        #region
        ////绘制当前时间
        //GUILayout.Space(10);
        //GUILayout.Label("Time:" + System.DateTime.Now);

        ////绘制对象
        //GUILayout.Space(10);
        //buggyGameObject = (GameObject)EditorGUILayout.ObjectField("Buggy Game Object", buggyGameObject, typeof(GameObject), true);

        ////绘制描述文本区域
        //GUILayout.Space(10);
        //GUILayout.BeginHorizontal();
        //GUILayout.Label("Description", GUILayout.MaxWidth(80));
        //description = EditorGUILayout.TextArea(description, GUILayout.MaxHeight(75));
        //GUILayout.EndHorizontal();

        //EditorGUILayout.Space();

        ////添加名为"Save Bug"按钮，用于调用SaveBug()函数
        //if (GUILayout.Button("Save Bug"))
        //{
        //    SaveBug();
        //}

        ////添加名为"Save Bug with Screenshot"按钮，用于调用SaveBugWithScreenshot() 函数
        //if (GUILayout.Button("Save Bug With Screenshot"))
        //{
        //    SaveBugWithScreenshot();
        //}
        #endregion
    }

    void DoWindow(int unusedWindowID)
    {
        GUILayout.Button("按钮");//创建button
        GUI.DragWindow();//画出子窗口
    }

    #region
    //void SaveBug()
    //{
    //    Directory.CreateDirectory("Assets/BugReports/" + bugReporterName);
    //    StreamWriter sw = new StreamWriter("Assets/BugReports/" + bugReporterName + "/" + bugReporterName + ".txt");
    //    sw.WriteLine(bugReporterName);
    //    sw.WriteLine(System.DateTime.Now.ToString());
    //    sw.WriteLine(EditorSceneManager.GetActiveScene().name);
    //    sw.WriteLine(description);
    //    sw.Close();
    //}

    //void SaveBugWithScreenshot()
    //{
    //    Directory.CreateDirectory("Assets/BugReports/" + bugReporterName);
    //    StreamWriter sw = new StreamWriter("Assets/BugReports/" + bugReporterName + "/" + bugReporterName + ".txt");
    //    sw.WriteLine(bugReporterName);
    //    sw.WriteLine(System.DateTime.Now.ToString());
    //    sw.WriteLine(EditorSceneManager.GetActiveScene().name);
    //    sw.WriteLine(description);
    //    sw.Close();
    //    ScreenCapture.CaptureScreenshot("Assets/BugReports/" + bugReporterName + "/" + bugReporterName + "Screenshot.png");
    //}
    #endregion

    #region 窗体事件调用
    /// <summary>
    /// 面板打开
    /// </summary>
    private void OnEnable()
    {
        //Debug.Log("当面板打开时调用");
    }
    /// <summary>
    /// 当场景改变时调用
    /// </summary>
    private void OnProjectChange()
    {
        //Debug.Log("当场景改变时调用");
    }
    /// <summary>
    /// 当选择对象属性改变时调用
    /// </summary>
    private void OnHierarchyChange()
    {
        //Debug.Log("当选择对象属性改变时调用");
    }
    /// <summary>
    /// 当窗口得到焦点时调用
    /// </summary>
    void OnGetFocus()
    {
        //Debug.Log("当窗口得到焦点时调用");
    }
    /// <summary>
    /// 当窗口失去焦点时调用
    /// </summary>
    private void OnLostFocus()
    {
        //Debug.Log("当窗口失去焦点时调用");
    }
    /// <summary>
    /// 当改变选择不同对象时调用
    /// </summary>
    private void OnSelectionChange()
    {
        if (Selection.activeGameObject != null)
        {
            if (set_model_index==0 && select == 1&& Selection.activeGameObject.name == "cell")
            {
                //Debug.Log(Selection.activeGameObject.name);
                GameObject pre = null;
                pre = objs[index];
                GameObject obj = GameObject.Instantiate(pre);
                obj.name = index.ToString();
                obj.transform.parent = root.transform;
                obj.transform.position = Selection.activeGameObject.transform.position;
                obj.transform.tag = "Clone_cell";
                Debug.Log(obj.name);
            }

            if (set_model_index == 0 && select == 2 && (Selection.activeGameObject.tag == "Clone_cell"|| Selection.activeGameObject.transform.parent.tag == "Clone_cell"))
            {
                if(Selection.activeGameObject.tag == "Clone_cell")
                DestroyImmediate(Selection.activeGameObject);
                if (Selection.activeGameObject.transform.parent.tag == "Clone_cell")
                { DestroyImmediate(Selection.activeGameObject.transform.parent.gameObject); }
            }
        }
        
        //Debug.Log("当改变选择不同对象时调用");

    }
    /// <summary>
    /// 监视面板调用
    /// </summary>
    private void OnInspectorUpdate()
    {
        //Repaint();//重绘
        // Debug.Log("监视面板调用");
    }
    /// <summary>
    /// 当窗口关闭时调用
    /// </summary>
    private void OnDestroy()
    {
        //Debug.Log("当窗口关闭时调用");
    }
    /// <summary>
    /// 当窗口获取键盘焦点时调用
    /// </summary>
    private void OnFocus()
    {
        //Debug.Log("当窗口获取键盘焦点时调用");
    }
    #endregion
}
