using UnityEngine;
using Zenject;

public class GameWindowUI : MonoBehaviour
{
    [Inject] private BlocksGenerator blocksGenerator;

    void Start()
    {
        Generate();
    }

    public void Generate(){
        StartCoroutine(blocksGenerator.Generate());
    }
}

