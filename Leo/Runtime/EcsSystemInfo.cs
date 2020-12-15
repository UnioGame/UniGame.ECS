namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;

    public class EcsSystemInfo
    {
        public PlayerLoopTiming updateType = PlayerLoopTiming.Update;
        public Type             type;
        public EcsSystems       systems;
        public IEcsSystem       ecsSystem;
        public int              counter;

        public bool IsActive => counter > 0;
    }
}