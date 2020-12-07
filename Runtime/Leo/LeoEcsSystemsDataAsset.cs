namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core.Runtime.ScriptableObjects;
    using Leopotam.Ecs;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameFlow/Ecs/SystemGroup", fileName = nameof(LeoEcsSystemsDataAsset))]
    public class LeoEcsSystemsDataAsset : LifetimeScriptableObject, ILeoEcsSystemsData
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public LeoEcsSystemsData ecsSystemsData = new LeoEcsSystemsData();
        
        public string                            GroupName  => ecsSystemsData.GroupName;
        public IReadOnlyList<IEcsSystem>         EcsSystems => ecsSystemsData.EcsSystems;
        public IReadOnlyList<ILeoEcsSystemsData> Groups     => ecsSystemsData.Groups;

        public bool Validate() => ecsSystemsData.Validate();

        [Conditional("UNITY_EDITOR")]
        public void OnValidate() => Validate();

    }
}