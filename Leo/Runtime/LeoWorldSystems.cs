namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using Core.Runtime.DataFlow.Interfaces;
    using Leopotam.Ecs;
    using UniCore.Runtime.DataFlow;


    public class LeoWorldSystems : IDisposable
    {
        private readonly EcsWorld                        _world;
        private readonly ILeoSystemsFactory              _factory;
        private readonly LifeTimeDefinition              _lifeTime = new LifeTimeDefinition();
        public           int                             _id;
        public           Dictionary<Type, EcsSystemInfo> _systems = new Dictionary<Type, EcsSystemInfo>(8);

        public LeoWorldSystems(EcsWorld world, ILeoSystemsFactory factory)
        {
            _world   = world;
            _factory = factory;
            _id      = world.GetHashCode();
        }

        public EcsWorld World => _world;

        public int ID => _id;

        public ILifeTime LifeTime => _lifeTime;

        public bool IsActive<TSystem>()
            where TSystem : IEcsSystem
        {
            return IsActive(typeof(TSystem));
        }

        public bool IsActive(Type systemType)
        {
            return GetSystemInfo(systemType).IsActive;
        }

        public void ActivateSystem<TSystem>()
            where TSystem : IEcsSystem
        {
            ActivateSystem(typeof(TSystem));
        }

        public void ActivateSystem(Type systemType)
        {
            var systemInfo = GetSystemInfo(systemType);
            if (!systemInfo.IsActive)
            {
                var ecsSystems = systemInfo.systems;
                ecsSystems.Init();
            }

            systemInfo.counter++;
        }

        public bool DisableSystem(Type systemType)
        {
            var systemInfo = GetSystemInfo(systemType);
            if (!systemInfo.IsActive)
                return false;
            systemInfo.counter--;
            if (!systemInfo.IsActive)
            {
                DestroySystem(systemInfo);
                return true;
            }

            return false;
        }


        public void Dispose()
        {
            foreach (var systemInfo in _systems)
            {
                DestroySystem(systemInfo.Value);
            }

            _systems.Clear();
            
            _lifeTime.Terminate();
        }

        private EcsSystemInfo GetSystemInfo(Type systemType)
        {
            if (!_systems.TryGetValue(systemType, out var info))
            {
                var system  = _factory.Create(systemType);
                var systems = new EcsSystems(_world).Add(system);
                info = new EcsSystemInfo()
                {
                    counter   = 0,
                    type      = systemType,
                    ecsSystem = system,
                    systems   = systems
                };
                _systems[systemType] = info;
            }

            return info;
        }


        private void DestroySystem(EcsSystemInfo system)
        {
            _systems.Remove(system.type);
            system.systems.Destroy();
            system.counter = 0;
        }
    }

    public class EcsSystemInfo
    {
        public Type       type;
        public EcsSystems systems;
        public IEcsSystem ecsSystem;
        public int        counter;

        public bool IsActive => counter > 0;
    }
}