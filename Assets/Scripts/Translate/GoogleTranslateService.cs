using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleTranslateService : MonoBehaviour
{
    private const string API_KEY = APIKeys.TRANSLATE_API_KEY;
    private const string DEFAULT_LANG_CODE = "uk";

    public void TranslateText(string textToTranslate, string sourceLanguage, Action<string> callback)
    {
        string url = $"https://translation.googleapis.com/language/translate/v2?key={API_KEY}";

        TranslationRequest requestData = new TranslationRequest()
        {
            q = textToTranslate,
            target = DEFAULT_LANG_CODE,
            source = sourceLanguage
        };

        Debug.Log("sourceLanguage " + sourceLanguage);


        StartCoroutine(SendTranslationRequest(url, requestData, callback));
    }

    public void GetSupportedLanguages(Action<LanguageList> callback)
    {
        string url = $"https://translation.googleapis.com/language/translate/v2/languages?key={API_KEY}&target=uk";
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

    private IEnumerator SendTranslationRequest(string url, TranslationRequest data, Action<string> callback)
    {
        string result = "вибачте, сталася помилка";
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
            result = response.data.translations[0].translatedText;
            Debug.Log("Translated Text: " + response.data.translations[0].translatedText);
        }
        else
        {
            Debug.LogError("Translation Error: " + request.downloadHandler.text);
        }
        callback?.Invoke(result);
    }
}
