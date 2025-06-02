using PocketZoneTest;
using UnityEngine;
using Zenject;

public class CoreSceneUIInstaller : MonoInstaller
{
    [SerializeField] private HealthView _healthView;
    [SerializeField] private TextView _ammoView;
    public override void InstallBindings()
    {
        BindPresenters();
    }

    private void BindPresenters()
    {
        Container.BindInterfacesAndSelfTo<HealthPresenter>()
           .AsSingle()
           .WithArguments(_healthView)
           .NonLazy();

        Container.BindInterfacesAndSelfTo<AmmoPresenter>()
            .AsSingle()
            .WithArguments(_ammoView)
            .NonLazy();
    }
}