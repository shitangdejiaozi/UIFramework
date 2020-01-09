using UnityEngine;
using XLua;

public class DemoTest : MonoBehaviour {

    LuaEnv luaEnv = new LuaEnv();
	// Use this for initialization
	void Start () {
        AddLuaLoader();
        luaEnv.DoString("require 'Common/main'");
        UGUIManager.Instance.Open(UGUI_TYPE.UITest);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        luaEnv.Dispose();
    }

    void AddLuaLoader()
    {
        luaEnv.AddLoader((ref string filename) => {
            byte[] luaBytes = null;
            bool isLoadText = true;
            if(!(filename.StartsWith("xlua") || filename.StartsWith("perf") || filename.StartsWith("tdr")))
            {
#if UNITY_EDITOR
                filename = "Lua/" + filename; //Resources下路径
#else
                isLoadText = false;
#endif
            }
            filename = filename.Replace('.', '/');
            if(isLoadText)
            {
                if (!filename.EndsWith(".lua"))
                {
                    filename = filename + ".lua";
                }

                TextAsset asset = Resources.Load<TextAsset>(filename); //编辑器下作为Text
                luaBytes = asset.bytes;
            }
            else
            {
                if (!filename.EndsWith(".lua.txt"))
                {
                    filename = filename + ".lua.txt";
                }
                //从udpate文件夹中读取
            }


            if (luaBytes == null)
            {
                Debug.LogError(" load file" + filename + "error");
                return null;
            }
            return luaBytes;
        });
    }
}
