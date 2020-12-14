using System;
using Leopotam.Ecs;

namespace UniModules.UniGame.ECS.SimplaeDemo.Systems
{
    using Components;
    using UnityEngine;

    [Serializable]
    public class MotionTargetSystem : IEcsRunSystem
        //, IEcsInitSystem
    {
        private readonly EcsWorld                                                                 _world    = null;

        //[EcsIgnoreInject]
        private EcsFilter<MovementDataComponent, MovementTargetComponent> _ecsFilter = null;
        
        public void Init()
        {
            _ecsFilter = _world.GetFilter(typeof(EcsFilter<MovementDataComponent, MovementTargetComponent>), true) 
                as EcsFilter<MovementDataComponent, MovementTargetComponent>;
        }
        
        public void Run()
        {
            
            foreach (var id in _ecsFilter)
            {
                var     entity       = _ecsFilter.GetEntity(id);
                ref var movement     = ref entity.Get<MovementTargetComponent>();
                ref var movementData = ref entity.Get<MovementDataComponent>();

                movement.Transform.position += movementData.Direction * movementData.Speed * Time.deltaTime;
            }
        }

    }
}