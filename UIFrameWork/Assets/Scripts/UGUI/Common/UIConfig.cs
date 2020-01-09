using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="UI/Build UIconfig", fileName = "Assets/Resources/UIConfig")]
public class UIConfig : ScriptableObject {

    public List<UIData> UIDatas = new List<UIData>();
}

[System.Serializable]
public class UIData 
{
    public string UIName;
    public string ScriptName;
    public string PrefabName;
    public bool UseLua ;
    public string LuaName;
}
