using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;
using System;
using SDGame.UITools;

public class UGUIManager : SingletonDontDestroy<UGUIManager> {

    private Dictionary<UGUI_TYPE, IWindowBase> m_loadedWindow = new Dictionary<UGUI_TYPE, IWindowBase>(); //记录加载过的UI
    private string m_configPath = "Assets/Resources/UIConfig.asset";

    private UIRoot2D m_root2D;
    private GameObject EventSystem;

    private void Awake()
    {
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

    public IWindowBase Open(UGUI_TYPE uiType)
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

        }
        uiwindow.Open();

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

    
}
