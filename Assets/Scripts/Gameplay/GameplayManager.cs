using UnityEngine;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    //[Inject] private readonly LevelSettings levelSettings;
    [Inject] private UIWindowManager windowManager;
    private int openedWindowId;
    private LevelConfig lvlConfig;

    public void StartLevel(LevelConfig config){
        lvlConfig = config;
        openedWindowId = 0;
        windowManager.OpenWindow(config.windowIds[0]);
    }

    public bool TryOpenNextWindow(){
        if (lvlConfig.windowIds.Count > openedWindowId + 1)
        {
            openedWindowId++;
            windowManager.OpenWindow(lvlConfig.windowIds[openedWindowId]);
            return true;
        }
        else return false;
    }
}