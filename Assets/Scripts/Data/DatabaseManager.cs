using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DatabaseManager : MonoBehaviour {
    public ServerData serverData = new();
    public PlayerData playerData;
    [Inject] private ApiManager apiManager;
    [Inject] private GameplayManager gameplayManager;

    private void Awake() {
        GetServerData();
        GetUserData();
        gameplayManager.OnLevelWin += IncreasePlayerLevel;
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
                    name = "Player1",
                    curlevel = 1,
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

    private void IncreasePlayerLevel(LevelFinishedData data)
    {
        if (data.level + 1 > playerData.curlevel)
            playerData.curlevel = data.level + 1;
        playerData.score += data.addScore;
        UpdateUserStats();
    }

    private void UpdateUserStats()
    {
        var updateData = new PlayerStats { curlevel = playerData.curlevel, score = playerData.score };
        string jsonData = JsonUtility.ToJson(updateData);
        apiManager.UpdatePlayerStats(playerData.id, TableNames.PLAYERS_TABLE, jsonData, OnSuccess, OnError);  

        void OnSuccess(string response)
        {
            Debug.Log("Changed player stats: " + response);
        }

        void OnError(long code, string error)
        {
            Debug.LogError("Failed to change player stats: " + error);
        } 
    }

    public void UpdateUserName(string newName)
    {
        var updateData = new PlayerNameData { name = newName };
        string jsonData = JsonUtility.ToJson(updateData);
        apiManager.UpdatePlayerName(playerData.id, TableNames.PLAYERS_TABLE, jsonData, OnSuccess, OnError);  
        playerData.name = newName;

        void OnSuccess(string response)
        {
            Debug.Log("Changed player name: " + response);
        }

        void OnError(long code, string error)
        {
            Debug.LogError("Failed to change player name: " + error);
        } 
    }

    public void DeletePlayer()
    {
        apiManager.Delete(playerData.id, TableNames.PLAYERS_TABLE, OnSuccess, OnError);  

        void OnSuccess(string response)
        {
            Debug.Log("Deleted player: " + response);
            Application.Quit();
        }

        void OnError(long code, string error)
        {
            Debug.LogError("Failed to delete player: " + error);
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

[Serializable]
public class PlayerStats
{
    public int curlevel;
    public int score;
}

[Serializable]
public class PlayerNameData
{
    public string name;
}