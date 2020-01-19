using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ReceiverBase
{

    private ReceiverManager m_receiverManger;
    public ReceiverBase(ReceiverManager receiverManager)
    {
        m_receiverManger = receiverManager;
        Initialize();
    }

    ~ReceiverBase()
    {
        Release();
    }
    public void AddMessageHandler(int msgId, Action handler)
    {
        m_receiverManger.AddHandler(msgId, handler);
    }


    public virtual void Initialize()
    {
        Debug.LogError("initialzie");
    }

    public virtual void Release()
    {

    }

}
