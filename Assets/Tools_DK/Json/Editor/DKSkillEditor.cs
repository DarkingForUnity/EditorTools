//=====================================================
// - FileName:      DKSkillEditor.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class DKSkillEditor : EditorWindow
{
    new string name;
    Sprite sprite_skill;
    int level;
    float cd_time;
    string describe;
    float hit_rate;
    float damage;
    KeyCode key;

    Sprite sprite_skill_;
    int skill_id;
    private Rect windowRect = new Rect(new Vector2(0, 0), new Vector2(200, 100));

    Skills skills_ = new Skills();
    bool show_window = false;
    string tmpUrlI;

    private Vector2 m_ScrollPosition;

    [MenuItem("Tool_DK/DKSkillEditor")]
    public static void Init()
    {
        DKSkillEditor window = EditorWindow.GetWindow<DKSkillEditor>("Skill Editor Window");
        window.Show();
        window.minSize = new Vector2(200, 300);
    }
    void OnGUI()
    {
        tmpUrlI = Application.dataPath + "/Tools_DK/Json/inputdate.json";
        getdata();
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();
        #region
        EditorGUILayout.LabelField("技能名称");
        name = EditorGUILayout.TextField(name);
        EditorGUILayout.LabelField("技能图标");
        sprite_skill = EditorGUILayout.ObjectField(" ", sprite_skill, typeof(Sprite), true) as Sprite;
        EditorGUILayout.LabelField("描述");
        describe = EditorGUILayout.TextArea(describe, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("使用等级");
        level = EditorGUILayout.IntField(level);
        EditorGUILayout.LabelField("冷却时间");
        cd_time = EditorGUILayout.FloatField(cd_time);
        EditorGUILayout.LabelField("命中率");
        hit_rate = EditorGUILayout.FloatField(hit_rate);
        EditorGUILayout.LabelField("伤害量");
        damage = EditorGUILayout.FloatField(damage);
        EditorGUILayout.LabelField("触发按键");
        key = (KeyCode)EditorGUILayout.EnumPopup(key);
        GUILayout.Space(20);
        GUI.color = Color.green;
        if (GUILayout.Button("UpDate"))
        {
            setJsonData(skill_id);
        }
        GUI.color = Color.red;
        if (GUILayout.Button("Delete"))
        {
            deleteData(skill_id);
        }
        #endregion

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical();
        m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition);
        //scrollView
        if (skills_._skills.Count > 0)
        {
            foreach (Skill sk in skills_._skills)
            {

                EditorGUILayout.BeginHorizontal();
                if (skill_id == sk.id)
                {
                    GUI.color = Color.green;
                }
                else
                {
                    GUI.color = Color.white;
                }
                
                Sprite sprite_skill = EditorGUILayout.
                    ObjectField(sk.id.ToString(), sk.sprite_skill, typeof(Sprite), true) as Sprite;
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("ID:" + sk.id);
                GUILayout.Label("技能名称:" + sk.name);
                GUILayout.Label("技能描述:" + sk.describe);
                GUILayout.Label("使用等级:" + sk.level);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("冷却时间:" + sk.cd_time);
                GUILayout.Label("命中率:" + sk.hit_rate);
                GUILayout.Label("伤害量:" + sk.damage);
                GUILayout.Label("触发按键:" + sk.key);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();

                if (GUILayout.Button("编辑"))
                {
                    getbuttonskill(sk);
                }


                EditorGUILayout.EndHorizontal();
            }
        }
        EditorGUILayout.EndScrollView();
        if (GUILayout.Button("+"))
        {
            show_window = true;
            clean();
            windowRect = new Rect(new Vector2(400, 400), new Vector2(200, 400));
        }
        if (show_window)
        {
            BeginWindows();//标记开始区域所有弹出式窗口
            windowRect = GUILayout.Window(0, windowRect, DoWindow, "添加新技能");//创建内联窗口,参数分别为id,大小位置，创建子窗口的组件的函数，标题
            EndWindows();//标记结束
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

    }
    /// <summary>
    /// 添加技能面板
    /// </summary>
    /// <param name="unusedWindowID"></param>
    void DoWindow(int unusedWindowID)
    {
        EditorGUILayout.LabelField("技能名称");
        name = EditorGUILayout.TextField(name);
        EditorGUILayout.LabelField("技能图标");
        sprite_skill = EditorGUILayout.ObjectField(" ", sprite_skill, typeof(Sprite), true) as Sprite;
        EditorGUILayout.LabelField("描述");
        describe = EditorGUILayout.TextArea(describe, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(true));
        EditorGUILayout.LabelField("使用等级");
        level = EditorGUILayout.IntField(level);
        EditorGUILayout.LabelField("冷却时间");
        cd_time = EditorGUILayout.FloatField(cd_time);
        EditorGUILayout.LabelField("命中率");
        hit_rate = EditorGUILayout.FloatField(hit_rate);
        EditorGUILayout.LabelField("伤害量");
        damage = EditorGUILayout.FloatField(damage);
        EditorGUILayout.LabelField("触发按键");
        key = (KeyCode)EditorGUILayout.EnumPopup(key);
        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("X"))
        {
            clean();
            show_window = false;
        }
        if (GUILayout.Button("Add"))
        {
            setJsonData();
        }
        EditorGUILayout.EndHorizontal();
        GUI.DragWindow();//画出子窗口
    }
    /// <summary>
    /// 面板数据清空
    /// </summary>
    void clean()
    {
        name = "";
        level = 0;
        cd_time = 0;
        describe = "";
        hit_rate = 0;
        damage = 0;
        key = KeyCode.A;
    }
    /// <summary>
    /// 获取技能Json数据
    /// </summary>
    void getdata()
    {

        StreamReader sr = new StreamReader(tmpUrlI, Encoding.UTF8);
        string json = sr.ReadToEnd();
        sr.Close();
        if (json.Length > 0)
        {
            skills_ = JsonUtility.FromJson<Skills>(json);
        }
        // sprite_skill_ = EditorGUILayout.ObjectField(" ", skills_._skills[1].sprite_skill, typeof(Sprite), true) as Sprite;

    }
    /// <summary>
    ///编辑按钮读取该技能信息
    /// </summary>
    /// <param name="sk"></param>
    void getbuttonskill(Skill sk)
    {
        skill_id = sk.id;
        name = sk.name;
        sprite_skill = sk.sprite_skill;
        describe = sk.describe;
        level = sk.level;
        cd_time = sk.cd_time;
        hit_rate = sk.hit_rate;
        damage = sk.damage;
        key = sk.key;
    }
    /// <summary>
    /// 添加技能
    /// </summary>
    public void setJsonData()
    {

        StreamReader sr = new StreamReader(tmpUrlI, Encoding.UTF8);

        string json = sr.ReadToEnd();

        sr.Close();

        Skills skills = new Skills();

        if (json.Length > 0)
        {
            skills = JsonUtility.FromJson<Skills>(json);
        }


        List<Skill> prive_skills = new List<Skill>();
        if (skills._skills != null)
            prive_skills = skills._skills;

        Skill skill = new Skill();
        skill.id = skills.Total;
        skill.name = name;
        skill.sprite_skill = sprite_skill;
        skill.describe = describe;
        skill.level = level;
        skill.cd_time = cd_time;
        skill.hit_rate = hit_rate;
        skill.damage = damage;
        skill.key = key;
        prive_skills.Add(skill);

        skills._skills = prive_skills;
        skills.name = "Skill Data";
        //用于生成唯一ID
        skills.Total += 1;
        string json_ = JsonUtility.ToJson(skills, true);
        File.WriteAllText(tmpUrlI, json_, Encoding.UTF8);

    }
    /// <summary>
    /// 编辑技能
    /// </summary>
    /// <param name="id"></param>
    public void setJsonData(int id)
    {
        StreamReader sr = new StreamReader(tmpUrlI, Encoding.UTF8);
        string json = sr.ReadToEnd();
        sr.Close();
        Skills skills = new Skills();
        if (json.Length > 0)
        {
            skills = JsonUtility.FromJson<Skills>(json);
        }
        Skill skill = new Skill();
        for (int i = 0; i < skills._skills.Count; i++)
        {
            if (skills._skills[i].id == id)
            {
                skill = skills._skills[i];
            }
        }
        skill.name = name;
        skill.sprite_skill = sprite_skill;
        skill.describe = describe;
        skill.level = level;
        skill.cd_time = cd_time;
        skill.hit_rate = hit_rate;
        skill.damage = damage;
        skill.key = key;
        
        string json_ = JsonUtility.ToJson(skills, true);
        File.WriteAllText(tmpUrlI, json_, Encoding.UTF8);

    }
    /// <summary>
    /// 删除技能
    /// </summary>
    /// <param name="id"></param>
    public void deleteData(int id)
    {

        StreamReader sr = new StreamReader(tmpUrlI, Encoding.UTF8);
        string json = sr.ReadToEnd();

        sr.Close();
        Skills skills = new Skills();

        if (json.Length > 0)
        {
            skills = JsonUtility.FromJson<Skills>(json);
            for (int i = 0; i < skills._skills.Count; i++)
            {
                if (skills._skills[i].id == id)
                {
                    skills._skills.Remove(skills._skills[i]);
                }
            }
            string json_ = JsonUtility.ToJson(skills, true);
            File.WriteAllText(tmpUrlI, json_, Encoding.UTF8);
        }
    }
    private void OnInspectorUpdate()
    {
        Repaint();//重绘
        // Debug.Log("监视面板调用");
    }
}
