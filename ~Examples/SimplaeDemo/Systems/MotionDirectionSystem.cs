using System;
using Leopotam.Ecs;

namespace UniModules.UniGame.ECS.SimplaeDemo.Systems
{
    using Components;
    using UnityEngine;

    [Serializable]
    public class MotionDirectionSystem : IEcsRunSystem
        //, IEcsInitSystem
    {
        [SerializeField] public float changeDirectionDelay = 2f;

        [Range(0, 5)] public float speedLimits = 5f;

        public EcsWorld _world = null;

        private EcsFilter<TimerComponent, MovementDataComponent, MovementTargetComponent> _filter = null;

        public void Run()
        {
            foreach (var id in _filter)
            {
                var entity = _filter.GetEntity(id);

                ref var timer    = ref entity.Get<TimerComponent>();
                ref var movement = ref entity.Get<MovementDataComponent>();

                timer.LastTime += Time.deltaTime;

                if (!(timer.LastTime > changeDirectionDelay))
                {
                    continue;
                }

                timer.LastTime = 0;
                var direction = Random.insideUnitSphere;
                movement.Direction = direction;
                movement.Speed     = Random.Range(0, speedLimits);
            }
        }
    }
}