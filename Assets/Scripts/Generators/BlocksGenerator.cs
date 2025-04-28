using Zenject;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BlocksGenerator : MonoBehaviour, IGenerator
{
    [Inject] private readonly LevelSettings levelSettings;
    [Header("Block config")]
    [SerializeField] private Transform parent;
    [SerializeField] private LevelsGenerator blockPrefab;
    [Header("Levels config")]
    [SerializeField] private Transform levelsParent;
    [SerializeField] private StartGame levelPrefab;

    private List<LevelsGenerator> blocks;

    public void Generate()
    {
        blocks = new List<LevelsGenerator>();

        for (int i = 0; i < levelSettings.blockConfigs.Count; i++)
        {
            var blockObj = Instantiate(blockPrefab, Vector3.zero, Quaternion.identity, parent);
            blockObj.Initialize(levelSettings, i, levelsParent, levelPrefab);
            blocks.Add(blockObj);

            blockObj.GetComponent<Button>().onClick.AddListener(() => GenerateLevels(blockObj));
        }
    }

    public void GenerateLevels(LevelsGenerator block){
        block.Generate();
    }
}