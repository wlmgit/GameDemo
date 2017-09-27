using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Tool.Editor
{
    public static class ToolUtility
    {
        private static GUIStyle m_toolbarButtonSelectStyle = null;
        private static GUIStyle m_toolPreferencePenelStyle = null;

        private static Dictionary<string, Texture2D> m_textureCache = new Dictionary<string, Texture2D>();

        public delegate void PreferenceChanged(EToolPreference pref, System.Object value);

        public static Texture2D LoadTexture(string imageName, bool useSkinColor = true, System.Object obj = null)
        {
            if (m_textureCache.ContainsKey(imageName))
            {
                return m_textureCache[imageName];
            }
            Texture2D texture2D = null;

            string name = string.Format("{0}{1}" , (!useSkinColor) ? string.Empty : ((!EditorGUIUtility.isProSkin) ? "Light" : "Dark"),imageName );
            Stream mainfestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);

            if (mainfestResourceStream == null)
            {
                name = string.Format("ToolEditor.Resources.{0}{1}", (!useSkinColor) ? string.Empty : ((!EditorGUIUtility.isProSkin) ? "Light" : "Dark"), imageName);
                mainfestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
            }
            if (mainfestResourceStream != null)
            {
                texture2D = new Texture2D(0, 0, TextureFormat.RGBA32, false, true);
                texture2D.LoadImage(ReadToEnd(mainfestResourceStream));
                mainfestResourceStream.Close();
            }
            
            texture2D.hideFlags = HideFlags.HideAndDontSave;
            m_textureCache.Add( imageName , texture2D );

            return texture2D;
        }

        private static byte[] ReadToEnd(Stream stream)
        {
            byte[] array = new byte[16384];
            byte[] result;
            using ( MemoryStream memoryStream = new MemoryStream() )
            {
                int count = 0;
                while ( (count = stream.Read(array,0,array.Length)) > 0 )
                {
                    memoryStream.Write(array,0,count);
                }
                result = memoryStream.ToArray();
            }
            return result;
        }

        public static GUIStyle ToolbarSelectStyle
        {
            get
            {
                if (m_toolbarButtonSelectStyle == null)
                {
                    m_toolbarButtonSelectStyle = new GUIStyle(EditorStyles.toolbarButton);
                    m_toolbarButtonSelectStyle.normal.background = m_toolbarButtonSelectStyle.active.background;
                }
                return m_toolbarButtonSelectStyle;
            }
        }

        public static GUIStyle ToolPreferencePanelStyle
        {
            get
            {
                if (m_toolPreferencePenelStyle == null)
                {
                    m_toolPreferencePenelStyle = new GUIStyle(GUI.skin.box);
                    m_toolPreferencePenelStyle.normal.background = EditorStyles.toolbarButton.normal.background;
                }
                return m_toolPreferencePenelStyle;
            }
        }
    }
}

