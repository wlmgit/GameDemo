using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

namespace Tools.Editor
{
    public class ToolWindow : EditorWindow
    {
        private static Vector2 m_windowSize = new Vector2(800,600);

        private static Rect m_fileToolbarRect;
        private static Rect m_preferencePanelRect;

        private bool m_showPreference;

        private Vector2 m_mouseCurrentPosition = Vector2.zero;

        #region 左边属性选择
        private Rect m_propertyToobarRect;
        private int m_toolbarSelect = 1;
        private string[] m_toolbarString = new string[] {
        "menu1",
        "menu2",
        "menu3"};
        #endregion

        private GameObject m_selectObject;

        [SerializeField]
        private GenericMenu m_gameobjectMenu;

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
            this.m_mouseCurrentPosition = Event.current.mousePosition;
            SetupSize();
            if ( Draw() )
            {

            }
            DrawToolPreference();
            this.HandleEvent();
        }

        private void SetupSize()
        {
            float width = base.position.width;
            m_fileToolbarRect = new Rect(300,0,width-300,18);
            m_preferencePanelRect = new Rect(width - 290,20,290,50);
            m_propertyToobarRect = new Rect(0,0,300,18);
        }
        private bool Draw()
        {
            bool result = false;
            DrawFileToolbar();
            DrawPropertyToolbar();
            return result;
        }
        private void DrawFileToolbar()
        {
            GUILayout.BeginArea(m_fileToolbarRect, EditorStyles.toolbar);
            GUILayout.BeginHorizontal();
            string selectObjName = m_selectObject == null ? "None Select" : m_selectObject.name;
            if (GUILayout.Button(selectObjName, EditorStyles.toolbarPopup, GUILayout.Width(150)))
            {
                BuildGameobjectMenus();
                m_gameobjectMenu.ShowAsContext();
            }
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Preference",(!m_showPreference) ? EditorStyles.toolbarButton : ToolUtility.ToolbarSelectStyle ,GUILayout.Width(80f)))
            {
                m_showPreference = !m_showPreference;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void BuildGameobjectMenus()
        {
            m_gameobjectMenu = new GenericMenu();
            m_gameobjectMenu.AddItem(new GUIContent("key"),false,new GenericMenu.MenuFunction(SelectCallBack));
            m_gameobjectMenu.AddItem(new GUIContent("key1"), false, new GenericMenu.MenuFunction(SelectCallBack));
        }
        private void SelectCallBack()
        {
            Debug.Log("callback");
            m_selectObject = GameObject.Find("Cube");
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

        /// <summary>
        /// 左边toolbar选择面板
        /// </summary>
        private void DrawPropertyToolbar()
        {
            GUILayout.BeginArea(m_propertyToobarRect,EditorStyles.toolbar);
            int preSelect = m_toolbarSelect;
            m_toolbarSelect = GUILayout.Toolbar(m_toolbarSelect,m_toolbarString,EditorStyles.toolbarButton);
            GUILayout.EndArea();

            if (m_toolbarSelect == 0)
            {
                Debug.Log(String.Format("preselect is {0}", preSelect));
                Debug.Log("menu1 is select");
            }
            else if (m_toolbarSelect == 1)
            {
                Debug.Log(String.Format("preselect is {0}", preSelect));
                Debug.Log("menu2 is select");
            }
            else if (m_toolbarSelect == 2)
            {
                Debug.Log(String.Format("preselect is {0}", preSelect));
                Debug.Log("menu3 is Select");
            }
        }

        private void PrefChanged( EToolPreference pref, System.Object value)
        {
            Debug.Log(value.ToString());
        }

        private void HandleEvent()
        {
            EventType type = Event.current.type;
            switch (type)
            {
                case EventType.MouseDown:
                    Debug.Log("click");
                    break;
                case EventType.MouseDrag:
                    Debug.Log("drag");
                    break;
            }
        }
    }
}

