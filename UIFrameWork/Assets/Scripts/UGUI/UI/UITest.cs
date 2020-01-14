using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using UnityEngine.UI;

public class UITest : UIBase2D {

    #region 控件绑定变量声明，自动生成请勿手改
    [ControlBinding]
    private Button m_ReturnBtn;
    [ControlBinding]
    private Button m_openBtn;
    [ControlBinding]
    private Button m_closeBtn;

    #endregion



    public override UGUI_LAYER Getlayer()
    {
        return UGUI_LAYER.MENU;
    }

    public override UGUI_TYPE GetUIType()
    {
        return UGUI_TYPE.UITest;
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.LogError("Initialize");
        m_ReturnBtn.onClick.AddListener(ClickClose);
        m_openBtn.onClick.AddListener(OpenWindow1);
        m_closeBtn.onClick.AddListener(CloseWindow1);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Debug.LogError("open uitest");
    }

    public override void OnClose()
    {
        base.OnClose();
        Debug.LogError("close uitest");
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    public void ClickClose()
    {
        Debug.LogError("ClickClose");
        UGUIManager.Instance.Close(GetUIType());
    }

    public void OpenWindow1()
    {
        UGUIManager.Instance.Open(UGUI_TYPE.UITestThree, UGUI_TYPE.UITestTwo);
    }

    public void CloseWindow1()
    {
        UGUIManager.Instance.Close(UGUI_TYPE.UITestTwo, true);
    }
}
