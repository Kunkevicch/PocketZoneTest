using PocketZoneTest;
using Zenject;

public class CoreSceneGameplayInstaller : MonoInstaller
{
    [UnityEngine.SerializeField] private ObjectForPooling _objectsForPooling;
    [UnityEngine.SerializeField] private int _inventorySize;
    [UnityEngine.SerializeField] private AvailableItem _availableItems;
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
            .FromComponentInHierarchy()
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