using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public static List<Window> windows;

    public bool isOpen
    {
        get { return gameObject.activeInHierarchy; }
    }
    public virtual void Open()
    {
        if(CurrentWindow!=null)
        CurrentWindow.Close();
        gameObject.SetActive(true);
    }

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
    protected virtual void Awake()
    {
        if(windows == null)
        {
            windows = new List<Window>();
        }
        windows.Add(this);
    }
    public Window CurrentWindow
    {
        get
        {
            return windows.Find((x) => x.isOpen);
        }
    }
}
