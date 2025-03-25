using UnityEngine;
using Zenject;

public class DependencyInstaller : MonoInstaller
{
    [SerializeField]
    private GoogleTranslateService translateService;

    public override void InstallBindings()
    {
        Container.Bind<GoogleTranslateService>()
            .FromInstance(translateService)
            .AsSingle();
    }
}