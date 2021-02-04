//=====================================================
// - FileName:      skill_.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skills
{
    public string name;
    public int Total;
    public List<Skill> _skills;
}
[Serializable]
public class Skill
{
    //技能ID
    public int id;
    //技能名称
    public string name;
    //技能图标
    public Sprite sprite_skill;
    //技能描述
    public string describe;
    //技能等级
    public int level;
    //冷却时间
    public float cd_time;
    //命中率
    public float hit_rate;
    //伤害量
    public float damage;
    //触发按键
    public KeyCode key;
}
