using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AudioRecorder : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private AudioClip recordedClip;
    private string filePath;
    [SerializeField]
    GoogleServicesController google;
    [SerializeField]
    Button recordButton;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "recordedAudio.wav");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartRecording();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopRecording();
    }

    public void StartRecording()
    {
        string device = Microphone.devices[0];
        int sampleRate = 44100;
        int lengthSec = 3599;
        recordedClip = Microphone.Start(device, false, lengthSec, sampleRate);
        Debug.Log($"Recording started");
    }

    public void StopRecording()
    {
        Microphone.End(null);
        SaveWav(filePath, recordedClip);
    }

    void SaveWav(string path, AudioClip clip)
    {
        // Convert AudioClip to PCM Data
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);

        byte[] wavData = ConvertToWav(samples, clip.frequency, clip.channels);

        // Save to file
        File.WriteAllBytes(path, wavData);
        Debug.Log($"Bites written");
        google.UploadAudio();
    }

    byte[] ConvertToWav(float[] samples, int sampleRate, int channels)
    {
        using (MemoryStream stream = new MemoryStream())
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            int byteRate = sampleRate * channels * 2;
            int dataSize = samples.Length * 2;

            // **WAV Header (44 bytes)**
            writer.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"));       // Chunk ID
            writer.Write(36 + dataSize);                                   // Chunk Size
            writer.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"));       // Format

            // Subchunk1 "fmt "
            writer.Write(System.Text.Encoding.UTF8.GetBytes("fmt "));       // Subchunk1 ID
            writer.Write(16);                                              // Subchunk1 Size
            writer.Write((short)1);                                        // Audio Format (PCM = 1)
            writer.Write((short)channels);                                 // Num Channels
            writer.Write(sampleRate);                                      // Sample Rate
            writer.Write(byteRate);                                        // Byte Rate
            writer.Write((short)(channels * 2));                          // Block Align
            writer.Write((short)16);                                       // Bits per Sample

            // Subchunk2 "data"
            writer.Write(System.Text.Encoding.UTF8.GetBytes("data"));       // Subchunk2 ID
            writer.Write(dataSize);                                        // Subchunk2 Size

            // Write PCM data
            foreach (float sample in samples)
            {
                short pcmSample = (short)(sample * short.MaxValue);
                writer.Write(pcmSample);
            }

            return stream.ToArray();
        }
    }
}