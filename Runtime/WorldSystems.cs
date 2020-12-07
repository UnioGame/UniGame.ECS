namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using Abctract;
    using UnityEngine;

    [Serializable]
    public class WorldSystems
    {
        public static IUniEcsSystem EmptySystem = new EmptySystem();
        
        public int           id;
        public SystemsMap    systemsCounter = new SystemsMap(8);
        
        public WorldSystems(int worldId)
        {
            id = worldId;
        }

        public IUniEcsSystem GetSystem(int systemId)
        {
            return !Contain(systemId) ? EmptySystem : systemsCounter[systemId].ecsSystem;
        }
        
        public bool Contain(int systemId) => systemsCounter.ContainsKey(systemId);
        
        public int Increase(int systemId)
        {
            if (!systemsCounter.TryGetValue(systemId, out var systemCounter))
            {
                return 0;
            }
            return systemCounter.Increase();
        }

        public IUniEcsSystem RegisterEcsSystems(IUniEcsSystem ecsSystems)
        {
            var systemId = ecsSystems.Id;
            if (systemsCounter.TryGetValue(systemId, out var systemCounter))
            {
                Debug.LogError($"ECS SYSTEM with Type {ecsSystems.GetType().Name} ID {ecsSystems.Id} EXISTS");
                return null;
            }
            
            systemsCounter[systemId] = new EcsSystemCounter().Initialize(ecsSystems);
            return ecsSystems;
        }
        
        public bool RemoveSystem(int systemId)
        {
            if (!systemsCounter.TryGetValue(systemId, out var systemCounter))
            {
                return true;
            }
            var counter = systemCounter.Decrease();
            if (counter <= 0)
            {
                systemCounter.Release();
                var result =systemsCounter.Remove(systemId);
                return result;
            }

            return false;
        }
        
        public override int GetHashCode() => id;

        
        public override bool Equals(object obj)
        {
            if (obj is WorldSystems worldSystems)
                return worldSystems.id == id;
            return base.Equals(obj);
        }
    }
}