using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Delegate Action Func Predicate 为四种委托 在C#中主要为这种委托 查看各种不同可以看下面的博客
/// https://www.cnblogs.com/akwwl/p/3232679.html
/// </summary>
public class DelegateTest {
    protected static Dictionary<EventName, Delegate> m_delegate_dic = new Dictionary<EventName, Delegate>();

    protected static Dictionary<EventName, Action> m_action_dic = new Dictionary<EventName, Action>();

    protected static Dictionary<EventName, Func<int>> m_func_dic = new Dictionary<EventName, Func<int>>();

    protected static Dictionary<EventName, Predicate<int>> m_predicate_dic = new Dictionary<EventName, Predicate<int>>();
}