using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DictionaryWindowVisual : MonoBehaviour
{
    [SerializeField]
    private Button enterButton;
    [SerializeField]
    private Button voiceButton;

    void Start()
    {
        voiceButton.onClick.AddListener(OnVoiceButtonClick);
    }

    void Update()
    {
        
    }

    private void OnVoiceButtonClick()
    {
    }

    private void OnDestroy()
    {
        voiceButton.onClick.RemoveAllListeners();
    }
}
