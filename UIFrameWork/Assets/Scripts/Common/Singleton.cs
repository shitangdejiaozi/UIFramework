using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : Singleton<T>, new()
{
    private static readonly object synObject = new object();
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if(m_instance == null)
            {
                lock(synObject)
                {
                    if (m_instance == null) //双检测，减少操作次数
                        m_instance = new T();
                }
            }
            return m_instance;
        }
    }
	
}

public class SingletonDontDestroy<T> : MonoBehaviour where T : Component
{
    private static readonly object synObject = new object();
    private static T m_instance;

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
               
                lock (synObject)
                {
                    m_instance = FindObjectOfType(typeof(T)) as T;
                    if (m_instance == null) //双检测，减少操作次数
                    {
                        
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).Name;
                        m_instance = (T)obj.AddComponent(typeof(T));
                    }
                    if (Application.isPlaying)
                        GameObject.DontDestroyOnLoad(m_instance);
                        
                }
            }
            return m_instance;
        }
    }
}
