using UnityEngine;

namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Runtime.DataFlow.Interfaces;
    using global::UniCore.Runtime.ProfilerTools;
    using Leopotam.Ecs;
    using UniCore.Runtime.DataFlow;
    using UnityEditor;

    public static class LeoEcsSystemsManager
    {

        private static LeoEcsSettingsAsset mainSystems;

        private static List<LeoWorldSystems> _worlds = new List<LeoWorldSystems>();

        [MenuItem("GameFlow/Ecs/LeoEcs/Stop Worlds")]
        public static void DestroyWorlds()
        {
            var activeWorlds = _worlds.ToList();
            activeWorlds.ForEach(x => x.Dispose());
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize()
        {
            mainSystems = Resources.Load<LeoEcsSettingsAsset>(nameof(LeoEcsSettingsAsset));
        }

        public static void DestroyWorld(this EcsWorld world)
        {
            var existsWorld = _worlds.FirstOrDefault(x => x.World == world);
            existsWorld?.Dispose();
        }

        public static ILifeTime InitializeWorld(this EcsWorld world)
        {
#if UNITY_EDITOR
            if (!mainSystems)
            {
                GameLog.LogError($"UNIGAME: LEOECS Missings Systems Asset");
                return LifeTime.TerminatedLifetime;
            }
#endif
            var existsWorld = _worlds.FirstOrDefault(x => x.World == world);
            if (existsWorld != null) return existsWorld.LifeTime;
            
            InitializeAllSystems(world);

            var systemFactory = mainSystems.CreateSystemsFactory();
            var leoWorld      = new LeoWorldSystems(world,systemFactory);
            var lifeTime      = leoWorld.LifeTime;
            _worlds.Add(leoWorld);

            lifeTime.AddDispose(systemFactory);
            lifeTime.AddCleanUpAction(() => _worlds.Remove(leoWorld));

            return leoWorld.LifeTime;
        }

        public static void DisableSystem(this EcsWorld world, Type systemType)
        {
            var ecsWorld = GetWorld(world);
            ecsWorld.DisableSystem(systemType);
        }
        
        //activate system into the world
        public static void ActivateSystem<TSystem>(this EcsWorld world)
        {
            ActivateSystem(world, typeof(TSystem));
        }

        public static void ActivateSystem(this EcsWorld world, Type systemType)
        {
            var ecsWorld = GetWorld(world);
            ecsWorld.ActivateSystem(systemType);
        }

        private static LeoWorldSystems GetWorld(EcsWorld world)
        {
            var ecsWorld = _worlds.FirstOrDefault(x => x.World == world);
            if (ecsWorld == null)
                InitializeWorld(world);
            return _worlds.FirstOrDefault(x => x.World == world);
        }

        private static void InitializeAllSystems(EcsWorld world)
        {
            //register all system into world for filter creation
            var ecsSystems = new EcsSystems(world);
            foreach (var system in mainSystems.Systems)
            {
                ecsSystems.Add(system);
            }
            ecsSystems.Init();
            //destroy initialized systems
            ecsSystems.Destroy();
        }
        
    }
}
