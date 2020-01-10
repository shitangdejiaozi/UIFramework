using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using System;

public abstract class UIBase2D : GameWindowBase, IWindowBase {

    protected UGUI_LAYER UILayer = UGUI_LAYER.ROOT;
    private bool m_autoRelease = true; //自动回收
    protected Transform UIRootTrans;

    public abstract UGUI_LAYER Getlayer(); //每个UI都要layer

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

    public bool IsVisible()
    {
        return  gameObject.activeSelf;
    }

    public void CloseSelf()
    {
        UGUIManager.Instance.Close(GetUIType());
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

    /// <summary>
    /// 设置UI的depth
    /// </summary>
    /// <param name="depth"></param>
    public void SetUIDepth(int depth)
    {
        if(UIRootTrans ==null)
        {
            UIRootTrans = transform;
        }
        if(UIRootTrans != null)
        {
            Canvas canvas = UIRootTrans.GetComponent<Canvas>();
            if(canvas != null)
            {
                canvas.renderMode = RenderMode.ScreenSpaceCamera;
                canvas.sortingOrder = depth;
            }
        }
    }
}
