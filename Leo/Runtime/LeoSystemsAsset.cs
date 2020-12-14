using UnityEngine;

namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ECS.Runtime.Leo;
    using Leopotam.Ecs;

    [CreateAssetMenu(menuName = "GameFlow/Ecs/LeoEcs/EcsSystemsAsset",fileName = nameof(LeoSystemsAsset))]
    public class LeoSystemsAsset : ScriptableObject,
        IEcsSystemsData
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeField]
        public EcsSystemsData systemsData = new EcsSystemsData();
        
        public IReadOnlyList<IEcsSystem> Systems => systemsData.systems;

    }
}
