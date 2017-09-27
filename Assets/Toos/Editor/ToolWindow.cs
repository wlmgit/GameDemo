using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Tool.Editor
{
    public class ToolWindow : EditorWindow
    {
        private static Vector2 m_windowSize = new Vector2(800,600);

        private static Rect m_fileToolbarRect;
        private static Rect m_preferencePanelRect;

        private bool m_showPreference;

        [MenuItem("Tool/Tool Name/Editor")]
        public static void ShowWindow()
        {
            ToolWindow window = EditorWindow.GetWindow<ToolWindow>(false,"ToolEditor");

            window.maxSize = m_windowSize;
            window.minSize = m_windowSize;

            ToolsPreference.InitToolPref();
            if (ToolsPreference.GetBool(EToolPreference.ShowWelcomeScreen))
            {
                WelcomeScreen.ShowWindow();
            }
        }

        public void OnGUI()
        {
            SetupSize();
            if ( Draw() )
            {

            }
            DrawToolPreference();
        }

        private void SetupSize()
        {
            float width = base.position.width;
            m_fileToolbarRect = new Rect(0,0,width,18);
            m_preferencePanelRect = new Rect(width - 290,20,290,50);
        }
        private bool Draw()
        {
            bool result = false;
            DrawFileToolbar();
            return result;
        }
        private void DrawFileToolbar()
        {
            GUILayout.BeginArea(m_fileToolbarRect,EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Preference",(!m_showPreference) ? EditorStyles.toolbarButton : ToolUtility.ToolbarSelectStyle ,GUILayout.Width(80f)))
            {
                m_showPreference = !m_showPreference;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawToolPreference()
        {
            if (m_showPreference)
            {
                GUILayout.BeginArea(m_preferencePanelRect,ToolUtility.ToolPreferencePanelStyle);
                ToolsPreference.DrawPrefPanel(new ToolUtility.PreferenceChanged(PrefChanged));
                GUILayout.EndArea();
            }
        }

        private void PrefChanged( EToolPreference pref, System.Object value)
        {
            Debug.Log(value.ToString());
        }
    }
}

