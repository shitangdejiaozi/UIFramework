using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonFunc  {

    /// <summary>
    /// 设置go的layer
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
	public static void SetLayer(GameObject go, string name)
    {
        int layer = LayerMask.NameToLayer(name);
        if(layer >= 0)
        {
            SetLayer(go, layer);
        }
    }

    private static List<Transform> m_transList = new List<Transform>();
    public static void SetLayer(GameObject go, int layer)
    {
        if (go == null)
            return;
        go.transform.GetComponentsInChildren(true, m_transList);
        for(int i = 0; i < m_transList.Count; i++)
        {
            GameObject child = m_transList[i].gameObject;
            if(child.layer !=  layer)
            {
                child.layer = layer;
            }
        }
    }

    public static GameObject GetUIEffect(string EffectName, Transform parent, Canvas canvas, int offsetOrder, LayerMask layer)
    {
        GameObject prefab = Resources.Load<GameObject>(EffectName);
        if(prefab == null)
        {
            Debug.LogError("effect no exit" + EffectName);
            return null;
        }

        GameObject go = GameObject.Instantiate(prefab, parent, false);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

        SetLayer(go, layer);
        var autoSort = go.GetOrAddComponent<AutoSortOrder>();
        if(autoSort != null)
        {
            autoSort.SetOrder(canvas, true, offsetOrder);

        }
        return go;
    }
}
