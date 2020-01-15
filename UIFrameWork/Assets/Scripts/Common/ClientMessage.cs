using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ClientMessage
{

	public static void SendMessage(MESSAGE_TYPE msgType)
    {
        CommonNotify message = (CommonNotify)MessageManager.Instance.GetMsg(typeof(CommonNotify));
        if(message == null)
        {
            message = new CommonNotify();
            MessageManager.Instance.AddMsg(typeof(CommonNotify), message);
        }
        message.SetData(msgType);
        MessageManager.Instance.SendMessage(message);
    }

    public static void SendMessage<T>(MESSAGE_TYPE msgType, T data)
    {
        CommonNotify<T> message = (CommonNotify<T>)MessageManager.Instance.GetMsg(typeof(CommonNotify<T>));
        if(message == null)
        {
            message = new CommonNotify<T>();
            MessageManager.Instance.AddMsg(typeof(CommonNotify<T>), message);
        }
        message.SetData(msgType, data);
        MessageManager.Instance.SendMessage(message);
    }

    public static void SendMessage<T, U>(MESSAGE_TYPE msgType, T dataT, U dataU)
    {
        CommonNotify<T, U> message = (CommonNotify<T, U>)MessageManager.Instance.GetMsg(typeof(CommonNotify<T, U>));
        if (message == null)
        {
            message = new CommonNotify<T, U>();
            MessageManager.Instance.AddMsg(typeof(CommonNotify<T, U>), message);
        }
        message.SetData(msgType, dataT, dataU);
        MessageManager.Instance.SendMessage(message);
    }
}
