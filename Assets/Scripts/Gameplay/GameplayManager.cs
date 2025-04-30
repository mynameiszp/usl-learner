using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [Inject] private readonly DatabaseManager dbManager;
    [Inject] private UIWindowManager windowManager;
    private int openedWindowId = 0;
    private int curWord = 0;
    private LevelConfig lvlConfig;
    private List<LevelWordData> wordsForLevel;

    public void StartLevel(LevelConfig config){
        lvlConfig = config;
        wordsForLevel = dbManager.serverData.LevelWords.Where(word => word.levelid == lvlConfig.levelNumber).ToList();
        TryOpenNextWindow();
    }

    public void TryOpenNextWindow(){
        if(curWord < wordsForLevel.Count){
            if (openedWindowId < lvlConfig.windowIds.Count)
            {
                var window = windowManager.GetWindow(lvlConfig.windowIds[openedWindowId]).GetComponent<BasicGameplayWindow>();
                var word = dbManager.serverData.Words.Find(word => word.id == wordsForLevel[curWord].wordid).name;
                InitializeGameplayWindow(lvlConfig, word, window);
                windowManager.OpenWindow(lvlConfig.windowIds[openedWindowId]);
                openedWindowId++;
            }
            else{
                curWord++;
                openedWindowId = 0;
            }
        }
        else{
            windowManager.OpenWindow(WindowNames.MAIN_WINDOW);
            curWord = 0;
            openedWindowId = 0;
        }
    }

    private void InitializeGameplayWindow(LevelConfig config, string word, BasicGameplayWindow window){
        window.Initialize(config.levelNumber, word);
        window.OnContinue += TryOpenNextWindow;
        window.SetVisual();
    }
}