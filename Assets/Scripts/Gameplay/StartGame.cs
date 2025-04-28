using UnityEngine;
using Zenject;

public class StartGame : MonoBehaviour
{
    [Inject] private GameplayManager gpManager;
    private int levelNumber;

    public void Initialize(int lvlNumber)
    {
        levelNumber = lvlNumber;
    }

    public void StartLevel(){
        gpManager.StartLevel(levelNumber);
    }
}
