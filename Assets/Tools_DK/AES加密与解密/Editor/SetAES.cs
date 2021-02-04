//=====================================================
// - FileName:      SetAES.cs
// - CreateTime:    #CreateTime#
// - Description:   脚本描述 
//======================================================
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SetAES : EditorWindow
{
    private static SetAES window;

    string Clear_Text;
    string Cipher_Text;

    [MenuItem("Tool_DK/SetAES")]
    public static void Init()
    {
        window = (SetAES)EditorWindow.GetWindow(typeof(SetAES), false, "AES 加密解密");
        window.Show();
    }

    private void OnGUI()
    {
        //GUILayout.BeginArea(new Rect(new Vector2(0,0),new Vector2(200,100)), "HelpBox");
        //GUILayout.EndArea();
        
        Clear_Text = GUILayout.TextArea("明文", Clear_Text);
        Cipher_Text = GUILayout.TextArea("密文", Cipher_Text);
        
    }
}
