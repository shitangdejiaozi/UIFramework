using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReceiverManager : MonoBehaviour {

    private Dictionary<int, Action> m_handlers = new Dictionary<int, Action>(); //Action<Msg> 具体Msg结构为消息的结构
	// Use this for initialization
	void Start ()
    {
        ReceiverBase receiverBase = new ReceiverBase(this);
	}
	
    public void AddHandler(int msgId, Action handler)
    {
        if (handler == null)
            return;

        if(m_handlers.ContainsKey(msgId))
        {
            m_handlers[msgId] += handler;
        }
        else
        {
            m_handlers[msgId] = handler;
        }
    }

    /// <summary>
    /// 由netmanager触发
    /// </summary>
    /// <param name="msgId"></param>
    public void Notify(int msgId)
    {
        Action action;
        if(m_handlers.TryGetValue(msgId,out action))
        {
            action();
        }
    }
	
}
