using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 全局消息系统，来自2017优化手册，比较通用的实现
/// </summary>
public class MessageManager : SingletonDontDestroy<MessageManager> {
    public delegate void MessageHandleDelegate(Message msg);
    private Dictionary<MESSAGE_TYPE, List<MessageHandleDelegate>> m_DelegateLists = new Dictionary<MESSAGE_TYPE, List<MessageHandleDelegate>>(); //注意dict中枚举为key的优化，会造成gc， 类型转换为int
    private Queue<Message> m_msgQueue = new Queue<Message>();
    private Dictionary<Type, Message> m_MsgMap = new Dictionary<Type, Message>();

    public void AddMsg(Type type, Message msg)
    {
        if(!m_MsgMap.ContainsKey(type))
        {
            m_MsgMap.Add(type, msg);
        }
    }

    public Message GetMsg(Type type)
    {
        if(m_MsgMap.ContainsKey(type))
        {
            return m_MsgMap[type];
        }
        return null;
    }
	// Update is called once per frame
	void Update ()
    {
        float preTime = Time.realtimeSinceStartup;
        while(m_msgQueue.Count > 0)
        {
            SendMessage(m_msgQueue.Dequeue());
            if(Time.realtimeSinceStartup - preTime > 0.01f)
            {
                return; //时间太长了，等下一帧再处理，优化点
            }
        }
		
	}

    public bool AddListener(MESSAGE_TYPE msgType, MessageHandleDelegate handle)
    {
        if(!m_DelegateLists.ContainsKey(msgType))
        {
            m_DelegateLists.Add(msgType, new List<MessageHandleDelegate>());
        }
        List<MessageHandleDelegate> handleList = m_DelegateLists[msgType];
        if(handleList.Contains(handle))
        {
            return false;
        }
        handleList.Add(handle);
        return true;
    }

    public void RemoveListener(MESSAGE_TYPE msgType, MessageHandleDelegate handle)
    {
        List<MessageHandleDelegate> handleList = null;
        if(m_DelegateLists.TryGetValue(msgType, out handleList))
        {
            handleList.Remove(handle);
        }
    }

    public void SendMessage(Message msg)
    {
        MESSAGE_TYPE type = msg.GetMsgType();
        List<MessageHandleDelegate> handleList = null;
        if(m_DelegateLists.TryGetValue(type, out handleList))
        {
            ExecuteDelegate(handleList, msg);
        }
    }

    public void PostMessage(Message msg)
    {
        if(!m_DelegateLists.ContainsKey(msg.GetMsgType()))
        {
            return;
        }
        m_msgQueue.Enqueue(msg);
    }

    private void ExecuteDelegate(List<MessageHandleDelegate> handleList, Message msg)
    {
        if (handleList != null)
        {
            for (int i = 0; i < handleList.Count; i++)
            {
                handleList[i](msg);
            }
        }
    }
}

public abstract class Message
{
    public abstract MESSAGE_TYPE GetMsgType();


}

public class CommonNotify : Message
{
    private MESSAGE_TYPE m_msgType;
    public override MESSAGE_TYPE GetMsgType()
    {
        return m_msgType;
    }

    public void SetData(MESSAGE_TYPE msgType)
    {
        m_msgType = msgType;
    }
}

public class CommonNotify<T> : Message
{
    private MESSAGE_TYPE m_msgType;
    private T _data;
    public override MESSAGE_TYPE GetMsgType()
    {
        return m_msgType;
    }

    public void SetData(MESSAGE_TYPE msgType, T data)
    {
        m_msgType = msgType;
        _data = data;
    }

    public void GetData(out T data)
    {
        data = _data;
    }
}

public class CommonNotify<T, U> : Message
{
    private MESSAGE_TYPE m_msgType;
    private T _dataT;
    private U _dataU;

    public override MESSAGE_TYPE GetMsgType()
    {
        return m_msgType;
    }

    public void SetData(MESSAGE_TYPE msgType, T dataT, U dataU)
    {
        m_msgType = msgType;
        _dataT = dataT;
        _dataU = dataU;
    }

    public void GetData(out T dataT, out U dataU)
    {
        dataT = _dataT;
        dataU = _dataU;

    }

    
}

public class CommonUIRefresh : Message
{
    public UGUI_TYPE UIType;
    private object[] m_data;
    public RefreshFuncType function;
    public override MESSAGE_TYPE GetMsgType()
    {
        return MESSAGE_TYPE.REFRESH_UI;
    }

    public void SetParam(params object[] args)
    {
        m_data = args;
    }

    public object[] GetParam()
    {
        return m_data;
    }
}
