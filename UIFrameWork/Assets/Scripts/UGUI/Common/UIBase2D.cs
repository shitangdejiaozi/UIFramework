using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using System;

public abstract class UIBase2D : GameWindowBase, IWindowBase {

    private bool m_autoRelease = true; //自动回收
    public void Open()
    {
        CommonFunc.SetLayer(gameObject, "UGUI");
        gameObject.SetActive(true);
        OnOpen();
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
        OnClose();
    }

    public void Release()
    {
        OnRelease();
    }

    public bool Is3D()
    {
        return false;
    }

    //抽象方法，必须实现
    public abstract UGUI_TYPE GetUIType();

    public virtual void Initialize()
    {

    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {

    }

    public virtual void OnRelease()
    {

    }
}
