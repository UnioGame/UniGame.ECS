namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using Leopotam.Ecs;

    public interface IEcsSystemData
    {
        IEcsSystem    System { get; }
        LeoSystemData Info   { get; }
    }
}