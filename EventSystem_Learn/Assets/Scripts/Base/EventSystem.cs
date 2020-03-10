using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace IEvent
{
    public class EventSystem
    {
        #region 第一版事件系统
        protected static Dictionary<EventName, Delegate> m_eventsystem = new Dictionary<EventName, Delegate>();

        public static void OnListeningAdd(EventName eventName, Delegate callback)
        {
            if (!m_eventsystem.ContainsKey(eventName))
            {
                m_eventsystem.Add(eventName, null);
            }
            Delegate d = m_eventsystem[eventName];
            if (d != null && d.GetType() != callback.GetType())
            {
                throw new Exception(string.Format("尝试添加两种不同类型的委托,委托1为{0}，委托2为{1}", d.GetType(), callback.GetType()));
            }
        }

        public static void OnListeningRemove(EventName eventName, Delegate callback)
        {
            if (m_eventsystem.ContainsKey(eventName))
            {
                Delegate d = m_eventsystem[eventName];
                if (d == null)
                {
                    throw new Exception(string.Format("移除事件监听错误，事件{0}没有委托", eventName));
                }
                else if (d.GetType() != callback.GetType())
                {
                    throw new Exception(string.Format("尝试移除不同类型的事件，事件名{0},已存储的委托类型{1},当前事件委托{2}", eventName, d.GetType(), callback.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("没有事件名{0}", eventName));
            }
        }

        public static void OnLinsterRemoved(EventName eventName)
        {
            if (m_eventsystem[eventName] == null)
            {
                m_eventsystem.Remove(eventName);
            }
        }

        //bind
        //zero param
        public static void Bind(EventName eventName, CallBack callBack)
        {
            OnListeningAdd(eventName, callBack);
            m_eventsystem[eventName] = (CallBack)m_eventsystem[eventName] + callBack;
        }
        //one param
        public static void Bind<X>(EventName eventName, CallBack<X> callBack)
        {
            OnListeningAdd(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X>)m_eventsystem[eventName] + callBack;
        }
        //two param
        public static void Bind<X, Y>(EventName eventName, CallBack<X, Y> callBack)
        {
            OnListeningAdd(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X, Y>)m_eventsystem[eventName] + callBack;
        }
        //three parm
        public static void Bind<X, Y, Z>(EventName eventName, CallBack<X, Y, Z> callBack)
        {
            OnListeningAdd(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X, Y, Z>)m_eventsystem[eventName] + callBack;
        }

        //fire
        //zero param
        public static void Fire(EventName eventName)
        {
            Delegate d;
            if (m_eventsystem.TryGetValue(eventName, out d))
            {
                CallBack call = d as CallBack;
                if (call != null)
                {
                    call();
                }
                else
                {
                    throw new Exception(string.Format("事件{0}包含着不同类型的委托", eventName));
                }
            }
        }
        //one param
        public static void Fire<X>(EventName eventName, X arg1)
        {
            Delegate d;
            if (m_eventsystem.TryGetValue(eventName, out d))
            {
                CallBack<X> call = d as CallBack<X>;
                if (call != null)
                {
                    call(arg1);
                }
                else
                {
                    throw new Exception(string.Format("事件{0}包含着不同类型的委托", eventName));
                }
            }
        }
        //two param
        public static void Fire<X, Y>(EventName eventName, X arg1, Y arg2)
        {
            Delegate d;
            if (m_eventsystem.TryGetValue(eventName, out d))
            {
                CallBack<X, Y> call = d as CallBack<X, Y>;
                if (call != null)
                {
                    call(arg1, arg2);
                }
                else
                {
                    throw new Exception(string.Format("事件{0}包含着不同类型的委托", eventName));
                }
            }
        }
        //three parm
        public static void Fire<X, Y, Z>(EventName eventName, X arg1, Y arg2, Z arg3)
        {
            Delegate d;
            if (m_eventsystem.TryGetValue(eventName, out d))
            {
                CallBack<X, Y, Z> call = d as CallBack<X, Y, Z>;
                if (call != null)
                {
                    call(arg1, arg2, arg3);
                }
                else
                {
                    throw new Exception(string.Format("事件{0}包含着不同类型的委托", eventName));
                }
            }
        }

        //unbind
        //zero param
        public static void UnBind(EventName eventName, CallBack callBack)
        {
            OnListeningRemove(eventName, callBack);
            m_eventsystem[eventName] = (CallBack)m_eventsystem[eventName] - callBack;
            OnLinsterRemoved(eventName);
        }
        //one param
        public static void UnBind<X>(EventName eventName, CallBack<X> callBack)
        {
            OnListeningRemove(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X>)m_eventsystem[eventName] - callBack;
            OnLinsterRemoved(eventName);
        }
        //two param
        public static void UnBind<X, Y>(EventName eventName, CallBack<X, Y> callBack)
        {
            OnListeningRemove(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X, Y>)m_eventsystem[eventName] - callBack;
            OnLinsterRemoved(eventName);
        }
        //three parm
        public static void UnBind<X, Y, Z>(EventName eventName, CallBack<X, Y, Z> callBack)
        {
            OnListeningRemove(eventName, callBack);
            m_eventsystem[eventName] = (CallBack<X, Y, Z>)m_eventsystem[eventName] - callBack;
            OnLinsterRemoved(eventName);
        }
        #endregion
    }



    /// <summary>
    /// 上面的事件监听系统只会监听一次 也就是我在一个事件名内加了两个回调函数 如果我解绑事件的话 就会解绑两个函数了(错了,其实还传了一个委托进来,所以上面还是会只解绑其中一个委托而已)
    /// 但是我想要的是 只解绑其中的一个函数 另一个还可以再继续监听 这就是下面的改进版 而且可以外部解绑其中的某个函数
    /// 以后有需要再加上延迟触发的事件系统
    /// </summary>
    public class M_EventSystem
    {
        //以eventname为第一个字段存储所有的action 然后id为唯一的对应各个action
        protected static Dictionary<EventName, Dictionary<int, Action>> m_event_dic = new Dictionary<EventName, Dictionary<int, Action>>();
        //再存储一份id对应各个eventname的字典
        protected static Dictionary<int, EventName> m_id_to_eventname_dic = new Dictionary<int, EventName>();
        //每个action的id都会是唯一的 id默认可以从0开始  最后会返回到绑定事件的地方 可以由外部取消绑定
        protected static int m_eventname_id = 0;
        protected const int m_max_event_id = 888888888;

        /// <summary>
        /// 注册事件函数
        /// </summary>
        /// <param name="eventName">事件名</param>
        /// <param name="action">事件的回调函数</param>
        private static void RegisterSystem(EventName eventName, Action action)
        {
            if (!m_event_dic.ContainsKey(eventName))
            {
                m_event_dic.Add(eventName, new Dictionary<int, Action>());
            }
            Dictionary<int, Action> m_d = m_event_dic[eventName];
            //++m_eventname_id;
            Debug.Log("111  " + m_eventname_id);
            m_eventname_id = m_eventname_id + 1;
            Debug.Log("222  " + m_eventname_id);
            if (!m_d.ContainsKey(m_eventname_id))
            {
                m_d.Add(m_eventname_id, null);
            }
            Action a = m_d[m_eventname_id];
            if (a != null && a.GetType() != action.GetType())
            {
                throw new Exception(string.Format("尝试绑定两个不同类型的方法,已有的类型{0},需要添加的类型{1}", a.GetType(), action.GetType()));
            }
            m_d[m_eventname_id] = m_d[m_eventname_id] + action;
            RecordEventId(eventName);
        }

        /// <summary>
        /// 只记录唯一的一次 也就是id是唯一的 eventname是不唯一的
        /// </summary>
        /// <param name="eventName"></param>
        private static void RecordEventId(EventName eventName)
        {
            if (!m_id_to_eventname_dic.ContainsKey(m_eventname_id))
            {
                m_id_to_eventname_dic.Add(m_eventname_id, eventName);
            }
        }

        //private static int OnRegisterSuccess(EventName eventName, Action action)
        //{
        //    Dictionary<int, Action> m_d = m_event_dic[eventName];
        //    ++m_eventname_id;
        //    if (!m_d.ContainsKey(m_eventname_id))
        //    {
        //        m_d.Add(m_eventname_id, null);
        //    }
        //    Action a = m_d[m_eventname_id];
        //    if (a != null && a.GetType() != action.GetType())
        //    {
        //        throw new Exception(string.Format("尝试绑定两个不同类型的方法,已有的类型{0},需要添加的类型{1}", a.GetType(), action.GetType()));
        //    }
        //    m_d[m_eventname_id] = m_d[m_eventname_id] + action;
        //    return m_eventname_id;
        //}

        /// <summary>
        /// 根据事件名解绑委托
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="action"></param>
        private static void OnEventnameRemove(EventName eventName, Action action)
        {

        }

        /// <summary>
        /// 根据eventid来解绑委托
        /// </summary>
        /// <param name="action"></param>
        private static void OnEventIdRemove(Action action)
        {

        }

        //none parm bind
        public static int Bind(EventName eventName, Action action)
        {
            RegisterSystem(eventName, action);
            return m_eventname_id;
        }
        //one parm bind
        public static int Bind<X>(EventName eventName, Action<X> action)
        {
            //RegisterSystem(eventName, (Action<X>)action);
            return m_eventname_id;
        }

        //none parm fire
        public static void Fire(EventName eventName)
        {
            Dictionary<int, Action> m_d;
            if (m_event_dic.TryGetValue(eventName, out m_d))
            {
                if (m_d != null)
                {
                    for (int i = 1; i <= m_d.Count; i++)
                    {
                        Action action;
                        if (m_d.TryGetValue(i, out action))
                        {
                            if (action != null)
                            {
                                action();
                            }
                            else
                            {
                                throw new Exception("尝试调用不同类型的委托");
                            }
                        }
                    }
                }
            }
        }
        //one parm fire
        public static void Fire<X>(EventName eventName, X parm1)
        {

        }
        
        //none parm unbind  解绑委托时,根据之前给的event_id去找到eventname,然后解绑,然后其实可以不用传委托进来了
        public static void UnBind(int event_id)
        {
            ////确保事件id是绑定的事件id
            //if (event_id >= 0 && event_id <= m_eventname_id)
            //{
            //    EventName eventName;
            //    if (m_id_to_eventname_dic.TryGetValue(event_id, out eventName))
            //    {
            //        Dictionary<int, Action> m_d;
            //        if (m_event_dic.TryGetValue(eventName, out m_d))
            //        {
            //            if (m_d != null)
            //            {

            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    throw new Exception(string.Format("尝试解绑不同事件id的值{0}", event_id));
            //}
            EventName eventName;
            if (m_id_to_eventname_dic.TryGetValue(event_id, out eventName))
            {
                if (m_event_dic.ContainsKey(eventName))
                {
                    Dictionary<int, Action> m_d;
                    if (m_event_dic.TryGetValue(eventName, out m_d))
                    {
                        if (m_d != null)
                        {
                            if (m_d.ContainsKey(event_id))
                            {
                                Action action;
                                if (m_d.TryGetValue(event_id, out action))
                                {
                                    if (action != null)
                                    {
                                        m_d[event_id] = m_d[event_id] - action;
                                        if (m_d[event_id] == null)
                                        {
                                            m_d.Remove(event_id);
                                            m_id_to_eventname_dic.Remove(event_id);
                                        }
                                        if (m_event_dic[eventName] == null)
                                        {
                                            m_event_dic.Remove(eventName);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                throw new Exception(string.Format("没有对应的事件,event_id: {0}", event_id));
                            }
                        }
                    }
                }
                else
                {
                    throw new Exception(string.Format("没有对应的事件,eventName: {0}", eventName));
                }
            }
        }
        //one parm unbind
        public static void UnBind<X>(int event_id, Action<X> action)
        {

        }
    }
}