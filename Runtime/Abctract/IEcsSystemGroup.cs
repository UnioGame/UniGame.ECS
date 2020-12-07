namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.Interfaces;

    public interface IEcsSystemGroup<TGroup,TSystem> : 
        ILifeTimeContext,
        IValidator, 
        IDisposable
        where TGroup : IEcsSystemGroup<TGroup,TSystem>
    {
        int Id { get; }

        string                 GroupName  { get; }
        
        IReadOnlyList<TSystem> EcsSystems { get; }
        
        IReadOnlyList<TGroup>  SubGroups  { get; }
    }
}