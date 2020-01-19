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

        MessageManager.Instance.AddListener(MESSAGE_TYPE.OPEN_TESTTHREE, OpenThreee);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        Debug.LogError("open uitest");
    }

    public override void OnRefresh(RefreshFuncType funcType, params object[] args)
    {
        base.OnRefresh(funcType, args);

        Debug.LogError("param" + args[0] + ", " +args[1]);
    }

    public override void OnClose()
    {
        base.OnClose();
        Debug.LogError("close uitest");
    }

    public override void OnRelease()
    {
        base.OnRelease();
        MessageManager.Instance.RemoveListener(MESSAGE_TYPE.OPEN_TESTTHREE, OpenThreee);

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

    public void OpenThreee(Message msg)
    {
        int res = 0;
        string name;
        CommonNotify<int, string> message = (CommonNotify<int, string>)msg;
        message.GetData(out res, out name);
        Debug.LogError("message call back" + res +"name " + name);
    }
}
