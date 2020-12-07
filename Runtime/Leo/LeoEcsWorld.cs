using System;
using Leopotam.Ecs;

namespace UniModules.UniGame.ECS.Runtime.Leo
{
    [Serializable]
    public class LeoEcsWorld : UniEcsWorld<EcsWorld>
    {
        protected override void DestroyWorld()
        {
            World.Destroy();
        }

        protected override EcsWorld CreateConcreteWorld() => new EcsWorld();

    }
}
