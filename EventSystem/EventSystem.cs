using System;
using System.Collections.Generic;
using Foundation.Common;

namespace Foundation.Event
{
    public static class EventSystem 
    {
        public static Dictionary<EEventType, Delegate> mEventTable = new Dictionary<EEventType, Delegate>();

        //handle the permanenet message ,regardless clear up
        public static List<EEventType> mPermanentMessages = new List<EEventType>();

        //标记成不被删除的event
        public static void MarkAsPermanent(EEventType eventType)
        {
            if (!mPermanentMessages.Contains(eventType))
            {
                mPermanentMessages.Add(eventType);
            }
        }

        public static void ClearUp()
        {
            List<EEventType> removeList = new List<EEventType>();

            foreach (KeyValuePair<EEventType, Delegate> pair in mEventTable)
            {
                bool isPermanent = mPermanentMessages.Contains(pair.Key);
                if (!isPermanent)
                {
                    removeList.Add(pair.Key);
                }
            }

            foreach (EEventType eventType in removeList)
            {
                mEventTable.Remove(eventType);
            }
        }



        #region add listener
        public static void OnListenningAdding(EEventType eventType, Delegate callBack)
        {
            if (!mEventTable.ContainsKey(eventType))
            {
                mEventTable.Add(eventType, null);
            }
            Delegate d = mEventTable[eventType];

            if (d != null && d.GetType() != callBack.GetType())
            {
                throw new EventException(String.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, callBack.GetType().Name));
            }
        }
        public static void AddListener(EEventType eventType,CallBack handler)
        {
            OnListenningAdding(eventType,handler);
            mEventTable[eventType] = (CallBack)mEventTable[eventType] + handler;
        }

        public static void AddListener<T>(EEventType eventType, CallBack<T> handler)
        {
            OnListenningAdding(eventType, handler);
            mEventTable[eventType] = (CallBack<T>)mEventTable[eventType] + handler;
        }

        public static void AddListener<T, U>(EEventType eventType, CallBack<T, U> handler)
        {
            OnListenningAdding(eventType,handler);
            mEventTable[eventType] = (CallBack<T, U>)mEventTable[eventType] + handler; 
        }
        #endregion

        #region  remove listener
        public static void OnListenerRemoving(EEventType eventType,Delegate removing)
        {
            if (mEventTable.ContainsKey(eventType))
            {
                Delegate handler = mEventTable[eventType];
                if (handler == null)
                {
                    throw new EventException(String.Format("Attempting remove listener {0},but is none", eventType));
                }
                else if (handler.GetType() != removing.GetType())
                {
                    throw new EventException(String.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, handler.GetType().Name, removing.GetType().Name));
                }
            }
            else
            {
                throw new EventException(String.Format("Attempting remove type {0} from event table,but not exist",eventType));
            }
        }
        public static void OnListenerRemoved(EEventType eventType)
        {
            if (mEventTable[eventType] == null)
                mEventTable.Remove(eventType);
        }

        public static void RemoveListener(EEventType eventType,CallBack handler)
        {
            OnListenerRemoving(eventType,handler);
            mEventTable[eventType] = (CallBack)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        public static void RemoveListener<T>(EEventType eventType, CallBack<T> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallBack<T>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        public static void RemoveListener<T,U>(EEventType eventType, CallBack<T,U> handler)
        {
            OnListenerRemoving(eventType, handler);
            mEventTable[eventType] = (CallBack<T,U>)mEventTable[eventType] - handler;
            OnListenerRemoved(eventType);
        }
        #endregion

        #region
        public static void Broadcast(EEventType eventType)
        {
            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallBack handler = d as CallBack;
                if (handler != null)
                {
                    handler();
                }
                else
                {
                    throw new EventException(String.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster", eventType));
                }
            }
        }
        public static void Broadcast<T>(EEventType eventType, T arg1)
        {
            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallBack<T> handler = d as CallBack<T>;
                if (handler != null)
                {
                    handler(arg1);
                }
                else
                {
                    throw new EventException(String.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster", eventType));
                }
            }
        }
        public static void Broadcast<T,U>(EEventType eventType, T arg1, U arg2)
        {
            Delegate d;
            if (mEventTable.TryGetValue(eventType, out d))
            {
                CallBack<T,U> handler = d as CallBack<T,U>;
                if (handler != null)
                {
                    handler(arg1,arg2);
                }
                else
                {
                    throw new EventException(String.Format("Broadcasting message \"{0}\" but listeners have a different signature than the broadcaster", eventType));
                }
            }
        }
        #endregion

        public static void SendEvent(CEvent evt)
        {
            Broadcast<CEvent>(evt.GetEventId(),evt);
        }
    }


    #region callback founctions
    public delegate void CallBack();
    public delegate void CallBack<T>(T arg);
    public delegate void CallBack<T, U>(T arg1, U arg2);
    public delegate void CallBack<T, U, V>(T arg1,U arg2,V arg3);
    public delegate void CallBack<T, U, V, X>(T arg1,U arg2,V arg3,X arg4);
    #endregion

    public class EventException : Exception
    {
        public EventException(string msg) : base(msg)
        {
        }

        public EventException(string msg, Exception innerException) : base(msg,innerException)
        {
        }
    }
}

