using System;
using System.Collections.Generic;

namespace Foundation.Event
{
    public class CEvent 
    {
        private EEventType m_eventId;
        private Dictionary<string, object> m_paramList;

        public CEvent()
        {
            m_paramList = new Dictionary<string, object>();
        }

        public CEvent(EEventType eventType)
        {
            m_eventId = eventType;
            m_paramList = new Dictionary<string, object>();
        }

        public EEventType GetEventId()
        {
            return m_eventId;
        }

        public void AddParam(string name, object value)
        {
            m_paramList[name] = value;
        }

        public object GetParam(string name)
        {
            if (m_paramList.ContainsKey(name))
            {
                return m_paramList[name];
            }
            return null;
        }

        public Dictionary<string, object> GetParamList()
        {
            return m_paramList;
        }

        public bool HasParam(string name)
        {
            if (m_paramList.ContainsKey(name))
                return true;
            return false;
        }
        public int GetParamCount()
        {
            return m_paramList.Count;
        }
    }
}

