namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using System.Collections.Generic;
    using ECS.Leo.Runtime;
    using Leopotam.Ecs;

    public interface IEcsSystemsData
    {
        IReadOnlyList<IEcsSystemData> Systems { get; }
    }
}