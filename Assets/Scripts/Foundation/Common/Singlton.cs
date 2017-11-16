using UnityEngine;
using System.Collections;

namespace Foundation.Common
{
    public abstract class Singleton<T> where T : new()
    {
        private static T m_instance;
        private static readonly object m_lock = new object();//防止多线程修改

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (m_lock)
                    {
                        if( m_instance ==null )
                            m_instance = new T();
                    }
                }
                return m_instance;
            }
        }
    }

    public class UnitySingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_instance;
        private static readonly object m_lock = new object();

        private static bool m_applicationIsQuiting = false;

        public static T Instance
        {
            get {
                if (m_applicationIsQuiting)
                {
                    Debug.LogWarning("Application has quit,Singleton instance '" + typeof(T) + "' will not be creat,return null");
                    return null;
                }
                lock (m_lock)
                {
                    m_instance = (T)FindObjectOfType(typeof(T));
                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("There exist more than one " + typeof(T) + "it should be a singleton");
                        return m_instance;
                    }
                    if (m_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        m_instance = singleton.AddComponent<T>();
                        singleton.name = "Singleton" + typeof(T).ToString();
                        DontDestroyOnLoad(singleton);
                    }
                    return m_instance;
                }
            }
        }
        //防止程序退出销毁后，再次创建
        public void OnDestroy()
        {
            m_applicationIsQuiting = true;
        }
    }
}

