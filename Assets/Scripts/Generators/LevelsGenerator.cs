using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelsGenerator : MonoBehaviour, IGenerator
{
    private LevelSettings levelSettings;
    private Transform parent;
    private StartGame levelPrefab;
    private List<StartGame> levels = new();
    public int blockId;

    public void Initialize(LevelSettings settings, int blockId, Transform parent, StartGame prefab)
    {
        levelSettings = settings;
        this.blockId = blockId;
        this.parent = parent;
        levelPrefab = prefab;
    }

    public void Generate()
    {
        foreach (var item in levels){
            Destroy(item.gameObject);
        }
        levels.Clear();

        foreach (var item in levelSettings.blockConfigs[blockId].levelConfigs)
        {
            var lvlObj = Instantiate(levelPrefab, parent);
            lvlObj.Initialize(item.levelNumber);
            levels.Add(lvlObj);
        }
    }
}