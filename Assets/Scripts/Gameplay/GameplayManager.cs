using UnityEngine;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    //[Inject] private readonly LevelSettings levelSettings;
    [Inject] private UIWindowManager windowManager;
    private int openedWindowId = -1;
    private LevelConfig lvlConfig;

    public void StartLevel(LevelConfig config){
        lvlConfig = config;
        TryOpenNextWindow();
    }

    public void TryOpenNextWindow(){
        if (lvlConfig.windowIds.Count > openedWindowId + 1)
        {
            openedWindowId++;
            var window = windowManager.GetWindow(lvlConfig.windowIds[openedWindowId]).GetComponent<BasicGameplayWindow>();
            InitializeGameplayWindow(lvlConfig, window);
            windowManager.OpenWindow(lvlConfig.windowIds[openedWindowId]);
        }
        else{
            windowManager.OpenWindow(WindowNames.MAIN_WINDOW);
        }
    }

    private void InitializeGameplayWindow(LevelConfig config, BasicGameplayWindow window){
        window.Initialize(config.levelNumber, "тато"); //change later
        window.OnContinue += TryOpenNextWindow;
        window.SetVisual();
    }
}