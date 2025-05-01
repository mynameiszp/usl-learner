using UnityEngine;
using TMPro;
using Zenject;
using UnityEngine.UI;

public class ProfileVisual : MonoBehaviour
{
    [Inject] private DatabaseManager databaseManager;
    [SerializeField] private TMP_InputField nameInput;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text scoreText;

    void OnEnable()
    {
        var data = databaseManager.playerData;
        nameInput.text = data.name;
        levelText.text = data.curlevel.ToString();
        scoreText.text = data.score.ToString();
    }

    public void ChangeName(){
        databaseManager.UpdateUserName(nameInput.text);
    }

    public void DeleteAccount(){
        databaseManager.DeletePlayer();
    }
}