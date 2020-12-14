using Leopotam.Ecs;

namespace UniModules.UniGame.ECS.SimplaeDemo.Systems
{
    using System;
    using Components;
    using UnityEngine;
    using Object = UnityEngine.Object;
    using Random = UnityEngine.Random;

    [Serializable]
    public class MotionTargetInitSystem : IEcsInitSystem
    {
        #region inspetor

        [EcsIgnoreInject] 
        public GameObject prefab;

        public int amount = 100;

        public float radius = 10;

        #endregion

        private readonly EcsWorld _world;

        public void Init()
        {
            for (var i = 0; i < amount; i++)
            {
                var instance  = Object.Instantiate(prefab);
                var transform = instance.transform;
                var position  = Random.insideUnitSphere * radius;
                transform.position = position;
                
                _world.NewEntity().
                    Replace(new TimerComponent()).
                    Replace(new MovementDataComponent()).
                    Replace(new MovementTargetComponent()
                    {
                        Transform = transform,
                        IsMovable = true,
                    });

            }
        }
    }
}