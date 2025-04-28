using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField]
    private GoogleTranslateService translateService;

    [SerializeField]
    private DBController dBController;

    [SerializeField]
    private UIWindowManager uiWindowManager;

    [SerializeField]
    private GameplayManager gameplayManager;
    
    [SerializeField]
    private LevelSettings levelSettings;
    [SerializeField]
    private BlocksGenerator blocksGenerator;

    public override void InstallBindings()
    {
        Container.Bind<GoogleTranslateService>()
            .FromInstance(translateService)
            .AsSingle();

        Container.Bind<DBController>()
            .FromInstance(dBController)
            .AsSingle();

        Container.Bind<UIWindowManager>()
            .FromInstance(uiWindowManager)
            .AsSingle();

        Container.Bind<GameplayManager>()
            .FromInstance(gameplayManager)
            .AsSingle();

        Container.Bind<LevelSettings>()
            .FromInstance(levelSettings)
            .AsSingle();

        Container.Bind<BlocksGenerator>()
            .FromInstance(blocksGenerator)
            .AsSingle();
    }
}