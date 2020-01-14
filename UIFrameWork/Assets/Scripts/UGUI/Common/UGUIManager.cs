using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using System;
using SDGame.UITools;

public class UGUIManager : SingletonDontDestroy<UGUIManager> {

    private Dictionary<UGUI_TYPE, IWindowBase> m_loadedWindow = new Dictionary<UGUI_TYPE, IWindowBase>(); //记录加载过的UI
    private string m_configPath = "Assets/Resources/UIConfig.asset";

    private Stack<ParentUIStruct> m_ParentUI = new Stack<ParentUIStruct>();
    private GameObject m_UICameraGo;
    private Camera m_uiCamera;
    private UIRoot2D m_root2D;
    private GameObject EventSystem;

    struct ParentUIStruct
    {
        public UGUI_TYPE ChildUI;
        public UGUI_TYPE ParentUI;
    }

    private void Awake()
    {
        if(m_UICameraGo == null)
        {
            m_uiCamera = gameObject.AddChild<Camera>();
            m_UICameraGo = m_uiCamera.gameObject;
            m_UICameraGo.name = "UICamera";

            int layer = LayerMask.NameToLayer("UGUI");
            m_uiCamera.clearFlags = CameraClearFlags.Depth;
            m_uiCamera.orthographic = true;
            m_uiCamera.depth = 1000;
            m_uiCamera.depthTextureMode = DepthTextureMode.None;
            m_uiCamera.orthographicSize = 5f;
            m_uiCamera.cullingMask = (1 << layer);
            m_uiCamera.useOcclusionCulling = false;
            m_uiCamera.allowHDR = false;
            m_uiCamera.allowMSAA = false;

        }
        if(EventSystem == null)
        {
            var es = gameObject.AddChild<StandaloneInputModule>();
            EventSystem = es.gameObject;
            EventSystem.name = "EventSystem";
        }
        if(m_root2D == null)
        {
            m_root2D = gameObject.AddChild<UIRoot2D>();
            m_root2D.Initialize();
        }
    }

    public IWindowBase Open(UGUI_TYPE uiType, UGUI_TYPE parentUIType = UGUI_TYPE._BEGIN)
    {
        IWindowBase uiwindow; 
        m_loadedWindow.TryGetValue(uiType, out uiwindow);
        if(uiwindow == null)
        {
            uiwindow = CreateWindow(uiType);
            if (uiwindow == null)
                return null;
            if(!uiwindow.Is3D())
            {
                m_root2D.Add(uiwindow);
            }
        }
        else
        {
            m_root2D.SortChildLayer(uiwindow); //还未销毁，就重新排序
        }
        
        uiwindow.Open();
        if(!uiwindow.Is3D())
        {
            UIBase2D uibase2D = uiwindow as UIBase2D;
            if (uibase2D.Getlayer() == UGUI_LAYER.MENU)
            {
                SetMainUIActive(false);
            }
        }
        HideMainCamera();
        if(parentUIType != UGUI_TYPE._BEGIN)
        {
            ParentUIStruct parentUI = new ParentUIStruct();
            parentUI.ParentUI = parentUIType;
            parentUI.ChildUI = uiType;
            m_ParentUI.Push(parentUI);
        }
        return uiwindow;
    }

    public void Close(UGUI_TYPE uiType, bool release = false)
    {
        IWindowBase uiwindow = GetUI(uiType);
        if (uiwindow == null)
            return;

        uiwindow.Close();
        if(release)
        {
            if(uiwindow.Is3D())
            {

            }
            else
            {
                m_root2D.Release(uiwindow);
            }
            m_loadedWindow.Remove(uiType);
        }

        if(!uiwindow.Is3D())
        {
            if(!CheckMenuLayerVisiable()) //如果没有全屏 的系统了，就打开mainUI
                SetMainUIActive(true);
        }

        if(m_ParentUI.Count > 0)
        {
            ParentUIStruct parentUI = m_ParentUI.Peek();
            if(parentUI.ChildUI == uiType)
            {
                Open(parentUI.ParentUI); //就是一个固定返回界面，一种父子的关系，尽量少用
                m_ParentUI.Pop();
            }
        }
    }

