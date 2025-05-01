using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnterTranslationGameplayWindow : BasicGameplayWindow
{
    [SerializeField] private TMP_InputField inputText;
    [SerializeField] private Button enterButton;
    [SerializeField] private TMP_Text failText;
    private bool isCorrectFirstTime = true;

    public override void SetVisual()
    {
        handAnimation.AnimationState.SetAnimation(0, word, true);
        handAnimation.gameObject.SetActive(true);
        failText.gameObject.SetActive(false);
        continueBut.gameObject.SetActive(false);
        inputText.text = "";
        enterButton.enabled = true;
        isCorrectFirstTime = true;

        enterButton.onClick.AddListener(CheckAnswer);
    }

    private void CheckAnswer(){
        if (inputText.text.Trim().ToLower() == word) //check for synonyms??
        {
            failText.gameObject.SetActive(false);
            continueBut.gameObject.SetActive(true);
            enterButton.enabled = false;
        }
        else
        {
            failText.gameObject.SetActive(true);
            isCorrectFirstTime = false;
        }
    }

    public override void Continue(){
        OnContinue?.Invoke(isCorrectFirstTime);
    }
}