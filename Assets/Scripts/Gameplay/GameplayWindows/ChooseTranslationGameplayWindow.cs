using UnityEngine;
using TMPro;
using Zenject;
using System.Linq;
using System;

public class ChooseTranslationGameplayWindow : BasicGameplayWindow
{
    [SerializeField] private TMP_Text[] options;
    [SerializeField] private TMP_Text failText;
    [SerializeField] private GameObject buttons;
    [Inject] private readonly DatabaseManager dbManager;
    private int correctOption;
    private bool isCorrectFirstTime = true;

    public override void SetVisual()
    {
        isCorrectFirstTime = true;
        continueBut.gameObject.SetActive(false);
        translationText.gameObject.SetActive(false);
        failText.gameObject.SetActive(false);
        buttons.SetActive(true);
        handAnimation.AnimationState.SetAnimation(0, word, true);
        handAnimation.gameObject.SetActive(true);
        translationText.text = word;
        var random = new System.Random();
        correctOption = random.Next(0, 3);
        var randomWords = dbManager.serverData.Words
                .Where(w => w.name != word)
                .OrderBy(w => Guid.NewGuid())
                .Take(3)
                .Select(w => w.name)
                .ToList();

        randomWords.Insert(correctOption, word);
        for (int i = 0; i < options.Length; i++)
            options[i].text = randomWords[i];
    }

    public void CheckAnswer(int button){
        if (button == correctOption){
            buttons.SetActive(false);
            failText.gameObject.SetActive(false);
            continueBut.gameObject.SetActive(true);
            translationText.gameObject.SetActive(true);
        }
        else{
            failText.gameObject.SetActive(true);
            isCorrectFirstTime = false;
        }
    }

    public override void Continue(){
        OnContinue?.Invoke(isCorrectFirstTime);
        isCorrectFirstTime = true;
    }
}
