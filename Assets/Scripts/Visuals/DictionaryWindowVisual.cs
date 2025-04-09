using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Zenject;
using System.Collections.Generic;
using Spine.Unity;

public class DictionaryWindowVisual : MonoBehaviour
{
    [SerializeField]
    private Button enterButton;
    [SerializeField]
    private Button voiceButton;
    [SerializeField]
    private TMP_InputField inputText;
    [SerializeField]
    private TMP_Text resultText;
    [SerializeField]
    private TMP_Dropdown languageDropdown;
    [SerializeField]
    private SkeletonAnimation handAnimation;

    [Inject] private GoogleTranslateService translatorService;
    private int chosenLanguageIndex;
    private LanguageList supportedLanguages;
    private const string DEFAULT_LANG_CODE = "uk";

    private void Start()
    {
        handAnimation.gameObject.SetActive(false);
        resultText.text = "";
        enterButton.onClick.AddListener(Translate);
        languageDropdown.onValueChanged.AddListener(OnLanguageValueChanged);
        InitLanguageDropdown();

        void OnLanguageValueChanged(int index)
        {
            chosenLanguageIndex = index;
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

            int defaultIndex = 0; 
            for (int i = 0; i < supportedLanguages.data.languages.Length; i++)
            {
                var lang = supportedLanguages.data.languages[i];
                options.Add(new TMP_Dropdown.OptionData(lang.name));

                if (lang.language == DEFAULT_LANG_CODE)
                    defaultIndex = i;
            }
            languageDropdown.AddOptions(options);
            languageDropdown.value = defaultIndex;
            languageDropdown.RefreshShownValue(); 
        }
    }

    private void Translate()
    {
        string srcLang = supportedLanguages.data.languages[chosenLanguageIndex].language;
        if (srcLang == DEFAULT_LANG_CODE)
        {
            DisplayResult(inputText.text);
            return;
        }
        translatorService.TranslateText(inputText.text, srcLang, DisplayResult);

        void DisplayResult(string result)
        {
            string name = $"\"{result}\"";
            resultText.text = name;
            handAnimation.AnimationName = result;
            handAnimation.gameObject.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        enterButton.onClick.RemoveAllListeners();
    }
}
