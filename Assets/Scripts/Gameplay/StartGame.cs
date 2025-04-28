using UnityEngine;
using Zenject;

public class StartGame : MonoBehaviour
{
    [Inject] private GameplayManager gpManager;
    private LevelConfig lvlConfig;

    public void Initialize(LevelConfig config)
    {
        lvlConfig = config;
    }

    public void StartLevel(){
        gpManager.StartLevel(lvlConfig);
    }
}
