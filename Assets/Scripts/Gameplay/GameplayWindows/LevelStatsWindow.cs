using UnityEngine;
using TMPro;
using Zenject;

public class LevelStatsWindow : MonoBehaviour
{
    [Inject] private UIWindowManager windowManager;
    [Inject] private GameWindowUI gameWindowUI;

    [SerializeField] protected TMP_Text lvl;
    [SerializeField] protected TMP_Text addScore;

    public void Initialize(LevelFinishedData data){
        lvl.text = data.level.ToString();
        addScore.text = data.addScore.ToString();
    }

    public void OnContinue(){
        windowManager.OpenWindow(WindowNames.MAIN_WINDOW);
        gameWindowUI.Generate();
    }
}