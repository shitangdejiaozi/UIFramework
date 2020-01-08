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
}
