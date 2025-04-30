using UnityEngine;
using Zenject;
using TMPro;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNum;
    [Inject] private GameplayManager gpManager;
    private LevelConfig lvlConfig;

    public void Initialize(LevelConfig config)
    {
        lvlConfig = config;
        levelNum.text = config.levelNumber.ToString();
    }

    public void StartLevel(){
        gpManager.StartLevel(lvlConfig);
    }
}
