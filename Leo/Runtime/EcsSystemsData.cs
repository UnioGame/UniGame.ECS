namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using ECS.Runtime.Leo;
    using Leopotam.Ecs;
    using UnityEngine;

    [Serializable]
    public class EcsSystemsData : IEcsSystemsData
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.ListDrawerSettings(Expanded = true)]
#endif
        [SerializeReference]
        public List<IEcsSystem> systems = new List<IEcsSystem>();

        
        public IReadOnlyList<IEcsSystem> Systems => systems;

    }
}
