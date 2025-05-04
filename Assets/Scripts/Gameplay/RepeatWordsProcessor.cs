using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class RepeatWordsProcessor : MonoBehaviour
{
    [Inject] private readonly LevelSettings levelSettings;
    [Inject] private readonly DatabaseManager dbManager;
    [Inject] private UIWindowManager windowManager;
    private int openedWindowId = 0;
    private int curWord = 0;
    private LevelConfig lvlConfig;
    private List<LevelWordData> wordsForLevel;

    public void StartRepeat(int curlevel)
    {
        lvlConfig = levelSettings.repeatLevel;
        wordsForLevel = dbManager.serverData.LevelWords.Where(word => word.levelid < curlevel).ToList();
        TryOpenNextWindow();
    }

    private void ProcessResult(bool isCorrect){
        TryOpenNextWindow();
    }

    private void TryOpenNextWindow(){
        if(curWord == wordsForLevel.Count - 1){
            curWord = 0;
            openedWindowId = 0;
            windowManager.OpenWindow(WindowNames.PROFILE_WINDOW);
            return;
        }
        if(curWord < wordsForLevel.Count){
            if (openedWindowId == lvlConfig.windowIds.Count)
            {
                curWord++;
                openedWindowId = 0;
            }
            var window = windowManager.GetWindow(lvlConfig.windowIds[openedWindowId]).GetComponent<BasicGameplayWindow>();
            var word = dbManager.serverData.Words.Find(word => word.id == wordsForLevel[curWord].wordid).name;
            InitializeGameplayWindow(lvlConfig, word, window);
            windowManager.OpenWindow(lvlConfig.windowIds[openedWindowId]);
            openedWindowId++;
        }
    }

    private void InitializeGameplayWindow(LevelConfig config, string word, BasicGameplayWindow window){
        window.OnContinue -= ProcessResult;
        window.Initialize(config.levelNumber, word);
        window.OnContinue += ProcessResult;
        window.SetVisual();
    }
}