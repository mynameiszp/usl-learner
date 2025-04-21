using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowManager : MonoBehaviour
{
    [SerializeField]
    private List<WindowData> windows;

    void Start()
    {
        foreach (var item in windows)
            item.window.SetActive(item.id == windows[0].id? true : false);
    }

    public void OpenWindow(string id){
        foreach (var item in windows)
            item.window.SetActive(item.id == id);
    } 
}

[Serializable]
public class WindowData{
    public GameObject window;
    public string id;
}
