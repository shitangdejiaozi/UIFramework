using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;

public class UIBase3D : GameWindowBase, IWindowBase{

    public void Open()
    {
        

    }

    public void Close()
    {

    }

    public void Release()
    {

    }

    public void Refresh(RefreshFuncType funcType, params object[] args)
    {

    }
    public bool Is3D()
    {
        return true;
    }
    public bool IsVisible()
    {
        return gameObject.activeSelf;
    }
}
