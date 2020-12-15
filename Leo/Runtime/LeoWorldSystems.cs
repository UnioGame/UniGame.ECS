namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime.DataFlow.Interfaces;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;
    using UniCore.Runtime.DataFlow;
    using UniCore.Runtime.Utils;
    using UniRx;


    public class LeoWorldSystems : IDisposable
    {
        private readonly EcsWorld                                         _world;
        private readonly ILeoSystemsFactory                               _factory;
        private readonly LifeTimeDefinition                               _lifeTime     = new LifeTimeDefinition();
        private readonly Dictionary<PlayerLoopTiming,List<EcsSystems>> _updateQueues = new Dictionary<PlayerLoopTiming, List<EcsSystems>>();
        
        public           int                 _id;
        public           List<EcsSystemInfo> _systems = new List<EcsSystemInfo>(8);

        public LeoWorldSystems(EcsWorld world, ILeoSystemsFactory factory)
        {
            _world   = world;
            _factory = factory;
            _id      = world.GetHashCode();

            foreach (var loopTiming in EnumValue<PlayerLoopTiming>.Values)
            {
                if(loopTiming == PlayerLoopTiming.Initialization)
                    continue;
                
                var systems = new List<EcsSystems>(8);
                _updateQueues[loopTiming] = systems;
                RunSystems(loopTiming, systems);
            }
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
            where TSystem : IEcsSystem => ActivateSystem(typeof(TSystem));

        public void ActivateSystem(IEcsSystem system, PlayerLoopTiming updateType = PlayerLoopTiming.Update)
        {
            ActivateSystem(system.GetType(),updateType, system); 
        }

        public void ActivateSystem(Type systemType, PlayerLoopTiming updateType = PlayerLoopTiming.Update)
        {
            ActivateSystem(systemType,updateType, null);
        }
        
        public bool DisableSystem(Type systemType)
        {
            var systemInfo = GetSystemInfo(systemType);
            if (!systemInfo.IsActive)
                return false;
            
            systemInfo.counter--;
            
            if (systemInfo.IsActive)
            {
                return false;
            }

            DestroySystem(systemInfo);
            return true;

        }
        
        public void Dispose()
        {
            foreach (var systemInfo in _systems)
            {
                DestroySystem(systemInfo);
            }

            _updateQueues.Clear();
            _systems.Clear();
            _lifeTime.Terminate();
        }

        private void ActivateSystem(Type systemType,PlayerLoopTiming updateType,IEcsSystem ecsSystem)
        {
            var systemInfo = GetSystemInfo(systemType);
            if (!systemInfo.IsActive)
            {
                var system  = ecsSystem ?? _factory.Create(systemType);
                
                var systems = system is IEcsRunSystem ?
                    new EcsSystems(_world).Add(system,systemType.Name):
                    new EcsSystems(_world).Add(system);
                
                systemInfo.systems   = systems;
                systemInfo.ecsSystem = system;
                systems.Init();
                
                _updateQueues[updateType].Add(systems);
            }

            systemInfo.counter++;
        }
        
        private EcsSystemInfo GetSystemInfo(Type systemType)
        {
            var info = _systems.FirstOrDefault(x => x.ecsSystem?.GetType() == systemType);
            if (info!=null)
                return info;
            
            info = new EcsSystemInfo()
            {
                counter   = 0,
                type      = systemType,
            };
            
            _systems.Add(info);
            return info;
        }
        
        private void DestroySystem(EcsSystemInfo system)
        {
            _systems.Remove(system);
            
            var ecsSystems = system.systems;
            _updateQueues[system.updateType].Remove(ecsSystems);
            
            ecsSystems.Destroy();
            system.counter = 0;
        }


        private async UniTask RunSystems(PlayerLoopTiming updateType, List<EcsSystems> systemInfos)
        {
            while (_lifeTime.IsTerminated == false)
            {
                await UniTask.Yield(updateType);
                foreach (var system in systemInfos)
                {
                    system.Run();
                }
            }

        }
        
    }
    
    
    
}