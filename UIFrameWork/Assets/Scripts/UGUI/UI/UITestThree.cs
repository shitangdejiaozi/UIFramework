using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;
using UnityEngine.UI;

public class UITestThree : UIBase2D {

    #region 控件绑定变量声明，自动生成请勿手改
    [ControlBinding]
    private Button m_closeBtn;

    #endregion


    public override UGUI_LAYER Getlayer()
    {
        return UGUI_LAYER.POPUP;
    }

    public override UGUI_TYPE GetUIType()
    {
        return UGUI_TYPE.UITestThree;
    }

    public override void Initialize()
    {
        base.Initialize();
        m_closeBtn.onClick.AddListener(CloseBtn);
    }

    public void CloseBtn()
    {
        CloseSelf();
    }
}
