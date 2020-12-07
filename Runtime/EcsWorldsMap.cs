namespace UniModules.UniGame.ECS.Runtime
{
    using System;
    using System.Collections.Generic;
    using Abctract;
    using Nodes;
    using UnityEngine;

    public static class EcsWorldsMap 
    {
        private static Dictionary<int, WorldSystems> _worldSystems     = new Dictionary<int, WorldSystems>(8);

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        public static void Reset()
        {
            _worldSystems?.Clear();
        }

        public static bool ContainsSystem(int world, int systemId) => _worldSystems.TryGetValue(world, out var systems) && systems.Contain(systemId);
        
        public static (IUniEcsSystem ecsSystem,bool isNew) RegisterUsage(int worldId,int systemId, Func<IUniEcsSystem> factory)
        {
            var world    = GetWorldSystems(worldId);
            if (world.Contain(systemId))
            {
                world.Increase(systemId);
                return (world.GetSystem(systemId),false);
            }

            return (RegisterSystem(worldId, factory()),true);
        }
        
        public static int RegisterUsage(int worldId, int systemId)
        {
            var world = GetWorldSystems(worldId);
            return world.Increase(systemId);
        }
        
        public static IUniEcsSystem RegisterSystem(int worldId, IUniEcsSystem system)
        {
            var world = GetWorldSystems(worldId);
            return world.RegisterEcsSystems(system);
        }
        
        public static bool RemoveSystem(int worldId,int systemsId)
        {
            var world = GetWorldSystems(worldId);
            return world.RemoveSystem(systemsId);
        }

        public static WorldSystems GetWorldSystems(int worldId)
        {
            if (!_worldSystems.TryGetValue(worldId, out var world))
            {
                Debug.LogError($"ECS WORLD with id {worldId} NOT FROUND");
            }

            return world;
        }
        
        public static bool DestroyWorld<TWorld>(this IUniEcsWorld<TWorld> world)
        {
            var result = _worldSystems.Remove(world.Id);
            world.Dispose();
            return result;
        }

        public static bool RegisterWorld<TWorld>(this IUniEcsWorld<TWorld> world)
        {
            var id = world.Id;
            if (_worldSystems.TryGetValue(id, out var systems))
            {
                return false;
            }
            systems           = new WorldSystems(id);
            _worldSystems[id] = systems;
            
            //cleanup world when it die
            world.LifeTime.AddCleanUpAction(() => world.DestroyWorld());
            
            return true;
        }
        
    }
}
