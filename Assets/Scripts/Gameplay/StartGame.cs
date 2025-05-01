using UnityEngine;
using Zenject;
using TMPro;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] private TMP_Text levelNum;
    [SerializeField] private Image background;
    [SerializeField] private Sprite availableIcon;
    [SerializeField] private Sprite notAvailableIcon;
    [Inject] private GameplayManager gpManager;
    [Inject] private readonly DatabaseManager dbManager;
    private LevelConfig lvlConfig;

    public void Initialize(LevelConfig config)
    {
        lvlConfig = config;
        levelNum.text = config.levelNumber.ToString();
        bool isAvailable = dbManager.playerData.curlevel >= config.levelNumber;
        GetComponent<Button>().enabled = isAvailable;
        background.sprite = isAvailable? availableIcon : notAvailableIcon;
    }

    public void StartLevel(){
        gpManager.StartLevel(lvlConfig);
    }
}
