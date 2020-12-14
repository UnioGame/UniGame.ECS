namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using System.Collections.Generic;
    using Leopotam.Ecs;

    public interface IEcsSystemsData
    {
        IReadOnlyList<IEcsSystem> Systems { get; }
    }
}