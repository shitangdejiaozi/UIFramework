using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SDGame.UITools;

public class UITestTwo : UIBase2D {

    #region 控件绑定变量声明，自动生成请勿手改
    [ControlBinding]
    private Button m_Btn1;
    [ControlBinding]
    private Button m_Btn2;

    #endregion


    public override UGUI_LAYER Getlayer()
    {
        return UGUI_LAYER.MENU;
    }

    public override UGUI_TYPE GetUIType()
    {
        return UGUI_TYPE.UITestTwo;
    }

    public override void Initialize()
    {
        base.Initialize();
        Debug.LogError("initaizelize");
        m_Btn1.onClick.AddListener(ClickClose);
    }
    public override void OnOpen()
    {
        Debug.LogError("onopen");
        base.OnOpen();
    }

    public void ClickClose()
    {
        CloseSelf();
    }
}
