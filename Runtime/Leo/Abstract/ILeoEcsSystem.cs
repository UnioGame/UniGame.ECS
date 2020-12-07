using Leopotam.Ecs;
using UniModules.UniGame.ECS.Runtime.Abctract;

namespace UniModules.UniGame.ECS.Runtime.Leo.Abstract
{
    public interface ILeoEcsSystem : 
        IUniEcsSystem<IEcsSystem>, 
        IEcsSystem
    {
        bool IsRunSystem { get; }

        string SystemName { get; }
    }
}
