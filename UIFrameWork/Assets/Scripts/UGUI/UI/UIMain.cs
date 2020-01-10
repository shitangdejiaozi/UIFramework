using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using UnityEngine.UI;

public class UIMain : UIBase2D {

    #region 控件绑定变量声明，自动生成请勿手改
    [ControlBinding]
    private Button m_btn1;
    [ControlBinding]
    private Button m_btn2;
    [ControlBinding]
    private Button m_btn3;
    [ControlBinding]
    private Button m_btn4;

    #endregion


    public override UGUI_LAYER Getlayer()
    {
        return UGUI_LAYER.ROOT;
    }

    public override UGUI_TYPE GetUIType()
    {
        return UGUI_TYPE.UIMain;
    }

    public override void Initialize()
    {
        base.Initialize();
        m_btn1.onClick.AddListener(ClickBtn1);
        m_btn2.onClick.AddListener(ClickBtn2);
    }

    public override void OnOpen()
    {
        base.OnOpen();
    }

    public override void OnClose()
    {
        base.OnClose();
    }

    public void ClickBtn1()
    {
        UGUIManager.Instance.Open(UGUI_TYPE.UITest);
    }
    public void ClickBtn2()
    {

    }
}
