using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ApiManager : MonoBehaviour
{
    private const string ServerIp = "https://4b02-93-170-44-43.ngrok-free.app";
    [Inject] private ApiClient apiClient;

    public void Fetch<T>(string table, Action<List<T>> onSuccess, Action<long, string> onError)
    {
        apiClient.Get($"{ServerIp}/{table}", OnSuccess, OnError);

        void OnSuccess(string json)
        {
            List<T> data = JsonUtilityWrapper.FromJsonList<T>(json);
            Debug.Log($"Fetched {data.Count} {typeof(T)}.");
            onSuccess?.Invoke(data);
        }

        void OnError(long code, string error)
        {
            Debug.LogError($"Fetch failed: {code} + {error}");
            onError?.Invoke(code, error);
        }
    }

    public void GetByUserId(string table, string userId, Action<string> onSuccess, Action<long, string> onError)
    {
        apiClient.GetByUserId($"{ServerIp}/{table}", userId, OnSuccess, OnError);

        void OnSuccess(string json)
        {
            onSuccess?.Invoke(json);
        }

        void OnError(long code, string error)
        {
            Debug.Log($"Fetch failed: {code} + {error}");
            onError?.Invoke(code, error);
        }
    }

    public void Post<T>(string table, string jsonData, Action<string> onSuccess, Action<long, string> onError)
    {
        apiClient.Post($"{ServerIp}/{table}", jsonData, OnSuccess, OnError);

        void OnSuccess(string json)
        {
            onSuccess?.Invoke(json);
            Debug.Log($"Posted {json} {typeof(T)}.");
        }

        void OnError(long code, string error)
        {
            Debug.LogError($"Post failed: {code} + {error}");
            onError?.Invoke(code, error);
        }
    }
}

public static class JsonUtilityWrapper {
    [System.Serializable]
    private class Wrapper<T> { public List<T> items; }

    public static List<T> FromJsonList<T>(string json) {
        return JsonUtility.FromJson<Wrapper<T>>("{\"items\":" + json + "}").items;
    }

    public static T FromJsonSingle<T>(string json)
    {
        return JsonUtility.FromJson<T>(json);
    }
}

public static class TableNames{
    public const string PLAYERS_TABLE = "players";
    public const string DICTIONARIES_TABLE = "dictionaries";
    public const string WORDS_TABLE = "words";
    public const string LEVELS_TABLE = "levels";
    public const string LEVEL_WORDS_TABLE = "levelsWords";
}