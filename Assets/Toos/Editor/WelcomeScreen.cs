using UnityEngine;
using System.Collections;
using UnityEditor;
namespace Tool.Editor
{
    public class WelcomeScreen : EditorWindow
    {

        private static Rect m_ToggleButtonRect = new Rect(10, 170, 200, 30);

        [MenuItem("Tool/Tool Name/WelcomeScreen")]
        public static void ShowWindow()
        {
            WelcomeScreen window = EditorWindow.GetWindow<WelcomeScreen>(true, "Welcom to ...");
            Vector2 vector = new Vector2(300, 200);
            window.maxSize = vector;
            window.minSize = vector;
        }

        public void OnEnable()
        {

        }

        public void OnGUI()
        {
            bool flag = GUI.Toggle(m_ToggleButtonRect,ToolsPreference.GetBool(EToolPreference.ShowWelcomeScreen),"Show at Startup");

            if (flag != ToolsPreference.GetBool(EToolPreference.ShowWelcomeScreen))
            {
                ToolsPreference.SetBool(EToolPreference.ShowWelcomeScreen,flag);
            }
        }
    }
}

