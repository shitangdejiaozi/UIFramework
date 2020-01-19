using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IData
{
    void Initialize();
    void Release();
}

public class DataManager : Singleton<DataManager>
{

    private Dictionary<Type, IData> m_dicData = new Dictionary<Type, IData>();
    
    public T Access<T>()
    {
        Type type = typeof(T);
        IData data;
        if(!m_dicData.TryGetValue(type, out data))
        {
            data = Activator.CreateInstance<T>() as IData;
            m_dicData.Add(type, data);
            data.Initialize();
        }
        return (T)data;
    }

    public void Clear<T>()
    {
        Type type = typeof(T);
        if(m_dicData.ContainsKey(type))
        {
            m_dicData[type].Release();
            m_dicData.Remove(type);
        }
    }
	
    public void ClearAll()
    {

    }
}
