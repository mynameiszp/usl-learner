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

    [SerializeField]
    private ApiClient apiClient;

    [SerializeField]
    private ApiManager apiManager;

    [SerializeField]
    private DatabaseManager dbManager;
    [SerializeField]
    private GameWindowUI gameWindowUI;

    public override void InstallBindings()
    {
        Container.Bind<GoogleTranslateService>()
            .FromInstance(translateService)
            .AsSingle();

        Container.Bind<ApiClient>()
            .FromInstance(apiClient)
            .AsSingle();

        Container.Bind<ApiManager>()
            .FromInstance(apiManager)
            .AsSingle();

        Container.Bind<DatabaseManager>()
            .FromInstance(dbManager)
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

        Container.Bind<GameWindowUI>()
            .FromInstance(gameWindowUI)
            .AsSingle();
    }
}