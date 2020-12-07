namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System;
    using UniCore.Runtime.ObjectPool.Runtime.Extensions;

    [Serializable]
    public class WorldSystems
    {
        public int        id;
        public SystemsMap systems = new SystemsMap(8);

        public WorldSystems(int worldId)
        {
            id = worldId;
        }

        public bool Contain(int systemId) => systems.ContainsKey(systemId);
        
        public int AddSystem(int systemId)
        {
            if (!systems.TryGetValue(systemId, out var systemCounter))
            {
                systemCounter     = new SystemCounter().Initialize(systemId);
                systems[systemId] = systemCounter;
            }

            return systemCounter.Increase();
        }

        public int RemoveSystem(int systemId)
        {
            if (!systems.TryGetValue(systemId, out var systemCounter))
            {
                return 0;
            }
            var counter = systemCounter.Decrease();
            if (counter <= 0)
            {
                systemCounter.Despawn();
                systems.Remove(systemId);
            }

            return counter;
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