namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System.Collections.Generic;
    using Leo.Abstract;
    using Leopotam.Ecs;
    using UnityEngine;


    public static class EcsSystemsMap
    {
        public static readonly List<ILeoEcsSystem> Systems          = new List<ILeoEcsSystem>();
        public static readonly List<ILeoEcsSystemGroup> Groups = new List<ILeoEcsSystemGroup>();
        

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        static void Reset()
        {
            Systems.Clear();
            Groups.Clear();
        }

        
        public static EcsSystems AddSystems(this ILeoEcsWorld ecsWorld, ILeoEcsSystemGroup group)
        {
            var world    = ecsWorld.World;

            var groupName      = group.GroupName;
            var ecsSystemGroup = new EcsSystems(world, groupName);

            foreach (var system in group.EcsSystems)
            {
                if (system == null)
                {
                    Debug.LogError($"LEO ECS System from group: {group.GroupName} is NULL");
                    continue;
                }
                ecsSystemGroup.Add(system, system.IsRunSystem ? system.SystemName : string.Empty);
            }

            return ecsSystemGroup;
        }

        public static void RegisterSystemsGroups(this ILeoEcsWorld ecsWorld, ILeoEcsSystemGroup parent, IReadOnlyList<ILeoEcsSystemGroup> groups)
        {
            var lifeTime = parent.LifeTime;
            
            foreach (var subGroup in groups)
            {
                //bind to parent lifetime
                var system = subGroup.RegisterGroup(ecsWorld, true);
                lifeTime.AddDispose(system);
            }
        }
        
    }
}