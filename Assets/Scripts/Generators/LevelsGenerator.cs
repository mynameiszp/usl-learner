using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelsGenerator : MonoBehaviour, IGenerator
{
    [Inject] private readonly LevelSettings levelSettings;
    [Inject] private DiContainer _diContainer;

    private Transform parent;
    private StartGame levelPrefab;
    private List<StartGame> levels = new();
    public int blockId;

    public void Initialize(int blockId, Transform parent, StartGame prefab)
    {
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
            var lvlObj = _diContainer.InstantiatePrefab(levelPrefab, parent).GetComponent<StartGame>();
            lvlObj.Initialize(item);
            levels.Add(lvlObj);
        }
    }
}