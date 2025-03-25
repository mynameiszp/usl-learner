using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleServicesController : MonoBehaviour
{
    private const string apiKey = APIKeys.TRANSLATE_API_KEY;
    private string filePath;
    private int chunkIndex = 0;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
    }

    public void UploadAudio()
    {
        StartCoroutine(ProcessAudioChunks(filePath));
    }

    private IEnumerator ProcessAudioChunks(string filePath)
    {
        byte[] fullAudio = File.ReadAllBytes(filePath);
        int maxChunkSize = 10485760; // 10MB limit per request

        int chunkSize = Mathf.Min(maxChunkSize, fullAudio.Length / (fullAudio.Length / (44100 * 2 * 60))); // 60 sec chunks
        int totalChunks = Mathf.CeilToInt((float)fullAudio.Length / chunkSize);

        for (int i = 0; i < totalChunks; i++)
        {
            byte[] chunk = new byte[Mathf.Min(chunkSize, fullAudio.Length - (i * chunkSize))];
            Array.Copy(fullAudio, i * chunkSize, chunk, 0, chunk.Length);

            string base64Chunk = Convert.ToBase64String(chunk);
            SendAudioChunkToGoogle(base64Chunk, i + 1, totalChunks);

            yield return new WaitForSeconds(1); // Slight delay between API calls
        }
    }

    private void SendAudioChunkToGoogle(string audioBase64, int chunkNumber, int totalChunks)
    {
        var dataToSend = new SpeechRecognitionRequest
        {
            config = new RecognitionConfig()
            {
                encoding = "LINEAR16",
                sampleRateHertz = 44100,
                languageCode = "en-US",
                enableAutomaticPunctuation = true
            },
            audio = new RecognitionAudio()
            {
                content = audioBase64
            }
        };

        string url = $"https://speech.googleapis.com/v1/speech:recognize?key={apiKey}";
        RequestService.SendDataToGoogle(url, dataToSend, response => TranscriptionSuccess(response, chunkNumber, totalChunks), TranscriptionError);
    }

    private void TranscriptionSuccess(string response, int chunkNumber, int totalChunks)
    {
        Debug.Log($"✅ Chunk {chunkNumber}/{totalChunks} Transcribed: {response}");
    }

    private void TranscriptionError(BadRequestData error)
    {
        Debug.LogError($"❌ Google API Error: {error.error.message}");
    }

    // --------- JSON Data Classes ---------
    [Serializable]
    public class SpeechRecognitionRequest
    {
        public RecognitionConfig config;
        public RecognitionAudio audio;
    }

    [Serializable]
    public class RecognitionConfig
    {
        public string encoding;
        public int sampleRateHertz;
        public string languageCode;
        public bool enableAutomaticPunctuation;
    }

    [Serializable]
    public class RecognitionAudio
    {
        public string content;
    }
}

// ------------ API Request Handler -------------
public class RequestService : MonoBehaviour
{
    public static void SendDataToGoogle(string url, object dataToSend, Action<string> onSuccess, Action<BadRequestData> onError)
    {
        string jsonData = JsonUtility.ToJson(dataToSend);
        Debug.Log($"📡 Sending Data to Google: {jsonData}");
        Post(url, jsonData, onSuccess, onError);
    }

    private static async void Post(string url, string jsonData, Action<string> onSuccess, Action<BadRequestData> onError)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        var operation = request.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        if (HasError(request, out var errorData))
        {
            onError?.Invoke(errorData);
        }
        else
        {
            onSuccess?.Invoke(request.downloadHandler.text);
        }

        request.Dispose();
    }

    private static bool HasError(UnityWebRequest request, out BadRequestData badRequestData)
    {
        if (request.responseCode == 200)
        {
            badRequestData = null;
            return false;
        }

        try
        {
            badRequestData = JsonUtility.FromJson<BadRequestData>(request.downloadHandler.text);
            return true;
        }
        catch
        {
            badRequestData = new BadRequestData
            {
                error = new Error
                {
                    code = (int)request.responseCode,
                    message = request.error
                }
            };
            return true;
        }
    }
}

// ------------ Error Handling Classes -------------
[Serializable]
public class BadRequestData
{
    public Error error;
}

[Serializable]
public class Error
{
    public int code;
    public string message;
}
