using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEvent;

public class TextBindTest : MonoBehaviour {
    [HideInInspector]
    public Text txt;
    [HideInInspector]
    public int event_id = 0;

    private void Awake()
    {
        txt = this.transform.GetComponent<Text>();
    }
    
    void Start () {
        //EventSystem.Bind<string, string>(EventName.SHOW_TEXT, WriteText);
        //event_id = M_EventSystem.Bind<string>(EventName.SHOW_TEXT, WriteText);
        event_id = M_EventSystem.Bind(EventName.SHOW_TEXT, WriteText);
        Debug.Log("event_id:" + event_id);
	}

    public void WriteText(string s, string s2)
    {
        if (txt!=null)
        {
            string str = s + s2;
            txt.text = str;
            Debug.Log("123321" + str);
        }
        EventSystem.UnBind<string, string>(EventName.SHOW_TEXT, WriteText);
    }

    public void WriteText(string s)
    {
        if (txt != null)
        {
            txt.text = s;
            Debug.Log("666666" + s);
        }
        //M_EventSystem.UnBind<string>(event_id, WriteText);
        //这里完全可以不用传委托进去了的
        M_EventSystem.UnBind(event_id);
    }

    public void WriteText()
    {
        if (txt != null)
        {
            txt.text = "69";
            Debug.Log("69");
        }
        M_EventSystem.UnBind(event_id);
    }
	
	void Update () {
		
	}
}
