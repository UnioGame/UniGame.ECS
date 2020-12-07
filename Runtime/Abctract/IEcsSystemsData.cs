namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.Interfaces;

    public interface IEcsSystemsData<TGroup,TSystem> : 
        IValidator 
        where TGroup : IEcsSystemsData<TGroup,TSystem>
    {
        string                 GroupName  { get; }
        
        IReadOnlyList<TSystem> EcsSystems { get; }
        
        IReadOnlyList<TGroup>  Groups  { get; }
    }
}