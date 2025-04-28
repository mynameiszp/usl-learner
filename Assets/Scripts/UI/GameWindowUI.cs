using UnityEngine;
using Zenject;

public class GameWindowUI : MonoBehaviour
{
    [Inject] private BlocksGenerator blocksGenerator;

    void Awake()
    {
        blocksGenerator.Generate();
    }
}

