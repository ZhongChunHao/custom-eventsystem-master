using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class IUIEvent : UnityEvent<string>
{
    //构造函数
    public IUIEvent()
    {
        Debug.Log("new IUIEvent");
    }

    //析构函数 该实例被删除时触发
    ~IUIEvent()
    {
        Debug.Log("IUIEvent delete");
    }
}

public class UnityEventTest : MonoBehaviour {
    public IUIEvent iUIEvent;
    
    void Start () {
        if (iUIEvent == null)
        {
            iUIEvent = new IUIEvent();
        }
        iUIEvent.AddListener((string argms) => Debug.Log(string.Format("AddListener,参数{0}", argms)));
        iUIEvent.Invoke("good");
    }
	
	void Update () {
		
	}
}
