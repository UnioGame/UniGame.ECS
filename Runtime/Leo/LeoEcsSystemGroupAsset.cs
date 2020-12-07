namespace UniModules.UniGame.ECS.Runtime.Nodes
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Core.Runtime.ScriptableObjects;
    using Leo.Abstract;
    using Leopotam.Ecs;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/GameFlow/Ecs/SystemGroup", fileName = nameof(LeoEcsSystemGroupAsset))]
    public class LeoEcsSystemGroupAsset : LifetimeScriptableObject, ILeoEcsSystemGroup
    {
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.InlineProperty]
        [Sirenix.OdinInspector.HideLabel]
#endif
        public LeoEcsSystemGroup ecsSystemGroup = new LeoEcsSystemGroup();


        public int Id => ecsSystemGroup.Id;

        public string                            GroupName     => ecsSystemGroup.GroupName;
        public IReadOnlyList<ILeoEcsSystem>      EcsSystems    => ecsSystemGroup.EcsSystems;
        public IReadOnlyList<ILeoEcsSystemGroup> SubGroups     => ecsSystemGroup.SubGroups;
        public EcsSystems                        LeoEcsSystems => ecsSystemGroup.LeoEcsSystems;
        public ILeoEcsWorld                      World         => ecsSystemGroup.World;

        public ILeoEcsSystemGroup RegisterGroup(ILeoEcsWorld world, bool isNestedGroup) => ecsSystemGroup.RegisterGroup(world, isNestedGroup);

        public ILeoEcsSystemGroup Inject<T>(T value) => ecsSystemGroup.Inject(value);

        public void Init() => ecsSystemGroup.Init();

        public bool Validate() => ecsSystemGroup.Validate();

        [Conditional("UNITY_EDITOR")]
        public void OnValidate() => Validate();

        public void Dispose() => ecsSystemGroup.Dispose();
        
        protected override void OnActivate()
        {
            base.OnActivate();
            LifeTime.AddDispose(this);
        }
    }
}