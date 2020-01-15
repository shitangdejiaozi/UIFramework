using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Init : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UGUIManager.Instance.Open(UGUI_TYPE.UIMain);
    }
	
	// Update is called once per frame
	void Update () {
        TimerManager.Instance.UpdateTimer(Time.deltaTime);
    }
}
