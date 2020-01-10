using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRoot2D : MonoBehaviour {

    private Dictionary<UGUI_LAYER, List<UIBase2D>> m_openMap = new Dictionary<UGUI_LAYER, List<UIBase2D>>(); //记录打开的每个layer的window列表
    private int m_depthInterval = 10; //depth间隔
    public void Initialize()
    {
        m_openMap.Clear();
    }

    public void Add(IWindowBase window)
    {
        if (window == null)
            return;
        UIBase2D uibase2D = window as UIBase2D;

        uibase2D.Initialize();
        uibase2D.transform.SetParent(transform, false);

        UGUI_LAYER layer = uibase2D.Getlayer();
        List<UIBase2D> listWindows = null;
        if(!m_openMap.TryGetValue(layer, out listWindows))
        {
            listWindows = new List<UIBase2D>();
            m_openMap.Add(layer, listWindows);
        }
        listWindows.Add(uibase2D);
        SortLayer(layer, listWindows);
    }

    public void Release(IWindowBase window)
    {
        if (window == null)
            return;
        UIBase2D uibase2D = window as UIBase2D;
        uibase2D.OnRelease();
        List<UIBase2D> listwindow = null;
        m_openMap.TryGetValue(uibase2D.Getlayer(), out listwindow);
        if(listwindow != null)
        {
            listwindow.Remove(uibase2D);
        }
        Destroy(uibase2D.gameObject);
    }

    /// <summary>
    /// 对UI进行排序
    /// </summary>
    public void SortLayer(UGUI_LAYER layer, List<UIBase2D> listWindow)
    {
        if (listWindow == null)
            return;

        int depth = (int)layer;
        for(int i = 0; i< listWindow.Count; i++)
        {
            listWindow[i].SetUIDepth(depth);
            depth += m_depthInterval;
        }
    }


    /// <summary>
    /// 对某个子UI重新排序
    /// </summary>
    /// <param name="window"></param>
    public void SortChildLayer(IWindowBase window)
    {
        UIBase2D uibase2D = window as UIBase2D;
        if(uibase2D == null)
        {
            return;
        }

        UGUI_LAYER layer = uibase2D.Getlayer();
        List<UIBase2D> listWindows = null;
        if (!m_openMap.TryGetValue(layer, out listWindows))
        {
            listWindows = new List<UIBase2D>();
            m_openMap.Add(layer, listWindows);
        }
        listWindows.Remove(uibase2D);
        listWindows.Add(uibase2D);
        SortLayer(layer, listWindows);
    }
}
