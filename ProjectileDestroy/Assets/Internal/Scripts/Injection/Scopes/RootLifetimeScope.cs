using UnityEngine.Serialization;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [FormerlySerializedAs("lifetimeScopePrefab")] public GamePlayTimeScope playTimeScopePrefab;
    protected override void Configure(IContainerBuilder builder)
    {
       
    }
}
