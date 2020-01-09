using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UGUITools  {

	public static T AddChild<T>(this GameObject parent) where T : Component
    {
        GameObject go = AddChild(parent);
        string name = typeof(T).ToString();
        go.name = name;
        return go.AddComponent<T>();
    }

    public static GameObject AddChild(this GameObject parent)
    {
        var go = new GameObject();
        if(parent != null)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }
}
