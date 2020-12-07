namespace UniModules.UniGame.ECS.Runtime.Abctract
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.Interfaces;
    using UniRx;

    public interface IUniEcsSystems<TWorld,TSystemsGroup,TSystem> : IDisposable, ILifeTimeContext
        where TSystemsGroup : class,IUniEcsSystems<TWorld,TSystemsGroup,TSystem>
        where TSystem : IUniEcsSystem
    {
        IUniEcsWorld<TWorld> World { get; }

        IReadOnlyReactiveCollection<TSystem> EcsSystems { get; }

        IReadOnlyReactiveCollection<TSystemsGroup> EcsGroupSystems { get; }

        string Name { get; }
        
        bool IsStarted { get; }

        TSystemsGroup Execute();
        
        TSystemsGroup Add(TSystem system);

        TSystemsGroup Add(TSystemsGroup systems);
        
    }
}
