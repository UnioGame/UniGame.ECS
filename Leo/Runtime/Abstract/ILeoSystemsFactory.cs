namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using Leopotam.Ecs;

    public interface ILeoSystemsFactory : IDisposable
    {
        IEcsSystem Create(Type systemType);
    }
}