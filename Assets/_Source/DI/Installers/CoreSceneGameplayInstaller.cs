using PocketZoneTest;
using UnityEngine;
using Zenject;

public class CoreSceneGameplayInstaller : MonoInstaller
{
    [SerializeField] private ObjectForPooling _objectsForPooling;
    [SerializeField] private int _inventorySize;
    [SerializeField] private AvailableItem _availableItems;
    [SerializeField] private GameObject _playerPrefab;

    public override void InstallBindings()
    {
        BindlSystem();
        BindGameplay();
    }

    private void BindlSystem()
    {
        Container
            .BindInterfacesAndSelfTo<ObjectPool>()
            .AsSingle()
            .WithArguments(_objectsForPooling.Pool)
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<InventoryController>()
            .AsSingle()
            .WithArguments(_inventorySize);

        Container
            .BindInterfacesAndSelfTo<JSONStorageService>()
            .AsSingle();

        Container
            .BindInterfacesAndSelfTo<EventMediator>()
            .AsSingle();
    }

    private void BindGameplay()
    {
        Container
            .BindInterfacesAndSelfTo<Player>()
            .FromComponentInNewPrefab(_playerPrefab)
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<IInput>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<InventoryView>()
            .FromComponentInHierarchy()
            .AsSingle()
            .NonLazy();

        Container
            .BindInterfacesAndSelfTo<InventoryModel>()
            .AsSingle()
            .WithArguments(_availableItems, _inventorySize)
            .NonLazy();
    }
}