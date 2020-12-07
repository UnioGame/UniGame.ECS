namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System.Collections.Generic;
    using Leo;
    using Leo.Abstract;
    using Leopotam.Ecs;
    using UnityEngine;
    
    public static class LeoEcsSystemsProcessor
    {
        public static readonly List<ILeoEcsSystem>  Systems = new List<ILeoEcsSystem>();
        public static readonly List<ILeoEcsSystems> Groups  = new List<ILeoEcsSystems>();
        
#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        static void Reset()
        {
            Systems.Clear();
            Groups.Clear();
        }

        public static ILeoEcsSystems CreateSystems(this ILeoEcsSystemsData group, ILeoEcsWorld ecsWorld)
        {
            var systems = AddSystems(ecsWorld, group).
                RegisterSystemsGroups(ecsWorld,group.Groups);
#if UNITY_EDITOR
            Groups.Add(systems);
            systems.LifeTime.AddCleanUpAction(() => Groups.Remove(systems)); 
#endif
            return systems;
        }

        public static int GetSystemId(this IEcsSystem ecsSystem) => ecsSystem.GetType().GetHashCode();
        
        private static ILeoEcsSystems AddSystems(this ILeoEcsWorld ecsWorld, ILeoEcsSystemsData group)
        {
            var worldId   = ecsWorld.Id;
            var groupName = group.GroupName;

            var ecsSystems = new LeoEcsSystems().
                Initialize(ecsWorld, group.GroupName);

            foreach (var system in group.EcsSystems)
            {
                if (system == null)
                {
                    Debug.LogError($"LEO ECS System from group: {group.GroupName} is NULL");
                    continue;
                }

                var systemId      = system.GetSystemId();
                var ecsSystemItem = EcsWorldsMap.
                    RegisterUsage(worldId, systemId, () => CreateSystem(ecsWorld,system,groupName));
                
                var ecsSystem     = ecsSystemItem.ecsSystem as ILeoEcsSystem;
                ecsSystems.Add(ecsSystem);
            }

            return ecsSystems;
        }

        private static ILeoEcsSystem CreateSystem(ILeoEcsWorld ecsWorld, IEcsSystem ecsSystem,string groupName)
        {
            var leoSystems = new EcsSystems(ecsWorld.World, groupName);
            leoSystems.Add(ecsSystem);
            var leoSystem = new LeoEcsSystem();
            leoSystem.Initialize(ecsWorld.Id);
            leoSystem.Activate(leoSystems);
            return leoSystem;
        }
        

        private static ILeoEcsSystems RegisterSystemsGroups(this  ILeoEcsSystems parent,ILeoEcsWorld ecsWorld, IReadOnlyList<ILeoEcsSystemsData> groups)
        {

            foreach (var subGroup in groups)
            {
                //bind to parent lifetime
                var system = subGroup.CreateSystems(ecsWorld);
                parent.Add(system);
            }

            return parent;
        }
        
        
    }
}