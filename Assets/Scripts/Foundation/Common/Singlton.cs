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
}

