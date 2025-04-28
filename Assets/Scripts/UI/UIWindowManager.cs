using System;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowManager : MonoBehaviour
{
    [SerializeField]
    private List<WindowData> windows;

    [SerializeField]
    private List<WindowData> popups;

    void Start()
    {
        foreach (var item in windows)
            item.window.SetActive(item.id == windows[0].id? true : false);
        foreach (var item in popups)
            item.window.SetActive(false);
    }

    public void OpenWindow(string id){
        foreach (var item in windows)
            item.window.SetActive(item.id == id);
    } 

    public void OpenPopup(string id){
        var popup = popups.Find(popup => popup.id == id);
        popup.window.SetActive(true);
    } 

    public void ClosePopup(string id){
        var popup = popups.Find(popup => popup.id == id);
        popup.window.SetActive(false);
    } 
}

[Serializable]
public class WindowData{
    public GameObject window;
    public string id;
}
