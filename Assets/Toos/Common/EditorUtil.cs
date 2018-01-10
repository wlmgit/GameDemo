using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EditorUtil{

    //filePath是整个文件路径（包含文件名字），返回的是文件名字（保含格式）
    public static string GetSelectFileName(ref string filePath)
    {
        filePath = AssetDatabase.GetAssetPath( Selection.activeInstanceID );
        if ( filePath == "" )
        {
            return "";
        }
        return Path.GetFileName(filePath);
    }

}
