using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Text;

public class ApiClient : MonoBehaviour
{
    public void Get(string url, Action<string> onSuccess, Action<long, string> onError)
    {
        StartCoroutine(GetCoroutine(url, onSuccess, onError));
    }

    public void GetById(string url, int id, Action<string> onSuccess, Action<long, string> onError)
    {
        StartCoroutine(GetCoroutine($"{url}/{id}", onSuccess, onError));
    }

    public void GetByUserId(string url, string userId, Action<string> onSuccess, Action<long, string> onError)
    {
        string fullUrl = $"{url}/{UnityWebRequest.EscapeURL(userId)}";
        StartCoroutine(GetCoroutine(fullUrl, onSuccess, onError));
    }

    public void Post(string url, string jsonData, Action<string> onSuccess, Action<long, string> onError)
    {
        StartCoroutine(PostCoroutine(url, jsonData, onSuccess, onError));
    }

    public void Put(string url, string jsonData, Action<string> onSuccess, Action<long, string> onError)
    {
        StartCoroutine(PutCoroutine($"{url}", jsonData, onSuccess, onError));
    }

    public void Delete(string url, int id, Action<string> onSuccess, Action<long, string> onError)
    {
        StartCoroutine(DeleteCoroutine($"{url}/{id}", onSuccess, onError));
    }

    private IEnumerator GetCoroutine(string url, Action<string> onSuccess, Action<long, string> onError)
    {
        using UnityWebRequest request = UnityWebRequest.Get(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            onError?.Invoke(request.responseCode, request.downloadHandler.text);
        else
            onSuccess?.Invoke(request.downloadHandler.text);
    }

    private IEnumerator PostCoroutine(string url, string jsonData, Action<string> onSuccess, Action<long, string> onError)
    {
        using UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            onError?.Invoke(request.responseCode, request.downloadHandler.text);
        else
            onSuccess?.Invoke(request.downloadHandler.text);
    }

    private IEnumerator PutCoroutine(string url, string jsonData, Action<string> onSuccess, Action<long, string> onError)
    {
        using UnityWebRequest request = new UnityWebRequest(url, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            onError?.Invoke(request.responseCode, request.downloadHandler.text);
        else
            onSuccess?.Invoke(request.downloadHandler.text);
    }

    private IEnumerator DeleteCoroutine(string url, Action<string> onSuccess, Action<long, string> onError)
    {
        using UnityWebRequest request = UnityWebRequest.Delete(url);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            onError?.Invoke(request.responseCode, request.downloadHandler.text);
        else
            onSuccess?.Invoke(request.result.ToString());
    }
}