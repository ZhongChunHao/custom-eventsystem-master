using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using IEvent;

public class BtnClickTest : MonoBehaviour {
    private void Awake()
    {
        this.transform.GetComponent<Button>().onClick.AddListener(() =>
           //EventSystem.Fire(EventName.SHOW_TEXT, "就是拽", this.transform.name)
           M_EventSystem.Fire(EventName.SHOW_TEXT)
        );
    }
    
    void Start () {
        
    }
	
	void Update () {
        
	}

    //public void SetText()
    //{
    //    EventSystem.Fire(EventName.SHOW_TEXT, "就是拽");
    //}
}
