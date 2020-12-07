namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using Abctract;
    using Abstract;
    using Leopotam.Ecs;

    public interface ILeoEcsSystems : IUniEcsSystems<EcsWorld,ILeoEcsSystems,ILeoEcsSystem>
    {
        ILeoEcsSystems Inject<T>(T value);

    }
}