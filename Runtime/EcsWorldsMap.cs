namespace UniModules.UniGame.ECS.Runtime
{
    using System.Collections.Generic;
    using Abctract;
    using Nodes;

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

        public static bool HasSystem(int world, int systemId) => _worldSystems.TryGetValue(world, out var systems) && systems.Contain(systemId);
        
        public static int RegisterSystem(int worldId, int systemId)
        {
            return _worldSystems.TryGetValue(worldId, out var systems) ? systems.AddSystem(systemId) : 0;
        }
        
        public static int RemoveSystem(int worldId, int systemId)
        {
            return _worldSystems.TryGetValue(worldId, out var systems) ? systems.RemoveSystem(systemId) : 0;
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
