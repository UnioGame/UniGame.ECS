using System;
using Leopotam.Ecs;

namespace UniModules.UniGame.ECS.SimplaeDemo.Systems
{
    using Components;
    using UnityEngine;

    [Serializable]
    public class MotionTargetSystem : IEcsRunSystem
    {
        private readonly EcsWorld                                                                 _world    = null;

        private EcsFilter<MovementDataComponent, MovementTargetComponent> _ecsFilter = null;

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