    private IWindowBase CreateWindow(UGUI_TYPE uiType)
    {
        UIData uicfg = GetUIDataByUIType(uiType);
        if(uicfg == null)
        {
            Debug.LogErrorFormat("ui type {0} cfg is empty", uiType.ToString());
            return null;
        }

        GameObject windowObj;
        Debug.LogError("prefa name " + uicfg.PrefabName);
        var prefab = Resources.Load<GameObject>(uicfg.PrefabName); //根据项目具体调整
        windowObj = GameObject.Instantiate(prefab, transform);

        if(windowObj == null)
        {
            Debug.LogErrorFormat("ui type {0} prefab is null", uiType.ToString());
            return null;
        }
        windowObj.transform.position = Vector3.zero;
        
        Type scriptType = Type.GetType(uicfg.ScriptName);
        if(scriptType == null || scriptType.GetInterface("IWindowBase") == null)
        {
            Debug.LogErrorFormat("ui type {0} not have IWindowBase attach", uiType.ToString());
            return null;
        }

        var scaler = windowObj.GetComponent<CanvasScaler>();
        if(scaler != null)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand; //屏幕的适应方式
            scaler.referenceResolution = new Vector2(1280, 720);
        }

        IWindowBase uiwindow = windowObj.AddComponent(scriptType) as IWindowBase;
        UIControlData ctrlData = windowObj.GetComponent<UIControlData>();
        if(ctrlData != null)
        {
            ctrlData.BindDataTo(uiwindow);
        }

        windowObj.SetActive(false);
        m_loadedWindow.Add(uiType, uiwindow);
        return uiwindow;
    }

    public void ReleaseWindow(UGUI_TYPE uiType)
    {
        IWindowBase uiwindow = GetUI(uiType);
        if (uiwindow == null)
            return;
        if (uiwindow.Is3D())
        {

        }
        else
        {
            m_root2D.Release(uiwindow);
        }
        m_loadedWindow.Remove(uiType);


    }

    private UIData GetUIDataByUIType(UGUI_TYPE uiType)
    {
        string uiName = uiType.ToString();
        var Datas = AssetDatabase.LoadAssetAtPath<UIConfig>(m_configPath);
        if(Datas != null)
        {
            for(int i = 0; i< Datas.UIDatas.Count; i++)
            {
                if(Datas.UIDatas[i].UIName == uiName)
                {
                    return Datas.UIDatas[i];
                }
            }
        }
        return null;
    }

    public IWindowBase GetUI(UGUI_TYPE uiType)
    {
        IWindowBase uiwindow;
        m_loadedWindow.TryGetValue(uiType, out uiwindow);
        return uiwindow;
    }

    /// <summary>
    /// 打开全屏不透明界面时，可以关闭maincamera， 节省性能
    /// </summary>
    private void HideMainCamera()
    {

    }

    /// <summary>
    /// 打开全屏的界面时，可以隐藏掉主界面层的UI， 一个优化点，不要直接setactive，会导致重建，可以关闭canvas和raycast。
    /// </summary>
    public void SetMainUIActive(bool show)
    {
        foreach(var window in m_loadedWindow)
        {
            if(!window.Value.Is3D())
            {
                UIBase2D uiwindow = window.Value as UIBase2D;
                if(uiwindow.Getlayer() == UGUI_LAYER.ROOT)
                {
                    int layer = LayerMask.NameToLayer("HideUI");
                    CommonFunc.SetLayer(uiwindow.gameObject, layer);
                    uiwindow.gameObject.GetComponent<Canvas>().enabled = show;
                    uiwindow.gameObject.GetComponent<GraphicRaycaster>().enabled = show;
                    GraphicRaycaster[] rays = uiwindow.gameObject.GetComponentsInChildren<GraphicRaycaster>();
                    for(int i = 0; i < rays.Length; i++)
                    {
                        rays[i].enabled = show;
                    }
                }
            }
        }
    }

    public bool CheckMenuLayerVisiable()
    {
        foreach (var window in m_loadedWindow)
        {
            if (!window.Value.Is3D())
            {
                UIBase2D uiwindow = window.Value as UIBase2D;
                if (uiwindow.Getlayer() == UGUI_LAYER.MENU)
                {
                    if (uiwindow.IsVisible())
                        return true;
                }
            }
        }
        return false;
    }
    
}
