using Leopotam.Ecs;
using UniModules.UniGame.ECS.Runtime.Abctract;

namespace UniModules.UniGame.ECS.Runtime.Leo.Abstract
{
    public interface ILeoEcsSystem : 
        IUniEcsSystem
    {

        EcsSystems EcsSystems { get; }

        IEcsSystem EcsSystem { get; }
        
        bool       IsRunSystem { get; }

        string SystemName { get; }

        ILeoEcsSystem Inject<TData>(TData value);
    }
}
