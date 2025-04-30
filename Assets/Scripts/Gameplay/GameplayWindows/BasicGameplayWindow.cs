using UnityEngine;
using TMPro;
using Spine.Unity;
using UnityEngine.UI;
using System;

public class BasicGameplayWindow : MonoBehaviour
{
    public Action OnContinue;
    [SerializeField] protected TMP_Text lvlNumber;
    [SerializeField] protected TMP_Text translationText;
    [SerializeField] protected SkeletonAnimation handAnimation;
    [SerializeField] protected Button continueBut;

    protected string word;

    public void Initialize(int lvlNum, string word){
        continueBut.onClick.RemoveAllListeners();
        lvlNumber.text = string.Format(lvlNumber.text, lvlNum);
        this.word = word;
        continueBut.onClick.AddListener(Continue);
    }

    public virtual void SetVisual(){
        translationText.text = word;
        handAnimation.AnimationState.SetAnimation(0, word, true);
        handAnimation.gameObject.SetActive(true);
    }

    public virtual void OnAnswerCorrect(){}

    public void Continue(){
        OnContinue?.Invoke();
    }
}