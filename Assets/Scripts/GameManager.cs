using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    //[Inject] DBController dBController;
    //[Inject] Data dBController;

    private void Awake()
    {
        Debug.Log("Awake");
        // dBController.CreateDatabase();
        // dBController.CreateTable<CustomDictionary>();
        // dBController.CreateTable<Word>();
        // dBController.CreateTable<DictionaryWord>();
        // dBController.CreateTable<SynonymPair>();
        // dBController.CreateTable<AntonymPair>();
        // Debug.Log("Created all");
        // dBController.Insert(new CustomDictionary("Dictionary1"));
        // Debug.Log("Inserted all");
        // Debug.Log($"Get dictionary 1: {dBController.Get<CustomDictionary>(1).ToString()}");
    }
}
