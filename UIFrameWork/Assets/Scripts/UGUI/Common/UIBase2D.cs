using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using System;

public abstract class UIBase2D : GameWindowBase, IWindowBase {

    protected UGUI_LAYER UILayer = UGUI_LAYER.ROOT;
    private bool m_autoRelease = true; //自动回收
    protected Transform UIRootTrans;
    private uint m_timer = 0;
    protected float m_releaseTime = 0.01f;
    public abstract UGUI_LAYER Getlayer(); //每个UI都要layer
    protected Canvas SelfCanvas = null;

    public void Open()
    {
        CommonFunc.SetLayer(gameObject, "UGUI");
        gameObject.SetActive(true);
        OnOpen();
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
        AddAutoReleaseTimer();
        OnClose();
    }

    public void Refresh(RefreshFuncType funcType, params object[] args)
    {
        OnRefresh(funcType, args);
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

    public Canvas GetSelfCanvas()
    {
        return SelfCanvas;
    }

    //抽象方法，必须实现
    public abstract UGUI_TYPE GetUIType();

    public virtual void Initialize()
    {
        SelfCanvas = GetComponent<Canvas>();
        SetCanvas();

    }

    public virtual void OnOpen()
    {

    }

    public virtual void OnRefresh(RefreshFuncType funcType, params object[] args)
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
        if(SelfCanvas != null)
        {
            SelfCanvas.sortingOrder = depth;
        }

        ClientMessage.SendMessage(MESSAGE_TYPE.SORT_ORDER);
    }

    
    private void SetCanvas()
    {
        
        if (SelfCanvas != null)
        {
            SelfCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Normal;
            SelfCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;
            SelfCanvas.additionalShaderChannels |= AdditionalCanvasShaderChannels.Tangent;
            SelfCanvas.renderMode = RenderMode.ScreenSpaceCamera; //应该放在初始化时
            if (UGUIManager.Instance.UICamera != null)
            {
                SelfCanvas.worldCamera = UGUIManager.Instance.UICamera;
            }
        }
    }

    private void AddAutoReleaseTimer()
    {
        if(m_autoRelease && m_timer == 0)
        {
            TimerManager.Instance.AddTimer(m_releaseTime, Release);
        }
    }

    private void Release()
    {
        UGUIManager.Instance.ReleaseWindow(GetUIType());
    }
}
