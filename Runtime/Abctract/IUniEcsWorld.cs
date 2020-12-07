namespace UniModules.UniGame.ECS.Runtime.Abctract
{
    using System;
    using Core.Runtime.Interfaces;

    public interface IUniEcsWorld<TWorld> : 
        ILifeTimeContext, 
        IDisposable
    {

        int Id { get; }

        TWorld World { get; }
        
    }
}
