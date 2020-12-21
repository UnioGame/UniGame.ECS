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
        [SerializeField]
        public List<EcsSystemData> systems = new List<EcsSystemData>();

        
        public IReadOnlyList<IEcsSystemData> Systems => systems;

    }
}
