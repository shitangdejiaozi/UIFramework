using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGUIManager : Singleton<UGUIManager> {

    private Dictionary<UGUI_TYPE, IWindowBase> m_loadedWindow = new Dictionary<UGUI_TYPE, IWindowBase>();
    public void Open(UGUI_TYPE uiType)
    {

    }

    public void Close(UGUI_TYPE uiType)
    {

    }
}
