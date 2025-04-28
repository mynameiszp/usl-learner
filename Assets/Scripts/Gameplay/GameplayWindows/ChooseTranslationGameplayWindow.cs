using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ChooseTranslationGameplayWindow : BasicGameplayWindow
{
    [SerializeField] private TMP_Text[] options;
    [SerializeField] private TMP_Text failText;
    [SerializeField] private GameObject buttons;
    private int correctOption;

    public override void SetVisual()
    {
        continueBut.gameObject.SetActive(false);
        translationText.gameObject.SetActive(false);
        failText.gameObject.SetActive(false);
        buttons.SetActive(true);
        handAnimation.AnimationState.SetAnimation(0, word, true);
        handAnimation.gameObject.SetActive(true);
        translationText.text = word;
        var random = new System.Random();
        correctOption = random.Next(0, 3);
        List<string> optText = new() {"мама", "брат", "сестра"}; //get from db
        optText.Insert(correctOption, word);
        for (int i = 0; i < options.Length; i++)
            options[i].text = optText[i];
    }

    public void CheckAnswer(int button){
        if (button == correctOption){
            buttons.SetActive(false);
            failText.gameObject.SetActive(false);
            continueBut.gameObject.SetActive(true);
            translationText.gameObject.SetActive(true);
        }
        else
            failText.gameObject.SetActive(true);
    }

    public override void OnAnswerCorrect()
    {
        base.OnAnswerCorrect();
    }
}
