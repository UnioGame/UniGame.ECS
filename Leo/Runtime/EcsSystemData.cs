namespace UniModules.UniGame.ECS.Leo.Runtime
{
    using System;
    using Cysharp.Threading.Tasks;
    using Leopotam.Ecs;
    using UnityEngine;

    [Serializable]
    public class EcsSystemData : IEcsSystemData
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public LeoSystemData systemData = new LeoSystemData(){updateType = PlayerLoopTiming.Update};
        
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        [SerializeReference]
        public IEcsSystem system;

        public IEcsSystem System => system;

        public LeoSystemData Info => systemData;
    }
}