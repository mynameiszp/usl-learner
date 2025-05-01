using Zenject;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class BlocksGenerator : MonoBehaviour, IGenerator
{
    [Inject] private readonly DatabaseManager dbManager;
    [Inject] private DiContainer _diContainer;
    [Header("Block config")]
    [SerializeField] private Transform parent;
    [SerializeField] private LevelsGenerator blockPrefab;
    [Header("Levels config")]
    [SerializeField] private Transform levelsParent;
    [SerializeField] private StartGame levelPrefab;

    private List<LevelsGenerator> blocks = new();

    public IEnumerator Generate()
    {
        while (dbManager.serverData.Dictionaries.Count == 0)
            yield return null;

        foreach (var item in blocks){
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(item.gameObject);
        }
        blocks.Clear();

        var dictionaries = dbManager.serverData.Dictionaries;

        for (int i = 0; i < dictionaries.Count; i++)
        {
            var blockObj = _diContainer.InstantiatePrefab(blockPrefab, parent).GetComponent<LevelsGenerator>();
            blockObj.Initialize(i, dictionaries[i].name, levelsParent, levelPrefab);
            blocks.Add(blockObj);

            blockObj.GetComponent<Button>().onClick.AddListener(() => GenerateLevels(blockObj));
        }
    }

    public void GenerateLevels(LevelsGenerator block){
        StartCoroutine(block.Generate());
    }

    void OnDestroy()
    {
        foreach (var item in blocks){
            item.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(item.gameObject);
        }
        blocks.Clear();
    }
}