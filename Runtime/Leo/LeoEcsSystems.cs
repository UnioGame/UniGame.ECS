namespace UniModules.UniGame.ECS.Runtime.Leo
{
    using Abstract;
    using Leopotam.Ecs;

    public class LeoEcsSystems : 
        UniEcsSystems<EcsWorld,ILeoEcsSystems,ILeoEcsSystem>, 
        ILeoEcsSystems
    {

        public ILeoEcsSystems Inject<T>(T value)
        {
            if (IsStarted) return this;

            foreach (var ecsSystem in EcsSystems)
            {
                ecsSystem.Inject(value);
            }
            
            foreach (var groupSystem in EcsGroupSystems)
            {
                groupSystem.Inject(value);
            }
            
            return this;
        }

    }
}
