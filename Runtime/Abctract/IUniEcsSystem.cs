namespace UniModules.UniGame.ECS.Runtime.Abctract
{
    using System;
    using Core.Runtime.Interfaces;

    public interface IUniEcsSystem : 
        ILifeTimeContext,
        IDisposable
    {
        int  Id       { get; }
        
        int   WorldId  { get; }
            
        bool IsActive { get; }

        bool Execute();

        bool Initialize(int worldId);
        
    }
}
