using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameplayManager : MonoBehaviour
{
    [Inject] private readonly DatabaseManager dbManager;
    [Inject] private UIWindowManager windowManager;
    public Action<LevelFinishedData> OnLevelWin;
    private int openedWindowId = 0;
    private int curWord = 0;
    private LevelConfig lvlConfig;
    private List<LevelWordData> wordsForLevel;
    private LevelData levelData;
    private float score;

    public void StartLevel(LevelConfig config){
        lvlConfig = config;
        wordsForLevel = dbManager.serverData.LevelWords.Where(word => word.levelid == lvlConfig.levelNumber).ToList();
        levelData = dbManager.serverData.Levels.Find(l => l.level == lvlConfig.levelNumber);
        score = levelData.points;
        TryOpenNextWindow();
    }

    private void ProcessResult(bool isCorrect){
        score -= isCorrect? 0: (levelData.points / lvlConfig.windowIds.Count);
        TryOpenNextWindow();
    }

    private void TryOpenNextWindow(){
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
        if(curWord == wordsForLevel.Count){
            var levelData = new LevelFinishedData(lvlConfig.levelNumber, (int)score);
            OnLevelWin?.Invoke(levelData);
            curWord = 0;
            openedWindowId = 0;
            score = 0;
            var window = windowManager.GetWindow(WindowNames.STATS_WINDOW).GetComponent<LevelStatsWindow>();
            window.Initialize(levelData);
            windowManager.OpenWindow(WindowNames.STATS_WINDOW);
        }
    }

    private void InitializeGameplayWindow(LevelConfig config, string word, BasicGameplayWindow window){
        window.OnContinue -= ProcessResult;
        window.Initialize(config.levelNumber, word);
        window.OnContinue += ProcessResult;
        window.SetVisual();
    }
}

[Serializable]
public class LevelFinishedData
{
    public int level;
    public int addScore;

    public LevelFinishedData(int lvl, int score)
    {
        level = lvl;
        addScore = score;
    }
}