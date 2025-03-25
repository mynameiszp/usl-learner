using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleTranslateService : MonoBehaviour
{
    private const string apiKey = APIKeys.TRANSLATE_API_KEY;

    public void TranslateText(string textToTranslate, string targetLanguage)
    {
        string url = $"https://translation.googleapis.com/language/translate/v2?key={apiKey}";

        TranslationRequest requestData = new TranslationRequest()
        {
            q = textToTranslate,
            target = targetLanguage
        };

        StartCoroutine(SendTranslationRequest(url, requestData));
    }

    public void GetSupportedLanguages(Action<LanguageList> callback)
    {
        string url = $"https://translation.googleapis.com/language/translate/v2/languages?key={apiKey}&target=uk";
        StartCoroutine(FetchSupportedLanguages(url, callback));
    }

    private IEnumerator FetchSupportedLanguages(string url, Action<LanguageList> callback)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        LanguageList languageList = new();
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Languages Response: " + request.downloadHandler.text);
            languageList = JsonUtility.FromJson<LanguageList>(request.downloadHandler.text);
        }
        else
        {
            Debug.LogError("Error Fetching Languages: " + request.error);
        }
        callback?.Invoke(languageList);
    }

    private IEnumerator SendTranslationRequest(string url, TranslationRequest data)
    {
        string jsonData = JsonUtility.ToJson(data);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            TranslationResponse response = JsonUtility.FromJson<TranslationResponse>(request.downloadHandler.text);
            Debug.Log("Translated Text: " + response.data.translations[0].translatedText);
        }
        else
        {
            Debug.LogError("Translation Error: " + request.downloadHandler.text);
        }
    }
}

[Serializable]
public class TranslationRequest
{
    public string q;       // Text to translate
    public string target;  // Target language code (e.g., "fr" for French)
}

[Serializable]
public class TranslationResponse
{
    public TranslationData data;
}

[Serializable]
public class TranslationData
{
    public Translation[] translations;
}

[Serializable]
public class Translation
{
    public string translatedText;
}

[Serializable]
public class LanguageList
{
    public Data data;
}

[Serializable]
public class Data
{
    public Language[] languages;
}

[Serializable]
public class Language
{
    public string language;
    public string name;
}