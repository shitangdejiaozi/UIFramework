using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SDGame.UITools;

public interface IWindowBase : IBindableUI{

    void Open();

    void Close();

    void Release();

    bool Is3D();
}
