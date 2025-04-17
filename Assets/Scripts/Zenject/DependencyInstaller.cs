using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField]
    private GoogleTranslateService translateService;

    [SerializeField]
    private DBController dBController;

    public override void InstallBindings()
    {
        Container.Bind<GoogleTranslateService>()
            .FromInstance(translateService)
            .AsSingle();

        Container.Bind<DBController>()
            .FromInstance(dBController)
            .AsSingle();
    }
}