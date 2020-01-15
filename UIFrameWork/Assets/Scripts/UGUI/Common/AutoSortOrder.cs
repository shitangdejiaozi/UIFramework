using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class AutoSortOrder : MonoBehaviour {

    [SerializeField]
    private bool m_is3D = false;
    [SerializeField]
    private Canvas m_rootCanvas = null;
    [SerializeField]
    private int m_sortOffset = 1;

    private int m_order = 1;
    private Canvas m_selfCanvas = null;
	// Use this for initialization
	void Start () {

        Debug.LogError("auto sort start");
		if(!m_is3D)
        {
            m_selfCanvas = GetComponent<Canvas>();
            if(m_selfCanvas == null)
            {
                m_selfCanvas = gameObject.AddComponent<Canvas>();
                gameObject.AddComponent<GraphicRaycaster>();
            }

        }
        Sort();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetOrder(Canvas canvas, bool is3D = false, int sortOffset = 1)
    {
        m_is3D = is3D;
        m_rootCanvas = canvas;
        m_sortOffset = sortOffset;
        Sort();
    }

    private void Sort()
    {
        Debug.LogError("Sort");
        if(m_rootCanvas != null)
        {
            m_order = m_rootCanvas.sortingOrder + m_sortOffset;
        }

        if(m_is3D)
        {
            Renderer[] renders = GetComponentsInChildren<Renderer>(true);
            foreach (var render in renders)
            {
                render.sortingOrder = m_order;
            }
        }
        else
        {
            if(m_selfCanvas == null)
            {
                m_selfCanvas = GetComponent<Canvas>();
            }
            m_selfCanvas.overrideSorting = true;
            m_selfCanvas.sortingOrder = m_order;
        }
    }

}
