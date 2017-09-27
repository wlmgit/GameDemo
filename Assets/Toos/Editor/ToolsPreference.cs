using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Tool.Editor
{
    public class ToolsPreference : UnityEditor.Editor
    {

        private static string[] m_prefString;

        private static string[] PrefString
        {
            get
            {
                if (m_prefString == null)
                {
                    InitPrefString();
                }
                return m_prefString;
            }
        }

        private static void InitPrefString()
        {
            m_prefString = new string[1];
            for (int i = 0; i < m_prefString.Length; i++)
            {
                m_prefString[i] = string.Format("Tool{0}", (EToolPreference)i);
            }
        }

        public static void InitToolPref()
        {
            if (!EditorPrefs.HasKey(PrefString[0]))
            {
                EditorPrefs.SetBool( PrefString[0],true );
            }
        }

        public static void SetBool(EToolPreference pref,bool value)
        {
            EditorPrefs.SetBool(PrefString[(int)pref],value);
        }

        public static bool GetBool(EToolPreference pref)
        {
            return EditorPrefs.GetBool(PrefString[(int)pref]);
        }

        private static void DrawBoolPref(EToolPreference pref,string desTxt,ToolUtility.PreferenceChanged callback)
        {
            bool @bool = GetBool(pref);
            bool flag = GUILayout.Toggle(@bool,desTxt);
            if (flag != @bool)
            {
                SetBool(pref,flag);
                callback(pref,flag);
            }
        }

        public static void DrawPrefPanel(ToolUtility.PreferenceChanged callback)
        {
            DrawBoolPref(EToolPreference.ShowWelcomeScreen,"show welcom screen",callback);
        }
    }
}


