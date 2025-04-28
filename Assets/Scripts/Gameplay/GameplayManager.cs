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
            InitializeGameplayWindow(window);
            windowManager.OpenWindow(lvlConfig.windowIds[openedWindowId]);
        }
        else{
            windowManager.OpenWindow(WindowNames.MAIN_WINDOW);
        }
    }

    private void InitializeGameplayWindow(BasicGameplayWindow window){
        window.Initialize("тато"); //change later
        window.OnContinue += TryOpenNextWindow;
        window.SetVisual();
    }
}