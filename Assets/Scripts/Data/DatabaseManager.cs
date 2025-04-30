using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DatabaseManager : MonoBehaviour {
    public ServerData serverData = new();
    public PlayerData playerData;
    [Inject] private ApiManager apiManager;

    private void Awake() {
        GetServerData();
        GetUserData();
    }

    private void GetServerData()
    {
        apiManager.Fetch<DictionaryData>(TableNames.DICTIONARIES_TABLE, data => serverData.Dictionaries = data, OnError);        
        apiManager.Fetch<WordData>(TableNames.WORDS_TABLE, data => serverData.Words = data, OnError);        
        apiManager.Fetch<LevelData>(TableNames.LEVELS_TABLE, data => serverData.Levels = data, OnError);        
        apiManager.Fetch<LevelWordData>(TableNames.LEVEL_WORDS_TABLE, data => serverData.LevelWords = data, OnError);  

        void OnError(long code, string error)
        {
            Debug.LogError("Failed to fetch players: " + error);
        }
    }

    private void GetUserData(){
        var userId = DeviceIdManager.GetUserId();
        apiManager.GetByUserId(TableNames.PLAYERS_TABLE, userId, data => playerData = JsonUtilityWrapper.FromJsonSingle<PlayerData>(data), OnError);   

        void OnError(long code, string error)
        {
            if (code == 404)
            {
                PlayerData newPlayer = new() {
                    userid = userId,
                    name = "Anastasiia",
                    curlevel = 0,
                    score = 0
                };
                string json = JsonUtility.ToJson(newPlayer);
                apiManager.Post<PlayerData>(TableNames.PLAYERS_TABLE, json, OnSuccess, OnError);
            } 
            else
                Debug.LogError("Failed to fetch this player: " + error);
        } 

        void OnSuccess(string data){
            Debug.Log($"Posted player: {data}");
            apiManager.GetByUserId(TableNames.PLAYERS_TABLE, userId, data => playerData = JsonUtilityWrapper.FromJsonSingle<PlayerData>(data), OnError);   
        } 
    }
}

[Serializable]
public class ServerData
{
    public List<DictionaryData> Dictionaries;
    public List<WordData> Words;
    public List<LevelData> Levels;
    public List<LevelWordData> LevelWords;
}