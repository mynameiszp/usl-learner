using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;

public class DictionaryWindowVisual : MonoBehaviour
{
    [SerializeField]
    private Button enterButton;
    [SerializeField]
    private Button voiceButton;
    [SerializeField]
    private TMP_InputField inputText;
    [SerializeField]
    private TMP_Dropdown languageDropdown;

    private string chosenLanguage;

    private LanguageList supportedLanguages;

    [Inject] private GoogleTranslateService translatorService;

    private void Start()
    {
        enterButton.onClick.AddListener(Translate);
        languageDropdown.onValueChanged.AddListener(OnLanguageValueChanged);
        InitLanguageDropdown();

        void OnLanguageValueChanged(int index)
        {
            chosenLanguage = languageDropdown.options[index].text;
        }
    }

    private void InitLanguageDropdown()
    {
        translatorService.GetSupportedLanguages(OnGetSupportedLanguages);

        void OnGetSupportedLanguages(LanguageList languageList)
        {
            supportedLanguages = languageList;
            languageDropdown.ClearOptions();
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            foreach (var lang in supportedLanguages.data.languages)
            {
                options.Add(new TMP_Dropdown.OptionData(lang.name));
            }
            languageDropdown.AddOptions(options);
        }
    }

    private void Translate()
    {
        translatorService.TranslateText(inputText.text, chosenLanguage);
    }


    private void OnDestroy()
    {
        enterButton.onClick.RemoveAllListeners();
    }
}
