namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using Leo.Abstract;
    using Leopotam.Ecs;

    public interface ILeoEcsSystemGroup : IEcsSystemGroup<ILeoEcsSystemGroup,ILeoEcsSystem>
    {
        EcsSystems LeoEcsSystems { get; }

        ILeoEcsWorld World { get; }
        
        ILeoEcsSystemGroup RegisterGroup(ILeoEcsWorld world, bool isNestedGroup);

        ILeoEcsSystemGroup Inject<T>(T value);

        void Init();
    }
}