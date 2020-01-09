using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot2D : MonoBehaviour {

    public void Initialize()
    {

    }

    public void Add(IWindowBase window)
    {
        if (window == null)
            return;
        UIBase2D uibase2D = window as UIBase2D;
        uibase2D.Initialize();
        uibase2D.transform.SetParent(transform, false);

    }

    public void Release(IWindowBase window)
    {
        if (window == null)
            return;
        UIBase2D uibase2D = window as UIBase2D;
        uibase2D.OnRelease();
        Destroy(uibase2D.gameObject);
    }
}
