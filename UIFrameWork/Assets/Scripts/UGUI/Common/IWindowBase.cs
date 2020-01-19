using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;

public enum UGUI_LAYER
{
    ROOT = 500, //主界面的相关的UI
    MENU = 1000, //各种系统的层,一般是全屏的
    POPUP = 1500,  //弹出的弹窗， 非全屏
    POPUP_TOP = 2000, //最上层的UI， 比如tips 
    LOADING = 4000, //LOAIDING在所有UI上。
}

public interface IWindowBase : IBindableUI{

    void Open();

    void Close();

    void Refresh(RefreshFuncType funcType, params object[] args);
    bool Is3D();

    bool IsVisible();
}
